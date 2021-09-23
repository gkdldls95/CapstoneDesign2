using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //**8���� �߰� �κ�

public class PlayerMove : MonoBehaviour
{

    Animator Ani;
    //�̵� �ӵ� ����
    public float moveSpeed = 7f;
    //ĳ���� ��Ʈ�ѷ� ����(�̵��ÿ� �߷��� �����ϱ� ����)
    CharacterController cc;
    //�߷º���
    float gravity = -20f;

    //���� �ӷº���
    public float yVelocity = 0;

    //������ ����
    public float jumpPower = 3f;

    //���� ���� ����(�ߺ� ������ ���� �ϱ� ����)
    public bool isJumping = false;

    // �÷��̾� ü�� ���� �÷��̾��� ü������ ���� **7���� �߰� �κ�
    public int hp = 20;

    int maxHp = 20; //�ִ�hp **8���� �߰� �κ�

    public Slider hpSlider; //Slider������Ʈ(HP Bar)�� ��� ���� ���� **8���� �߰� �κ�

    public GameObject hitEffect; //�ǰ��� �޾����� ��Ÿ���ٰ� ������Բ� **10���� �߰� �κ�

    void Start()
    {
        maxHp = hp;
        Ani = GetComponentInChildren<Animator>();
        //ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(is_GManager.gm.gState != is_GManager.GameState.Run) //**10���� �߰� �κ�
        {
            return;
        }

        */

        //W A S D Ű�� ������ �Է��ϸ� ĳ���͸� �� �������� �̵���Ű�� �ʹ�.
        //�����̽��� Ű�� ������ ĳ���͸� �������� ������Ű�� �ʹ�.

        //1. ������� �Է��� �޴´�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2.�̵� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1. ���� ī�޶� �������� ������ ��ȯ�Ѵ�.(������ǥ�� �����ǥ�� �ٲٱ� ����)
        dir = Camera.main.transform.TransformDirection(dir);

        // 2-2. ����, ���� ���̾���, �ٽ� �ٴڿ� �����ߴٸ�...
        if (cc.collisionFlags == CollisionFlags.Below)
        { // ���� �� ���·� �ʱ�ȭ�Ѵ�.
            if (isJumping)
            {
                isJumping = false;
            }
            // ĳ���� ���� �ӵ��� 0���� �����.
            yVelocity = 0;
        }


        //2-3. ����, Ű���� spacebar Ű�� �Է��߰�, ������ ���� ���� ���¶��..
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            //ĳ���� ���� �ӵ��� �������� �����ϰ� ���� ���·� �����Ѵ�.
            yVelocity = jumpPower;
            isJumping = true;
        }

        //2-4 ĳ���� ���� �ӵ��� �߷� ���� �����Ѵ�.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;


        // 3.�̵� �ӵ��� ���� �̵��Ѵ�.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Ani.SetBool("move", true);
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        { Ani.SetBool("move", false); }
        //transform.position += dir * moveSpeed * Time.deltaTime;

        hpSlider.value = (float)hp / (float)maxHp; // Slider������Ʈ�� value���� ����ü���� �ִ�ü������ ���� ������ �ݿ��ȴ�.
                                                   //ü���� ��������� �����ϱ� �����̴�. **8���� �߰� �κ�

    }

    // �÷��̾��� �ǰ� �Լ� **7���� �߰� �κ�
    public void DamageAction(int damage)
    {
        // ���ʹ��� ���ݷ¸�ŭ �÷��̾��� ü���� ��´�.
        hp -= damage;

        if(hp > 0)
        {
            StartCoroutine(PlayHitEffect()); //hp�� 0���� ũ�� �ǰ� �ڷ�ƾ ���� **10���� �߰� �κ�
        }

        //�ǰ� effect �ڷ�ƾ �Լ� **10���� �߰� �κ�
        IEnumerator PlayHitEffect()
        {
            hitEffect.SetActive(true); //�ǰ� �������� effect�� �ְ� 

            yield return new WaitForSeconds(0.3f); //0.3�� ��ٷȴٰ�

            hitEffect.SetActive(false); //effect �������ְ�
        }
    }
}
