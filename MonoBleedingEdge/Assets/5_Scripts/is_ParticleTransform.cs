using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class is_ParticleTransform : MonoBehaviour
{
    private Transform tr;
    public PhotonView PV;
    Vector3 Pos;
    Quaternion Rot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }

        //클론이 통신을 받는 
        else
        {
           Pos= (Vector3)stream.ReceiveNext();
           Rot = (Quaternion)stream.ReceiveNext();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Pos = tr.position;
        Rot = tr.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            tr.position = Pos;
            tr.rotation = Rot;
        }
}
}
