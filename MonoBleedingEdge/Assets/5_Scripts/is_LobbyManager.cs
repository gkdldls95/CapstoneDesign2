using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class is_LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "2";   // 1은 솔로 2는 멀티 예정??
    public Text connectionInfoText;
    public Button joinButton;
    public InputField NickNameInput;
    //public Dropdown dropdown;

    void Awake()
    {
        Screen.SetResolution(1280, 700, false);
        //PhotonNetwork.SendRate = 60;
        //PhotonNetwork.SerializationRate = 30;
    }

    // Start is called before the first frame update
    private void Start()
    {
  
       //if (PhotonNetwork.IsConnected)
       //{
       // PhotonNetwork.Disconnect();
        //}
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();  //마스터 서버 들어가기 
        joinButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속 중...";
    }


    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    } // 서버 연결시 룸 접속 버튼 활성화

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음";
        PhotonNetwork.ConnectUsingSettings();
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음(연결 재시도중)";
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = " 룸에 접속 시도";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "오프라인 : 연결되지 않음";
            PhotonNetwork.ConnectUsingSettings();
            connectionInfoText.text = "오프라인 : 연결되지 않음(연결 재시도중)";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = " 빈방이 없으므로, 새로운 방 생성 ";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 6 });

    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "방 참가 성공";

        //SceneManager.LoadScene("MainScene");

        PhotonNetwork.LoadLevel("MainScene");
        /*
        if (dropdown.value == 0)
        { PhotonNetwork.LoadLevel("MainScene"); }

        else if (dropdown.value == 1)
        { PhotonNetwork.LoadLevel("ReadyPlace"); }
        */
    }

}
