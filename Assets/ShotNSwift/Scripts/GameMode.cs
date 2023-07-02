// Oleg Kotov

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public delegate void GameOverDelegate();
    public event GameOverDelegate GameOverEvent;

    public List<GameObject> leftSidePoints = new List<GameObject>();
    public List<GameObject> rightSidePoints = new List<GameObject>();

    public GameObject playerPrefab;
    public GameObject targetPrefab;
    public GameObject countdownBarPrefab;

    public GameObject baseHud;
    public GameObject experienceHud;
    public GameObject gameOverHud;

    public GameOverHudController gameOverHudController;

    public GameObject pointGrid;
    public GameObject spaceDebris;
    private SpaceDebrisSpawner spaceDebrisSpawner;

    private GameObject player;
    private GameObject target;
    private GameObject countdownBar;

    private PlayerController playerController;

    private bool moveToRightSide = true;

    private PointerController pointerController;
    private CountdownHudController countdownHudController;
    public ExperienceHudController experienceHudController;

    private CameraShake cameraShake;

    private ReactionTimeStatistics reactionTimeStats = new ReactionTimeStatistics();

    private enum GameState
    {
        MainMenu,
        Gameplay,
        Pause,
        GameOver
    }

    private GameState gameState = GameState.MainMenu;

    void Start()
    {
        Application.targetFrameRate = 120;

        GameStats.Instance.ResetLevelStats();
        InitGameplay();
        gameState = GameState.Gameplay;

        gameOverHudController = gameOverHud.GetComponent<GameOverHudController>();

        reactionTimeStats.StartMeasure();
    }

    private void InitGameplay()
    {
        ShowPointGrid();
        ShowExperienceHud();
        SpawnPlayer();
        SpawnTarget();
        SpawnCountdownHud();

        Vector3 direction = ( target.transform.position - player.transform.position ).normalized;
        SetPointerDirection( new Vector2( direction.x, direction.y ) );

        spaceDebrisSpawner = spaceDebris.GetComponent<SpaceDebrisSpawner>();
        EnableSpaceDebrisSpawner();

        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void ShowPointGrid()
    {
        pointGrid.SetActive( true );
    }

    private void HidePointGrid()
    {
        pointGrid.SetActive( false );
    }

    private void ShowExperienceHud()
    {
        experienceHud.SetActive( true );
    }

    private void HideExperienceHud()
    {
        experienceHud.SetActive( false );
    }

    private void EnableSpaceDebrisSpawner()
    {
        spaceDebrisSpawner.enabled = true;
    }

    private void DisableSpaceDebrisSpawner()
    {
        spaceDebrisSpawner.enabled = false;
    }

    void Update()
    {
        switch ( gameState )
        {
            case GameState.Gameplay:
                UpdateGameplay();
                break;
            case GameState.GameOver:
                UpdateGameOver();
                break;
        }
    }

    void UpdateGameplay()
    {
        if ( Gameplay.IsScreenTouched() && !playerController.IsMoving )
        {
            AudioManager.instance.PlayTapSound();
            playerController.MoveToTarget( target.transform.position );
            reactionTimeStats.StopMeasure();
        }
    }

    void UpdateGameOver()
    {
        if ( Gameplay.IsScreenTouched() )
        {
            SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        }
    }

    void OnDestroy()
    {
        playerController.PlayerTargetReached -= OnPlayerTargetReached;
        playerController.PlayerObstacleCollision -= OnPlayerCollisionWithObstacle;
    }

    private void SpawnPlayer()
    {
        GameObject point = leftSidePoints[0];
        player = Instantiate( playerPrefab, point.transform.position, Quaternion.identity );

        pointerController = player.transform.Find( "pointer" ).gameObject.GetComponent<PointerController>();

        playerController = player.GetComponent<PlayerController>();
        playerController.PlayerTargetReached += OnPlayerTargetReached;
        playerController.PlayerObstacleCollision += OnPlayerCollisionWithObstacle;
    }

    private void SpawnTarget()
    {
        int randomIndex = Random.Range( 0, rightSidePoints.Count );
        GameObject point = rightSidePoints[randomIndex];
        target = Instantiate( targetPrefab, point.transform.position, Quaternion.identity );
    }

    private void SpawnCountdownHud()
    {
        Vector3 spawnPosition = target.transform.position;
        countdownBar = Instantiate( countdownBarPrefab );

        countdownHudController = countdownBar.GetComponent<CountdownHudController>();
        countdownHudController.SetPosition( spawnPosition );
        countdownHudController.CountdownExpired += OnCountdownExpired;
    }

    private void SetPointerDirection( Vector2 direction )
    {
        pointerController.SetDirection( direction );
    }

    private void GameOver()
    {
        gameState = GameState.GameOver;

        Destroy( player );
        Destroy( target );
        Destroy( countdownBar );

        HideExperienceHud();
        HidePointGrid();
        DisableSpaceDebrisSpawner();
        ShowGameOverHud();

        GameOverEvent?.Invoke();

        gameOverHudController.UpdateHudData();

        GameStats.Instance.OnGameOver();

        // Debug.Log( "Reaction min time: " + reactionTimeStats.GetMinTime() );
        // Debug.Log( "Reaction avg time: " + reactionTimeStats.GetAvgTime() );
        // Debug.Log( "Reaction max time: " + reactionTimeStats.GetMaxTime() );
    }

    private void ShowGameOverHud()
    {
        gameOverHud.SetActive( true );
    }

    private void OnPlayerTargetReached( Vector3 moveDirection )
    {
        GameStats.Instance.AddJump();

        moveToRightSide = !moveToRightSide;

        int randomIndex = Random.Range( 0, leftSidePoints.Count );
        GameObject point = ( moveToRightSide ) ? rightSidePoints[randomIndex] : leftSidePoints[randomIndex];

        target.transform.position = point.transform.position;

        countdownHudController.SetPosition( target.transform.position );
        countdownHudController.ResetTimer();

        Vector3 direction = ( target.transform.position - player.transform.position ).normalized;
        SetPointerDirection( new Vector2( direction.x, direction.y ) );

        moveDirection = -moveDirection;

        cameraShake.StartShake( moveDirection );

        experienceHudController.OnPlayerTargetReached();

        reactionTimeStats.StartMeasure();
    }

    private void OnPlayerCollisionWithObstacle()
    {
        GameOver();
    }

    private void OnCountdownExpired()
    {
        GameOver();
    }
}

