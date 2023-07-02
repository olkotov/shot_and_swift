// Oleg Kotov

using System.Collections.Generic;
using UnityEngine;

// i used this to adjust the game balance

public class ReactionTimeStatistics
{
    private float startTime;
    private List<float> times = new List<float>();

    public void StartMeasure()
    {
        startTime = Time.time;
    }

    public void StopMeasure()
    {
        float reactionTime = Time.time - startTime;
        times.Add( reactionTime );
    }

    public float GetMinTime()
    {
        float minTime = float.MaxValue;

        foreach ( float time in times )
        {
            if ( time < minTime )
            {
                minTime = time;
            }
        }

        return minTime;
    }

    public float GetMaxTime()
    {
        float maxTime = float.MinValue;

        foreach ( float time in times )
        {
            if ( time > maxTime )
            {
                maxTime = time;
            }
        }

        return maxTime;
    }

    public float GetAvgTime()
    {
        float sumTime = 0;

        foreach ( float time in times )
        {
            sumTime += time;
        }

        return sumTime / times.Count;
    }
}

