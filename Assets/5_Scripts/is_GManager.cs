using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class is_GManager : MonoBehaviour
{
    public static is_GManager gm;
    //public GameObject[] prefabs;
    public GameObject[] areas;
    //private List<GameObject> Players = new List<GameObject>();
    //private List<GameObject> newObject = new List<GameObject>();
    private float Item_Timer=0;
    private bool HavePlayer=false;

    public enum GameState
    {
        Ready,
        Run,
        //GameOver,
        //Cleared,
        Dead
    }
    /*
        Ready -> Run -> 팀 데스 매치      ( 10킬까지)               -> End
                     -> 서바이벌 매치     ( 1명 남을떄까지)
    */
    public GameState gState;

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;
    public GameObject PlayerNumLabel;
    // 게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;
    Text PNL;

    public void Spawn_Player()
    {
        Vector3 spawnPos = GetRandomPosition();
        if (PhotonNetwork.IsConnected)
        {
            GameObject instance = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

            HavePlayer = true;
        }
    }

    /*
    public void addPlayers(GameObject instance)
    {
        instance.transform.parent = GameObject.Find("Players").GetComponent<Transform>();// 묶어주기
        Players.Add(instance);
        Debug.Log("현재 Player 수 : ");
        Debug.Log(Players.Count);
    }
    */

public void Spawn_Items()
    {
        int SelectItemNum = Random.Range(0, 3);
        Vector3 spawnPos = GetRandomPosition();
        switch (SelectItemNum)
        {
            case 0:
                PhotonNetwork.Instantiate("FirstAidKit_Green", spawnPos, Quaternion.identity);
                break;

            case 1:
                PhotonNetwork.Instantiate("FirstAidKit_White", spawnPos, Quaternion.identity);
                break;

            case 2:
                PhotonNetwork.Instantiate("AmmoBox", spawnPos, Quaternion.identity);
                break;
        }
    }

    private Vector3 GetRandomPosition()
    {
        int randArea = Random.Range(0, areas.Length);
        GameObject selectedarea = areas[randArea];

        MeshRenderer renderer = areas[randArea].GetComponent<MeshRenderer>();
        Vector3 size = renderer.bounds.size;

        Vector3 basePosition = selectedarea.transform.position;


        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        //float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, 3, posZ);

        return spawnPos;
    }

    /*
    private GameObject selectPrefab()
    {
        int randPrepab = Random.Range(0, prefabs.Length);
        GameObject selectedPrefab = prefabs[randPrepab];

        return selectedPrefab;
    }

    private void Spawn()
    {
        Vector3 spawnPos = GetRandomPosition();
        GameObject selectedPrefab = selectPrefab();

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        instance.transform.parent = GameObject.Find("items").GetComponent<Transform>();// 묶어주기
        newObject.Add(instance);
        Debug.Log(newObject.Count);
    }
    */


    IEnumerator ReadyToStart()
    {
        // 게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져온다.
        gameText = gameLabel.GetComponent<Text>();
        // "준비 텍스트 출력"
        gameText.text = "준비!";
        // 텍스트 색 조절
        gameText.color = new Color32(153, 255, 0, 153); //글자 색 변경 가능
                                                        // 게임 상태 준비에서 Run으로 바꿈
                                                        // 2초간 준비상태 유지
        yield return new WaitForSeconds(2f);

        // 이후 시작 텍스트 출력
        gameText.text = "시작!";

        yield return new WaitForSeconds(0.5f);

        // 상태 텍스트를 비활성화
        gameLabel.SetActive(false);
        //HPBar.SetActive(true);
        // 게임 시작
        gState = GameState.Run;
        if (HavePlayer == false)
        {
            Spawn_Player();
        }
    }

    /* PVE
    IEnumerator GameOver()
    {
        gState = GameState.GameOver;
        gameLabel.SetActive(true);

        gameText.text = "게임 over";
        gameText.color = new Color32(255, 0, 0, 255);
        //게임종료 텍스트를 출력
        //        animator = GameObject.Find("Player").GetComponent<Animator>();
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
    IEnumerator clear()
    {
        gState = GameState.Cleared;
        gameLabel.SetActive(true);

        gameText.text = "clear!";
        gameText.color = new Color32(255, 0, 0, 255);
        //게임종료 텍스트를 출력

        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
    */



    private void Awake()
    {
            if (gm == null)
            {
                gm = this;
            }
    }

    void Start()
    {
        gState = GameState.Ready;

        PNL = PlayerNumLabel.GetComponent<Text>();
    }

    void Update()
    {

        if (PhotonNetwork.IsConnected)
        {
            PNL.text = "Player 수 : " + PhotonNetwork.CurrentRoom.PlayerCount;
            Item_Timer += Time.deltaTime;
            if (Item_Timer > 30)
            {
                Item_Timer = 0;
                Spawn_Items();
            }

           
            if (gm.gState == GameState.Ready)
            { 
                StartCoroutine(ReadyToStart());
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PhotonNetwork.Disconnect();

            }

            if (gState == GameState.Dead)
            {
                gameLabel.SetActive(true);
                gameText.text = "Press \"R\"key to respone or \"esc\" key to quit";
                HavePlayer = false;
                if (gState == GameState.Dead && Input.GetKeyDown(KeyCode.R))
                { gState = GameState.Ready; }

            }

            // 준비상태

            //if (false)
            // {
            //    PhotonNetwork.LoadLevel("MainScene");
            //  }
        }
        else
        {
            PhotonNetwork.LoadLevel("Lobby");

        }

        //만약 hp가 0보다 작다면

        /*
        if (player.hp <= 0)
        {
             // StartCoroutine(GameOver());
             PhotonNetwork.Destroy(player.gameObject);
        }
        if (player.hp > 0 && EnemyNum <= 0)
        {
            StartCoroutine(clear());
        }
        string restNum = EnemyNum.ToString();
        EN_rest.text = "남은 적의 수 : " + restNum;
        */
    }


}