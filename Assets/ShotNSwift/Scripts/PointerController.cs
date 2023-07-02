// Oleg Kotov

using UnityEngine;

public class PointerController : MonoBehaviour
{
    public float radius = 0.5f;
    public float rotationTime = 0.2f;

    private float elapsedTime = 0.0f;

    private float initialAngle = 0.0f;
    private float targetAngle = 0.0f;

    public void SetDirection( Vector2 direction )
    {
        elapsedTime = 0.0f;

        // double check
        direction.Normalize();

        Vector2 currentDir = new Vector2( transform.localPosition.x, transform.localPosition.y ).normalized;
      
        initialAngle = Shapes.ShapesMath.DirToAng( currentDir );
        targetAngle = Shapes.ShapesMath.DirToAng( direction );
    }

    void Update()
    {
        if ( elapsedTime >= rotationTime ) return;

        elapsedTime += Time.deltaTime;
        if ( elapsedTime > rotationTime ) elapsedTime = rotationTime;

        float t = elapsedTime / rotationTime;

        float angle = Mathf.LerpUnclamped( initialAngle, targetAngle, t );
        Vector2 position = Shapes.ShapesMath.AngToDir( angle ) * radius;

        transform.localPosition = new Vector3( position.x, position.y, 0.0f );
        transform.localEulerAngles = new Vector3( 0.0f, 0.0f, angle * Mathf.Rad2Deg + 135.0f );
    }
}

