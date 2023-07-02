// Oleg Kotov

using System.Collections;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    private float targetScale = 1.15f;
    private float animationTime = 0.1f;

    private float initialScale;

    public void StartAnimation()
    {
        initialScale = transform.localScale.y;
        StartCoroutine( Animate() );
    }

    private IEnumerator Animate()
    {
        float elapsedTime = 0.0f;
        
        while ( elapsedTime < animationTime )
        {
            elapsedTime += Time.deltaTime;
            if ( elapsedTime > animationTime ) elapsedTime = animationTime;

            float t = elapsedTime / animationTime;

            // ---

            float scale = Mathf.LerpUnclamped( initialScale, targetScale, t );

            Vector3 newScale = transform.localScale;
            newScale.y = scale;

            transform.localScale = newScale;

            yield return null;
        }

        elapsedTime = 0.0f;

        while ( elapsedTime < animationTime )
        {
            elapsedTime += Time.deltaTime;
            if ( elapsedTime > animationTime ) elapsedTime = animationTime;

            float t = elapsedTime / animationTime;

            // ---

            float scale = Mathf.LerpUnclamped( targetScale, initialScale, t );

            Vector3 newScale = transform.localScale;
            newScale.y = scale;

            transform.localScale = newScale;

            yield return null;
        }
    }
}

