using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public Transform booster;
    public float force;
    public AudioSource sound;
    public LayerMask playerLayer;
    public SphereCollider thrustPoint;


    private void OnTriggerEnter(Collider other)
    {
        // Check if player is touching the jumpad
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Launching");

            // Launch player in set direction with set force amount
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(booster.up * force, ForceMode.Impulse);
            sound.Play(); // Play jumpad sound
        }
    }
}
