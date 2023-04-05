using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchManager : MonoBehaviour
{
    public float[] pitches;

    /*
       Blue pitches[3] = 0.8f;
       Green pitches[7] = 0.9f;
       Yellow pitches[4] = 1.0f;
       Orange pitches[6] = 1.1f;
       Red pitches[2] = 1.2f;
       Purple pitches[5] = 1.3f;
       White pitches[1] = 1.0f;
       Null pitches[0] = 1.0f;
       Brown pitches[9] = 0.5f;
   */

    public float ColorPitch(int color)
    {
        return pitches[color];
    }
}
