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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Launching");
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(booster.up * force, ForceMode.Impulse);
            sound.Play();
        }
    }
}
