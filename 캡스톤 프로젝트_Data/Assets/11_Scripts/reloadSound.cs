using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reloadSound : MonoBehaviour
{
    public AudioClip soundExplosion;
    AudioSource myAudio;
    public static reloadSound instance;
    void Awake()
    {
        if (reloadSound.instance == null)
        {
            reloadSound.instance = this;
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
