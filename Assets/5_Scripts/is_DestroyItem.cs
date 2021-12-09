using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class is_DestroyItem : MonoBehaviourPun
{
    public bool pre_ate =true;

    [PunRPC]
    public void DestroyItem()
    {
        PhotonNetwork.Destroy(gameObject);
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
