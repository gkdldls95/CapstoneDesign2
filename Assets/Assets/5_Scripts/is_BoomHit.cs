using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class is_BoomHit : MonoBehaviourPunCallbacks
{
    [PunRPC]
    public void HitByBoom(int damage)
    {
        transform.parent.GetComponent<is_PlayerController > ().photonView.RPC("DamageAction", RpcTarget.All,damage);
        //transform.parent.GetComponent<is_PlayerController > ().hp--;  
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
