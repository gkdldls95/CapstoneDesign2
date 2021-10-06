using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombSound : MonoBehaviour
{
    public AudioClip soundExplosion; 
    AudioSource myAudio; 
    public static bombSound instance;  
    void Awake() 
    {
        if (bombSound.instance == null) 
        {
            bombSound.instance = this; 
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