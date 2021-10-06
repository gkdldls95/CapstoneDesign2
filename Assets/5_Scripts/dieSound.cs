using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieSound : MonoBehaviour
{
    public AudioClip soundExplosion;
    AudioSource myAudio;
    public static dieSound instance;
    void Awake()
    {
        if (dieSound.instance == null)
        {
            dieSound.instance = this;
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
