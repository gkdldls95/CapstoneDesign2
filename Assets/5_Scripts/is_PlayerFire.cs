using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class is_PlayerFire : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(RemainGr);
        }

        else
        {
            this.RemainGr = (int) stream.ReceiveNext();
        }

    }//  IPunObservable 함수????
    public GameObject firePosition;
    public float throwPower = 10f;
    public Camera PlayerCam;
    public PhotonView PV;
    public GameObject RemainGr_Text;
    public int RemainGr = 2; //남은 수류탄
    Text Gr_rest;
    public GameObject Grenade;

    void Start()
    {
      Gr_rest = RemainGr_Text.GetComponent<Text>();
    }

        void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (is_GManager.gm.gState != is_GManager.GameState.Run) //**10주차 추가 부분
        {
            return;
        }

        if (Input.GetMouseButtonDown(1) && RemainGr>0)
        {
            RemainGr--;
            //GameObject bomb = Instantiate(Grenade); 
            GameObject bomb = PhotonNetwork.Instantiate("Grenade", new Vector3(0, 0, 0), Quaternion.identity);
            bomb.transform.position = firePosition.transform.position;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(PlayerCam.transform.forward * throwPower, ForceMode.Impulse);
        }
        string R_Ammo = RemainGr.ToString();
        Gr_rest.text = "Boom : " + R_Ammo;
    }
}
