using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //**8주차 추가 부분

public class EFSM : MonoBehaviour
{
    Animator ani;
    enum EnemyState //열겨형
    {
        Idle,   //대기 상태
        Move,   //움직이는 상태
        //다시 Move는 2가지로 분류 1.Player가 일정범위 안으로 들어와 공격하기 위해 이동하는 경우 2.Player가 일정범위 밖으로 벗어나 다시 원위치로 복귀하는 경우 
        Attack, //공격하고 있는 상태
        Return, //복귀하는 상태
        Damaged,//공격을 당하고 있는 상태
        Die     //죽음
    }

    EnemyState m_State; //에너미 상태 변수

    public float findDistance = 5f; //적이 Player을 인지하는 거리 설정

    GameObject GManager;
    Transform player;

    public float attackDistance = 2f; //적이 Player를 공격하는 범위

    public float moveSpeed = 5f; //적이 이동하는 속도

    CharacterController cc;

    //누적시간 변수
    float currentTime = 0;
    //공격 딜레이 시간
    float attackDelay = 2f;
    // 에너미 공격력
    public int attackPower = 3; //이 숫자 만큼 Player의 체력 hp가 감소 ->PlayerMove DamageAction함수 damage매개변수로 들어감  

    Vector3 originPos;// 초기 위치 저장용 변수
    Quaternion originRot;
    // 이동 가능 범위
    public float moveDistance = 12f;
    // 에너미의 체력
    public int hp = 15;
    int maxHp = 15; //**8주차 추가 부분
    float yVelocity;

    //public Slider hpSlider; // slider오브젝트를 담기 위한 변수 선언 **8주차 추가 부분

    void Start()
    {
        m_State = EnemyState.Idle; //최초의 적의 상태는 대기상태
        player = GameObject.Find("Player").transform;
        GManager = GameObject.Find("is_GManager");
        cc = GetComponent<CharacterController>(); //적의 캐릭터 컨트롤러를 받아옴

        originPos = transform.position;
        originRot = transform.rotation;
        ani=GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (m_State)
        {

            case EnemyState.Idle:
                Idle();
                ani.SetBool("Move", false);
                break;
            case EnemyState.Move:
                Move();
                ani.SetBool("Move", true);
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;

        }

        //hpSlider.value = (float)hp / (float)maxHp;
    }


    void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance) // 적과 Player사이의 거리가 8f 이내라면 Move로 전환
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
        }

    }

    void Move()
    {
        if (Vector3.Distance(transform.position, player.position) > moveDistance)  // 일정 거리 이상 벗어나면
        {
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance) //공격범위 밖이라면 Player를 공격하기 위해서 이동한다.
        {

            Vector3 dir = (player.position - transform.position).normalized; //어느쪽으로 이동할지 방향을 설정해주고

            cc.Move(dir * moveSpeed * Time.deltaTime); //적의 캐릭터 컨트롤러를 이용해 Player쪽으로 이동한다.

            transform.forward = dir; // player의 방향쪽으로 향한다

        }
        else //Player가 공격범위 안에 존재한다면 공격을 한다
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");
            currentTime = 1.75f; //누적시간을 1; 시간으로 시작하여 처음에 공격하지 않는 문제를 해결 
        }


    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance) //공격범위 안이라면 Player를 공격하기 위해서 이동한다.
        {
            // 일정한 시간마다 플레이어를 공격한다.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                ani.SetTrigger("Attack");
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;
            }

        }
        else //공격범위 밖이라면 재추격한다.
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;
        }
    }

    
    void Return()
    {
        if (Vector3.Distance(transform.position, player.position) > moveDistance)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.forward = dir;
            if (transform.position == originPos)
            {
            m_State = EnemyState.Idle;
                print("상태 전환: Return -> Idle");
            }
        }
        // 그렇지 않다면, 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기 상태로 전환한다.

       /* else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            // hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");
            ani.SetTrigger("MoveToIdle");
        }*/
    }

    

    // 데미지 실행 함수
    public void HitEnemy(int hitPower)
    {
        // 만일, 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면 아무런 처리도 하지 않고 함수를 종료한다.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die )
        {
            return;
        }

        // 플레이어의 공격력만큼 에너미의 체력을 감소시킨다.
        hp -= hitPower;

        // 에너미의 체력이 0보다 크면 피격 상태로 전환한다. ->체력이 남아 있으니까
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");
            Damaged();
            hitSound.instance.PlaySound();
        }
        // 그렇지 않다면, 죽음 상태로 전환한다.
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            Die();
        }
    }

    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(0.1f);

        ani.SetTrigger("Damaged");
        // 현재 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    void Die()
    {
        dieSound.instance.PlaySound();
        // 진행중인 피격 코루틴을 중지한다.
        StopAllCoroutines();
        ani.SetTrigger("Dead");
        // 죽음 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 콘트롤러 컴포넌트를 비활성화한다.
        cc.enabled = false;

        // 0.5초 동안 기다린 뒤에 자기 자신을 제거한다.
        yield return new WaitForSeconds(1f);

        GManager.GetComponent<is_GManager>().EnemyNum--;
        print("소멸!");
        Destroy(gameObject);
    }

}
