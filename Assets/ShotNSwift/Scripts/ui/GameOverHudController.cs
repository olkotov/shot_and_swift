// Oleg Kotov

using TMPro;
using UnityEngine;

public class GameOverHudController : MonoBehaviour
{
    public TextMeshProUGUI scoreCounterText;
    public TextMeshProUGUI bestText;

    private GameMode gameMode;

    void Start()
    {
        gameMode = GameObject.Find( "game_mode" ).GetComponent<GameMode>();
        // gameMode.GameOverEvent += UpdateHudData; // don't work ¯\_(ツ)_/¯
    }

    public void UpdateHudData()
    {
        int coinCount = GameStats.Instance.CoinCount;
        int bestScore = GameStats.Instance.BestScore;

        scoreCounterText.text = coinCount.ToString();

        bestText.text = ( coinCount > bestScore ) ? "NEW BEST" : "BEST " + bestScore.ToString();
    }
}

