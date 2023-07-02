// Oleg Kotov

using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter2D( Collider2D  other )
    {
        if ( other.gameObject.tag == "Player" )
        {
            AudioManager.instance.PlayObstacleImpactSound();

            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController?.OnPlayerObstacleCollision();
        }
    }
}

