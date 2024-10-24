using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class MyMath
{
    public static Color Lerp(Color start, Color end, float amount)
    {
        return start + (end - start) * amount;
    }
}