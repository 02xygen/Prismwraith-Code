using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orient : MonoBehaviour
{
    public Transform holder;
    public float lerpStrength;
    public float rotLerpStrength;

    // Lerp the color gun's position to the gun holder position, this causes the gun to lag slightly behind player and camera movement
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, holder.position, lerpStrength * Time.deltaTime * (holder.position - transform.position).magnitude);
        transform.rotation = Camera.main.transform.rotation;
    }
}
