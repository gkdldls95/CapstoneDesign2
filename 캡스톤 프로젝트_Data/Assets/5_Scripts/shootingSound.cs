using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingSound : MonoBehaviour
{
    public AudioClip soundExplosion; 
    AudioSource myAudio; 
    public static shootingSound instance;  
    void Awake() 
    {
        if (shootingSound.instance == null) 
        {
            shootingSound.instance = this; 
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
