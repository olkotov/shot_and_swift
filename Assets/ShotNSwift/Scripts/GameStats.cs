// Oleg Kotov

using UnityEngine;

public class GameStats : MonoBehaviour
{
    public delegate void StatChangedDelegate( int statValue );

    public event StatChangedDelegate CoinStatChangedEvent;
    public event StatChangedDelegate JumpStatChangedEvent;

    private int coinCount = 0;
    private int jumpCount = 0;
    private int bestScore = 0;

    public int CoinCount => coinCount;
    public int JumpCount => jumpCount;
    public int BestScore => bestScore;

    private static GameStats instance;

    public static GameStats Instance
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<GameStats>();

                if ( instance == null )
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "game_stats";
                    instance = gameObject.AddComponent<GameStats>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if ( instance != null && instance != this )
        {
            Destroy( gameObject );
            return;
        }

        instance = this;
        DontDestroyOnLoad( gameObject );
    }

    void Start()
    {
        // test only
        // PlayerPrefs.DeleteAll();

        LoadBestScore();
        ResetLevelStats();
    }

    public void AddCoin()
    {
        coinCount++;
        CoinStatChangedEvent?.Invoke( coinCount );
    }

    public void AddJump()
    {
        jumpCount++;
        JumpStatChangedEvent?.Invoke( jumpCount );
    }

    public void ResetLevelStats()
    {
        coinCount = 0;
        jumpCount = 0;

        CoinStatChangedEvent?.Invoke( coinCount );
        JumpStatChangedEvent?.Invoke( jumpCount );
    }

    private void LoadBestScore()
    {
        if ( PlayerPrefs.HasKey( "bestScore" ) )
        {
            bestScore = PlayerPrefs.GetInt( "bestScore" );
        }
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetInt( "bestScore", bestScore );
        PlayerPrefs.Save();
    }

    public void OnGameOver()
    {
        if ( coinCount > bestScore )
        {
            bestScore = coinCount;
            SaveBestScore();
        }
    }

    void OnApplicationQuit()
    {
        SaveBestScore();
    }
}

