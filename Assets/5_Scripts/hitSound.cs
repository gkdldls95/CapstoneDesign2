using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitSound : MonoBehaviour
{
    public AudioClip soundExplosion;
    AudioSource myAudio;
    public static hitSound instance;
    void Awake()
    {
        if (hitSound.instance == null)
        {
            hitSound.instance = this;
        }
    }
    void Start()
    {
        myAudio = this.gameObject.GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        myAudio.PlayOneShot(soundExplosion);
    }
    void Update()
    {

    }
}
