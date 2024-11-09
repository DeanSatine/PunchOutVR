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
    /// Maps value to a new number within range <paramref name="newMin"/> to <paramref name="newMax"/>.Threshholds for min and max outputs can be set with <paramref name="minOriginal"/> and <paramref name="maxOriginal"/>.
    /// </summary>
    /// <param name="value">Value to be normalized.</param>
    /// <param name="newMin">Desired minimum.</param>
    /// <param name="newMax">Desired maximum.</param>
    /// <param name="minOriginal">Lowest input value.</param>
    /// <param name="maxOriginal">Highest input value.</param>
    public static float NormalizeToRange (this float value, float newMin, float newMax, float minOriginal = 0, float maxOriginal = 5)
    {
        return (value - minOriginal) / (maxOriginal - minOriginal) * (newMax- newMin) + newMin; // 
    }
}


public static class ColorExtensions
{
    public static Color Subtract(this Color color, float rSub, float gSub, float bSub, float aSub)
    {
        // Subtract values and clamp each channel to ensure it stays within the valid range (0 to 1)
        float r = Mathf.Clamp01(color.r - rSub);
        float g = Mathf.Clamp01(color.g - gSub);
        float b = Mathf.Clamp01(color.b - bSub);
        float a = Mathf.Clamp01(color.a - aSub);

        return new Color(r, g, b, a);
    }
}
