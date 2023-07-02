// Oleg Kotov

using TMPro;
using UnityEngine;

public class BaseHudController : MonoBehaviour
{
    public GameObject coinCounter;
    public GameObject jumpCounter;
    public GameObject framerateCounter;

    private TextMeshProUGUI coinCounterText;
    private TextAnimation coinCounterAnimation;

    private TextMeshProUGUI jumpCounterText;
    private TextAnimation jumpCounterAnimation;

    private TextMeshProUGUI framerateText;

    void Start()
    {
        coinCounterText = coinCounter.GetComponent<TextMeshProUGUI>();
        coinCounterAnimation = coinCounter.GetComponent<TextAnimation>();

        jumpCounterText = jumpCounter.GetComponent<TextMeshProUGUI>();
        jumpCounterAnimation = jumpCounter.GetComponent<TextAnimation>();

        framerateText = framerateCounter.GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        GameStats.Instance.CoinStatChangedEvent += OnCoinStatChanged;
        GameStats.Instance.JumpStatChangedEvent += OnJumpStatChanged;
    }

    void OnDisable()
    {
        GameStats.Instance.CoinStatChangedEvent -= OnCoinStatChanged;
        GameStats.Instance.JumpStatChangedEvent -= OnJumpStatChanged;
    }

    private void OnCoinStatChanged( int statValue )
    {
        coinCounterText.text = GameStats.Instance.CoinCount.ToString();
        coinCounterAnimation.StartAnimation();
    }

    private void OnJumpStatChanged( int statValue )
    {
        jumpCounterText.text = GameStats.Instance.JumpCount.ToString();
        jumpCounterAnimation.StartAnimation();
    }

    void Update()
    {
        framerateText.text = Mathf.RoundToInt( 1.0f / Time.deltaTime ).ToString() + " fps";
    }
}

