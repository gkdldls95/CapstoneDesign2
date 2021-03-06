using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class is_PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hp);
        }

        else
        {
            this.hp = (int)stream.ReceiveNext();
        }
    }//  IPunObservable 함수????

    public PhotonView PV;
    Animator Ani;
    //이동 속도 변수
    public float moveSpeed = 2.5f; 
    public float RunSpeed = 6f;
    //캐릭터 컨트롤로 변수(이동시에 중력을 적용하기 위해)
    CharacterController cc;

    float gravity = -10f;
    float yVelocity = 0;

    float mx = 0;
    float mxx = 0;
    public float rotSpeed = 1500f;
    public float jumpPower = 3f;

    //점프 상태 변수(중복 점프를 방지 하기 위해)
    public bool isJumping = false;

    public int maxHp = 20; //최대hp **8주차 추가 부분
    public int hp;

    public Slider hpSlider; //Slider오브젝트(HP Bar)를 담기 위한 변수 **8주차 추가 부분
    public GameObject HPText;
    Text HP;
    public GameObject hitEffect; //피격을 받았을때 나타났다가 사라지게끔 **10주차 추가 부분

    public Camera PlayerCam;

    


    void Start()
    {
        hp = maxHp;
        Ani = GetComponentInChildren<Animator>();
        //캐릭터 콘트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
        HP = HPText.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<Canvas>().gameObject);
            return;
        }

        if (is_GManager.gm.gState != is_GManager.GameState.Run)
        {
            return;
        }



        is_PlayerRotate();
        is_PlayerMove();

        if (hp > maxHp)
        { hp = maxHp; }
        HP.text = hp + " / " + maxHp;
        hpSlider.value = (float)hp / (float)maxHp; // Slider오브젝트의 value값은 현재체력을 최대체력으로 나눈 값으로 반영된다.
                                                   //체력을 상대적으로 생각하기 때문이다. **8주차 추가 부분

    }

    // 플레이어의 피격 함수 **7주차 추가 부분
    [PunRPC]
    public void DamageAction(int damage)
    {
        if(is_GManager.gm.gState == is_GManager.GameState.Run)
        { hp -= damage; }

        if (hp > 0)
        {
            StartCoroutine(PlayHitEffect()); //hp가 0보다 크면 피경 코루틴 적용 **10주차 추가 부분
        }
        if(hp<=0)
        {
            StartCoroutine(Dead());
            
           // if (PhotonNetwork.IsConnected)
           // { PhotonNetwork.Disconnect(); }
        }

    }

        //피격 effect 코루틴 함수 **10주차 추가 부분
    private IEnumerator PlayHitEffect()
    {
            hitEffect.SetActive(true); //피격 당했을떄 effect를 주고 

            yield return new WaitForSeconds(0.3f); //0.3초 기다렸다가

            hitEffect.SetActive(false); //effect 해제해주고
    }
    private IEnumerator Dead()
    {
        hp = 0;
        HP.text = hp + " / " + maxHp;
        hpSlider.value = (float)hp / (float)maxHp;
        Ani.SetBool("Dead", true);
        if (PV.IsMine)
        { 
            is_GManager.gm.gState = is_GManager.GameState.Dead;
            //print(PV.Owner.NickName + " is dead"); 
        }
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(gameObject);
        //PhotonNetwork.Disconnect();
        //PhotonNetwork.LoadLevel("Lobby");
    }

    private void is_PlayerRotate()
    {

        float mouse_X = Input.GetAxis("Mouse X");

        mx += mouse_X * rotSpeed * Time.deltaTime;
        mxx = mx + is_CamManager.XRebound;
        transform.eulerAngles = new Vector3(0, mxx, 0);

        //transform.foward = PlayerCam.transform.forward; 
        //PlayerCam.transform.forward
    }

    private void is_PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2.이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1. 메인 카메라를 기준으로 방향을 변환한다.(절대좌표를 상대좌표로 바꾸기 위해)
        dir = PlayerCam.transform.TransformDirection(dir);

        // 2-2. 만일, 점프 중이었고, 다시 바닥에 착지했다면...
        if (cc.collisionFlags == CollisionFlags.Below)
        { // 점프 전 상태로 초기화한다.
            isJumping = false;
            //Ani.SetBool("is_Jumping", false);
            if (yVelocity < 0)
            { yVelocity = 0; }
            // 캐릭터 수직 속도를 0으로 만든다.
        }

        if (cc.collisionFlags == CollisionFlags.Above)
        {
            yVelocity = -0.2f;
        }

        //2-3. 만일, 키보드 spacebar 키를 입력했고, 점프를 하지 않은 상태라면..
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            //캐릭터 수직 속도에 점프력을 적용하고 점프 상태로 변경한다.
            yVelocity = jumpPower;
            isJumping = true;
            //Ani.SetBool("is_Jumping", true);
        }


        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        { cc.Move(dir * RunSpeed * Time.deltaTime); }
        else
        { cc.Move(dir * moveSpeed * Time.deltaTime); }

        if (Input.GetKey(KeyCode.W))
        {
            //Ani.SetBool("move", true);
            Ani.SetInteger("Move", 1);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Ani.SetInteger("Move", 2);
            }
        }
        else if (Input.GetKey(KeyCode.A) )
        { Ani.SetInteger("Move", 3); }
        else if (Input.GetKey(KeyCode.S) )
        { Ani.SetInteger("Move", 4); }
        else if (Input.GetKey(KeyCode.D) )
        { Ani.SetInteger("Move", 5); }
        else
        { Ani.SetInteger("Move", 0); }
        //transform.position += dir * moveSpeed * Time.deltaTime;
    }

}
