using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;

public class is_GetItem : MonoBehaviourPun
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "AmmoBox")
        {
            PhotonNetwork.Destroy(hit.collider.gameObject);
            transform.GetComponent<is_PlayerShooting>().RemainAmmo += 25;
            transform.GetComponent<is_PlayerFire>().RemainGr += 1;
        }

        else if (hit.collider.gameObject.tag == "GreenHealPack")
        {
            PhotonNetwork.Destroy(hit.collider.gameObject);
            transform.GetComponent<is_PlayerController>().hp += 50;
        }

        else if (hit.collider.gameObject.tag == "whiteHealPack")
        {
            PhotonNetwork.Destroy(hit.collider.gameObject);
            transform.GetComponent<is_PlayerController>().hp += 25;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
