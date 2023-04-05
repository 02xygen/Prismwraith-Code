using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip music;
    public AudioSource nondiagetic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            nondiagetic.clip = music;
            nondiagetic.Play();
        }
    }
}
