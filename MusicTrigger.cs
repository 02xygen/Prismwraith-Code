using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip music;
    public AudioSource nondiagetic;

    private void OnTriggerEnter(Collider other)
    {
        // If the player hits the trigger play the set music
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            nondiagetic.clip = music;
            nondiagetic.Play();
        }
    }
}
