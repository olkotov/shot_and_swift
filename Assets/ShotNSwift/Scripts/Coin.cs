// Oleg Kotov

using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D( Collider2D  other )
    {
        if ( other.gameObject.tag == "Player" )
        {
            GameStats.Instance.AddCoin();
            AudioManager.instance.PlayCoinPickupSound();
            Destroy( gameObject );
        }
    }
}

