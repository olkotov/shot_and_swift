// Oleg Kotov

using UnityEngine;

public static class Gameplay
{
    public static bool IsScreenTouched()
    {
        return ( Input.touchCount > 0 ) && ( Input.GetTouch( 0 ).phase == TouchPhase.Began );
    }
}

