// Oleg Kotov

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerTargetReachedHandler( Vector3 direction );
    public event PlayerTargetReachedHandler PlayerTargetReached;

    public delegate void PlayerObstacleCollisionHandler();
    public event PlayerObstacleCollisionHandler PlayerObstacleCollision;

    public float moveTime = 0.25f;
    private float elapsedTime = 0.0f;

    private bool isMoving = false;
    public bool IsMoving => isMoving;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    public void OnPlayerObstacleCollision()
    {
        PlayerObstacleCollision?.Invoke();
    }

    public void MoveToTarget( Vector3 position )
    {
        isMoving = true;

        initialPosition = transform.position;
        targetPosition = position;
    }

    void Update()
    {
        if ( !isMoving ) return;

        elapsedTime += Time.deltaTime;
        if ( elapsedTime > moveTime ) elapsedTime = moveTime;

        float t = elapsedTime / moveTime;

        transform.position = Vector3.LerpUnclamped( initialPosition, targetPosition, t );

        if ( elapsedTime == moveTime )
        {
            isMoving = false;
            elapsedTime = 0.0f;

            Vector3 moveDirection = ( targetPosition - initialPosition ).normalized;
            PlayerTargetReached?.Invoke( moveDirection );
        }
    }
}

