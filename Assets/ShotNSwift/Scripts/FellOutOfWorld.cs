// Oleg Kotov

using UnityEngine;

public class FellOutOfWorld : MonoBehaviour
{
    public float ypos = 12.5f;

    void Update()
    {
        if ( transform.position.y > ypos )
        {
            Destroy( gameObject );
        }
    }
}

