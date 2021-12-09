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
            is_DestroyItem DI = hit.collider.gameObject.GetComponent<is_DestroyItem>();
            DI.photonView.RPC("DestroyItem", RpcTarget.All);

            if (DI.pre_ate)
            {
                transform.GetComponent<is_PlayerShooting>().RemainAmmo += 25;
                transform.GetComponent<is_PlayerFire>().RemainGr += 1;
                DI.pre_ate = false;
            }
        }

        else if (hit.collider.gameObject.tag == "GreenHealPack")
        {
            is_DestroyItem DI = hit.collider.gameObject.GetComponent<is_DestroyItem>();
            DI.photonView.RPC("DestroyItem", RpcTarget.All);
            if (DI.pre_ate)
            {
                transform.GetComponent<is_PlayerController>().hp += 50;
                DI.pre_ate = false;
            }
        }

        else if (hit.collider.gameObject.tag == "whiteHealPack")
        {
            is_DestroyItem DI = hit.collider.gameObject.GetComponent<is_DestroyItem>();
            DI.photonView.RPC("DestroyItem", RpcTarget.All);
            if (DI.pre_ate)
            {
                transform.GetComponent<is_PlayerController>().hp += 25;
                DI.pre_ate = false;
            }
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
