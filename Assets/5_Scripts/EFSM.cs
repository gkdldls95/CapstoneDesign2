using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //**8���� �߰� �κ�

public class EFSM : MonoBehaviour
{
    Animator ani;
    enum EnemyState //������
    {
        Idle,   //��� ����
        Move,   //�����̴� ����
        //�ٽ� Move�� 2������ �з� 1.Player�� �������� ������ ���� �����ϱ� ���� �̵��ϴ� ��� 2.Player�� �������� ������ ��� �ٽ� ����ġ�� �����ϴ� ��� 
        Attack, //�����ϰ� �ִ� ����
        Return, //�����ϴ� ����
        Damaged,//������ ���ϰ� �ִ� ����
        Die     //����
    }

    EnemyState m_State; //���ʹ� ���� ����

    public float findDistance = 5f; //���� Player�� �����ϴ� �Ÿ� ����

    GameObject GManager;
    Transform player;

    public float attackDistance = 2f; //���� Player�� �����ϴ� ����

    public float moveSpeed = 5f; //���� �̵��ϴ� �ӵ�

    CharacterController cc;

    //�����ð� ����
    float currentTime = 0;
    //���� ������ �ð�
    float attackDelay = 2f;
    // ���ʹ� ���ݷ�
    public int attackPower = 3; //�� ���� ��ŭ Player�� ü�� hp�� ���� ->PlayerMove DamageAction�Լ� damage�Ű������� ��  

    Vector3 originPos;// �ʱ� ��ġ ����� ����
    Quaternion originRot;
    // �̵� ���� ����
    public float moveDistance = 12f;
    // ���ʹ��� ü��
    public int hp = 15;
    int maxHp = 15; //**8���� �߰� �κ�
    float yVelocity;

    //public Slider hpSlider; // slider������Ʈ�� ��� ���� ���� ���� **8���� �߰� �κ�

    void Start()
    {
        m_State = EnemyState.Idle; //������ ���� ���´� ������
        player = GameObject.Find("Player").transform;
        GManager = GameObject.Find("is_GManager");
        cc = GetComponent<CharacterController>(); //���� ĳ���� ��Ʈ�ѷ��� �޾ƿ�

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
        if (Vector3.Distance(transform.position, player.position) < findDistance) // ���� Player������ �Ÿ��� 8f �̳���� Move�� ��ȯ
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Idle -> Move");
        }

    }

    void Move()
    {
        if (Vector3.Distance(transform.position, player.position) > moveDistance)  // ���� �Ÿ� �̻� �����
        {
            m_State = EnemyState.Return;
            print("���� ��ȯ: Move -> Return");
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance) //���ݹ��� ���̶�� Player�� �����ϱ� ���ؼ� �̵��Ѵ�.
        {

            Vector3 dir = (player.position - transform.position).normalized; //��������� �̵����� ������ �������ְ�

            cc.Move(dir * moveSpeed * Time.deltaTime); //���� ĳ���� ��Ʈ�ѷ��� �̿��� Player������ �̵��Ѵ�.

            transform.forward = dir; // player�� ���������� ���Ѵ�

        }
        else //Player�� ���ݹ��� �ȿ� �����Ѵٸ� ������ �Ѵ�
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ: Move -> Attack");
            currentTime = 1.75f; //�����ð��� 1; �ð����� �����Ͽ� ó���� �������� �ʴ� ������ �ذ� 
        }


    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance) //���ݹ��� ���̶�� Player�� �����ϱ� ���ؼ� �̵��Ѵ�.
        {
            // ������ �ð����� �÷��̾ �����Ѵ�.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                ani.SetTrigger("Attack");
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;
            }

        }
        else //���ݹ��� ���̶�� ���߰��Ѵ�.
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Attack -> Move");
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
                print("���� ��ȯ: Return -> Idle");
            }
        }
        // �׷��� �ʴٸ�, �ڽ��� ��ġ�� �ʱ� ��ġ�� �����ϰ� ���� ���¸� ��� ���·� ��ȯ�Ѵ�.

       /* else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            // hp = maxHp;
            m_State = EnemyState.Idle;
            print("���� ��ȯ: Return -> Idle");
            ani.SetTrigger("MoveToIdle");
        }*/
    }

    

    // ������ ���� �Լ�
    public void HitEnemy(int hitPower)
    {
        // ����, �̹� �ǰ� �����̰ų� ��� ���� �Ǵ� ���� ���¶�� �ƹ��� ó���� ���� �ʰ� �Լ��� �����Ѵ�.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die )
        {
            return;
        }

        // �÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��.
        hp -= hitPower;

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ�Ѵ�. ->ü���� ���� �����ϱ�
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ: Any state -> Damaged");
            Damaged();
            hitSound.instance.PlaySound();
        }
        // �׷��� �ʴٸ�, ���� ���·� ��ȯ�Ѵ�.
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ: Any state -> Die");
            Die();
        }
    }

    void Damaged()
    {
        // �ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DamageProcess());
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(0.1f);

        ani.SetTrigger("Damaged");
        // ���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
    }

    void Die()
    {
        dieSound.instance.PlaySound();
        // �������� �ǰ� �ڷ�ƾ�� �����Ѵ�.
        StopAllCoroutines();
        ani.SetTrigger("Dead");
        // ���� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
        cc.enabled = false;

        // 0.5�� ���� ��ٸ� �ڿ� �ڱ� �ڽ��� �����Ѵ�.
        yield return new WaitForSeconds(1f);

        GManager.GetComponent<is_GManager>().EnemyNum--;
        print("�Ҹ�!");
        Destroy(gameObject);
    }

}
