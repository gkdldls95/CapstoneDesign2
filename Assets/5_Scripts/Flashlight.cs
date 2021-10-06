using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    public GameObject Light;
    public bool LightActive;

    void Start()
    {
        Light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LightActive = !LightActive;

            if (LightActive)
            {
                FlashlightActive();
            }

            if (!LightActive)
            {
                FlashlightInactive();
            }
        }
    }

    void FlashlightActive()
    {
        Light.SetActive(true);
    }

    void FlashlightInactive()
    {
        Light.SetActive(false);
    }
}
