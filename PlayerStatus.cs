using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatus : MonoBehaviour
{
    public float tankPressure;
    // in Bar
    public float tankVolume;
    // in ml
    public float suitPressure;
    // in Bar
    public float suitVolume;
    // in ml
    public float waterPressure;
    // in Bar
    public float playerDepth;
    // in Meters
    public float health;
    public float heartRate;
    public float breathingRate;
    public float deadAir;
    public float intakeRate;
    public float outputRate;
    public float suitArmor;

    public Transform surface;

    private void Awake()
    {

    }

    void FixedUpdate()
    {
        getDepth();
        calcWaterPressure();
    }

    void getDepth()
    {
        playerDepth = (surface.transform.position.y - transform.position.y);
    }

    void calcWaterPressure()
    {
        waterPressure = (playerDepth / 10f) + 1f;
        waterPressure = Mathf.Clamp(waterPressure, 1f , 1000000000f);
    }

    void calcTankVolume()
    { 

    }

    void calcTankSuitPressure()
    {

    }
}
