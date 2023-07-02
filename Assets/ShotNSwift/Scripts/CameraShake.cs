// Oleg Kotov

using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.25f;
    public float shakeTime = 0.1f;

    private float elapsedTime = 0.0f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private bool isShaking = false;
    private bool isReverse = false;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    public IEnumerator Shake( Vector2 direction )
    {
        Vector3 targetPosition = initialPosition + ( new Vector3( direction.x, direction.y, 0 ) * shakeAmount );

        float elapsedTime = 0.0f;
        
        while ( elapsedTime < shakeTime )
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01( elapsedTime / shakeTime );

            Vector3 newPosition = Vector3.LerpUnclamped( initialPosition, targetPosition, t );

            transform.localPosition = newPosition;

            yield return null;
        }

        elapsedTime = 0.0f;
        
        while ( elapsedTime < shakeTime )
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01( elapsedTime / shakeTime );

            Vector3 newPosition = Vector3.LerpUnclamped( targetPosition, initialPosition, t );

            transform.localPosition = newPosition;

            yield return null;
        }
    }

    public void StartShake( Vector3 direction )
    {
        targetPosition = initialPosition + direction * shakeAmount;
        elapsedTime = 0.0f;
        isShaking = true;
        isReverse = false;
    }

    void Update()
    {
        if ( !isShaking ) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01( elapsedTime / shakeTime );

        if ( !isReverse )
        {
            Vector3 newPosition = Vector3.LerpUnclamped( initialPosition, targetPosition, t );
            transform.localPosition = newPosition;
        }
        else
        {
            Vector3 newPosition = Vector3.LerpUnclamped( targetPosition, initialPosition, t );
            transform.localPosition = newPosition;
        }

        if ( elapsedTime >= shakeTime && !isReverse )
        {
            isReverse = true;
        }
        else if ( elapsedTime >= shakeTime && isReverse )
        {
            isReverse = false;
            isShaking = false;
        }
    }
}

