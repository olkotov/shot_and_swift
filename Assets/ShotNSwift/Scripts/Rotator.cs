// Oleg Kotov

using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 10.0f;

    void Update()
    {
        transform.Rotate( Vector3.forward, rotationSpeed * Time.deltaTime );
    }
}

