using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchManager : MonoBehaviour
{
    public float[] pitches;

    public float ColorPitch(int color)
    {
        return pitches[color]; // Return the pitch corresponding to the inputed color index
    }
}
