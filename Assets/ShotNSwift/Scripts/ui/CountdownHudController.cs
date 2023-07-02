// Oleg Kotov

using UnityEngine;

public class CountdownHudController : MonoBehaviour
{
    public delegate void CountdownExpiredHandler();
    public event CountdownExpiredHandler CountdownExpired;

    public float limitTime = 2.5f;
    public float delayTime = 2.5f / 4.0f; // 4 step

    private float remainingTime = 0.0f;
    private float nextDamageTimer = 0.0f;

    public Shapes.Rectangle progressRect;
    private float rectFullWidth;

    public Shapes.Rectangle damageRect;

    float animationTime = 0.5f;
    float elapsedTime = 0.0f;

    float initialValue;
    float targetValue;

    void Awake()
    {
        rectFullWidth = progressRect.Width;
        ResetTimer();
    }

    void Update()
    {
        nextDamageTimer -= Time.deltaTime;

        if ( nextDamageTimer <= 0 )
        {
            nextDamageTimer = delayTime;
            DecreaseTimer();

            initialValue = progressRect.Width;
            UpdateProgressRectWidth();
            targetValue = progressRect.Width;

            elapsedTime = 0.0f;
        }

        UpdateDamageAnimation();

        if ( remainingTime <= 0.0f )
        {
            CountdownExpired?.Invoke();
        }
    }

    public void SetPosition( Vector3 position )
    {
        position.x = position.x - rectFullWidth * 0.5f;
        position.y -= 0.75f;
        position.z = 0.0f;

        transform.position = position;
    }

    public void DecreaseTimer()
    {
        remainingTime -= delayTime;
    }

    public void ResetTimer()
    {
        remainingTime = limitTime;
        nextDamageTimer = delayTime;

        UpdateProgressRectWidth();
    }

    private void UpdateProgressRectWidth()
    {
        float ratio = remainingTime / limitTime;
        float newWidth = ratio * rectFullWidth;
        progressRect.Width = newWidth;
    }

    public void UpdateDamageAnimation()
    {
        if ( elapsedTime == animationTime ) return;

        // ---

        elapsedTime += Time.deltaTime;
        if ( elapsedTime > animationTime ) elapsedTime = animationTime;

        float t = elapsedTime / animationTime;

        // ---

        float value = Mathf.LerpUnclamped( initialValue, targetValue, t );
        damageRect.Width = value;
    }
}

