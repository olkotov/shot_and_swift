// Oleg Kotov

using UnityEngine;

public class PulseAnimation : MonoBehaviour
{
    public float amplitude = 1.0f;
    public float speed = 1.0f;

    private Vector3 initialScale = Vector3.one;
    private float elapsedTime = 0.0f;

    void Start()
    {
        initialScale = transform.localScale;

        Reset();
    }

    public void Reset()
    {
        elapsedTime = 0.0f;
        transform.localScale = initialScale;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime * speed;

        float scale = Mathf.Sin( elapsedTime ) * amplitude;
        Vector3 newScale = initialScale + new Vector3( scale, scale, scale );

        transform.localScale = newScale;
    }
}

