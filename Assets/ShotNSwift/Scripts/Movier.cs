// Oleg Kotov

using UnityEngine;

public class Movier : MonoBehaviour
{
    public float speed = 10.0f;

    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += speed * Time.deltaTime;
        transform.position = newPosition;
    }
}

