// Oleg Kotov

using System.Collections;
using TMPro;
using UnityEngine;

// it would be nice to split it into several classes

public class ExperienceHudController : MonoBehaviour
{
    public TextMeshPro currentLevelText;
    public TextMeshPro nextLevelText;

    public Shapes.Rectangle progressRect;
    public Shapes.Rectangle backgroundRect;
    private float rectFullWidth;

    public Shapes.Disc rightDisc;
    public Color fullColor;

    private int experienceLevel = 1;
    private int experiencePoints = 0;
    private int expPointsToLevelUp = 200;
    private int expPointsPerAction = 10;

    float initialPosition = -100.0f;

    private Camera mainCamera;

    void Awake()
    {
        rectFullWidth = backgroundRect.Width;
    }

    void Start()
    {
        mainCamera = Camera.main;

        SetPosition( new Vector2( 0.0f, initialPosition ) );
        PlayShowAnimation();

        Load();

        UpdateLevelText();
        UpdateProgressBar();
    }

    void SetPosition( Vector2 pos )
    {
        Vector3 screenPosition = new Vector3( pos.x, pos.y, 0.0f );
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint( screenPosition );

        worldPosition.x = 0.0f;
        worldPosition.y = -worldPosition.y;
        worldPosition.z = 0.0f;

        transform.localPosition = worldPosition;
    }

    public void OnPlayerTargetReached()
    {
        GainExperience();
        UpdateProgressBar();
        Save();
    }

    private void GainExperience()
    {
        experiencePoints += expPointsPerAction;
        if ( experiencePoints > expPointsToLevelUp ) LevelUp();
    }

    private void LevelUp()
    {
        AudioManager.instance.PlayLevelUpSound();

        experienceLevel++;
        experiencePoints = 0;
        expPointsToLevelUp += (int)( expPointsToLevelUp * 0.3f ); // +30%
        expPointsToLevelUp -= expPointsToLevelUp % 10;

        UpdateLevelText();
    }

    private void Save()
    {
        PlayerPrefs.SetInt( "experienceLevel", experienceLevel );
        PlayerPrefs.SetInt( "experiencePoints", experiencePoints );
        PlayerPrefs.SetInt( "expPointsToLevelUp", expPointsToLevelUp );
        PlayerPrefs.Save();
    }

    private void Load()
    {
        if ( PlayerPrefs.HasKey( "experienceLevel" ) )
        {
            experienceLevel = PlayerPrefs.GetInt( "experienceLevel" );
        }

        if ( PlayerPrefs.HasKey( "experiencePoints" ) )
        {
            experiencePoints = PlayerPrefs.GetInt( "experiencePoints" );
        }

        if ( PlayerPrefs.HasKey( "expPointsToLevelUp" ) )
        {
            expPointsToLevelUp = PlayerPrefs.GetInt( "expPointsToLevelUp" );
        }
    }

    private void UpdateLevelText()
    {
        currentLevelText.text = experienceLevel.ToString();
        nextLevelText.text = (experienceLevel + 1).ToString();
    }

    private void UpdateProgressBar()
    {
        float ratio = experiencePoints / (float)expPointsToLevelUp;
        float newWidth = ratio * rectFullWidth;
        progressRect.Width = newWidth;

        rightDisc.Color = ( experiencePoints == expPointsToLevelUp ) ? fullColor : Color.white;
    }

    public void PlayShowAnimation()
    {
        StartCoroutine( ShowAnimation() );
    }

    float easeOutElastic( float t )
    {
        const float c4 = ( 2.0f * Mathf.PI ) / 3.0f;

        if ( t == 0.0f ) return 0.0f;
        if ( t == 1.0f ) return 1.0f;
    
        return Mathf.Pow( 2.0f, -10.0f * t ) * Mathf.Sin( ( t * 10.0f - 0.75f ) * c4 ) * 0.5f + 1.0f;
    }

    private IEnumerator ShowAnimation()
    {
        float targetPosition = 200.0f;
        float animationTime = 0.75f;

        // ---

        float elapsedTime = 0.0f;
        
        while ( elapsedTime < animationTime )
        {
            // ---

            elapsedTime += Time.deltaTime;
            if ( elapsedTime > animationTime ) elapsedTime = animationTime;

            float t = Mathf.Clamp01( elapsedTime / animationTime );
            t = easeOutElastic( t );

            // ---

            float position = Mathf.LerpUnclamped( initialPosition, targetPosition, t );
            SetPosition( new Vector2( 0.0f, position ) );

            yield return null;
        }
    }
}

