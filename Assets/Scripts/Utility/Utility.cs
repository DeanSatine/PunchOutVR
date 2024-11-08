using UnityEngine;

public class Utility : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class FloatExtensions
{
    /// <summary>
    /// Takes a number and normalizes/maps it to within a given range.
    /// </summary>
    /// <param name="value">Value to be normalized.</param>
    /// <param name="newMin">Desired minimum.</param>
    /// <param name="newMax">Desired maximum.</param>
    /// <param name="minOriginal">Lowest input value.</param>
    /// <param name="maxOriginal">Highest input value.</param>
    public static float NormalizeToRange (this float value, float newMin, float newMax, float minOriginal = 0, float maxOriginal = 5)
    {
        return (value - minOriginal) / (maxOriginal - minOriginal) * (newMax- newMin) + newMin;
    }
}
