using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CapsuleScript : MonoBehaviourPunCallbacks, IPunObservable //변수 동기화 진행
{
    public Rigidbody RB;
    //public SpriterRenderer SR;
    public PhotonView PV;
    public Text NickNameText;
    //public Image HealthImage;

    void Awake()
    {
         NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;

        if (PV.IsMine) //만약에 나면
        {
            NickNameText.color = Color.red; // 레드로 표시하고
        }
        else
        {
            NickNameText.text = " "; // 적의 이름은 표시 하지 않는다.
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
