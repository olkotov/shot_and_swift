// Oleg Kotov

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHudController : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;

    void Start()
    {
        bestScoreText.text = GameStats.Instance.BestScore.ToString();
    }

    void Update()
    {
        if ( Gameplay.IsScreenTouched() )
        {
            SceneManager.LoadScene( "gameplay" );
        }
    }
}

