﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class is_PlayerShooting : MonoBehaviour
{
    Animator ani;
    public GameObject bulletEffect;
    public GameObject bloodEffect;
    public GameObject Amo_Label;
    public Camera PlayerCam;
    Text Ammo_rest;
    ParticleSystem ps;
    ParticleSystem ps2;
    //발사 무기 공격력 **7주차 추가 부분
    public int weaponPower = 5;
    public int RemainAmmo = 100; //남은 탄
    public int MaxAmmo = 25; // 최대탄
    public int magAmmo;  // 현재탄
    public float reloadTime = 1.5f;
    //public float DelayTime = 0.5f;
    public bool isReroading = false;

    public PhotonView PV;

    public enum State
    {
        Ready,
        Empty,
        Reload,
        Delayed
    }

    public State gunState
    {
        get;
        private set;
    }


    private IEnumerator ReLoad() //코루틴이므로 장전동안 총의 다른기능이 동작하지 않도록 잠금
    {
        gunState = State.Reload;
        isReroading = true;
        reloadSound.instance.PlaySound();
        print("장전중!");
        yield return new WaitForSeconds(reloadTime);  // 코루틴 일시정지 + 장전시간
        int ammoToFill = MaxAmmo - magAmmo;

        if (RemainAmmo < ammoToFill)
        { ammoToFill = RemainAmmo; }

        magAmmo += ammoToFill;
        RemainAmmo -= ammoToFill;

        gunState = State.Ready;
        print("장전완료!");
        isReroading = false;
    }

    private IEnumerator AfterFire()
    {
        magAmmo--;
        is_CamManager.Rebound();
        gunState = State.Delayed;
        yield return new WaitForSeconds(0.1f);
        gunState = State.Ready;
        if (magAmmo <= 0)
        {
            gunState = State.Empty;
            print("총알 다씀!");
        }

    }

    public bool checkReload()
    {
        if (gunState == State.Reload || RemainAmmo <= 0 || magAmmo >= MaxAmmo || isReroading)
        {
            print("장전불가!");
            return false;
        }

        else
        {
            StartCoroutine(ReLoad());
            return true;
        }
    }

    private void shoot()
    {
        if (gunState == State.Empty) //눌렀는데 empty거나 장전 누르면
        {
            print("장전시도");
            checkReload();
        }

        if (gunState == State.Ready && !isReroading)
        {

            //레이를 생성한 후 발사될 위치와 진행 방향을 설정한다.
            Ray ray = new Ray(PlayerCam.transform.position, PlayerCam.transform.forward);

            //레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                /*
                // 만일 레이에 부딪힌 대상의 레이어가 "Enemy"라면 데미지 함수를 실행한다. **7주차 추가 부분
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EFSM eFSM = hitInfo.transform.GetComponent<EFSM>();
                    eFSM.HitEnemy(weaponPower); //weaponPower만큼 Enemy의 체력 hp가 감소 ->ESFM HitEnemy함수 hitpower매개변수로 들어감
                    bloodEffect.transform.position = hitInfo.point;
                    bloodEffect.transform.forward = hitInfo.normal;
                    ps2.Play();
                }
                */

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player")) 
                {
                    is_PlayerController player = hitInfo.transform.GetComponent<is_PlayerController>();
                    player.photonView.RPC("DamageAction", RpcTarget.All, 22);
                   // player.DamageAction(22);
                    bloodEffect.transform.position = hitInfo.point;
                    bloodEffect.transform.forward = hitInfo.normal;
                    ps2.Play();

                }

                // 그렇지 않다면, 레이에 부딪힌 지점에 피격 이펙트를 플레이한다. (즉 적이 아닐때)
                else
                {
                    // 피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다.
                    bulletEffect.transform.position = hitInfo.point;

                    // 피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다.
                    bulletEffect.transform.forward = hitInfo.normal;

                    // 피격 이펙트를 플레이한다.
                    ps.Play();

                }
                ani.SetTrigger("Shoot");
                shootingSound.instance.PlaySound();
                StartCoroutine(AfterFire());
            }

        }


    }

    void Start()
    {
        Ammo_rest = Amo_Label.GetComponent<Text>();
        ani = GetComponentInChildren<Animator>();
        gunState = State.Ready;
        magAmmo = MaxAmmo;
        
        //피격 이펙트 오브젝트에서 파티클 시스템 컴포넌트 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();
        ps2 = bloodEffect.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (is_GManager.gm.gState != is_GManager.GameState.Run)
        {
            return;
        }
        //  단발if (Input.GetMouseButtonDown(0))    
        if (Input.GetMouseButton(0))
        {
            shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            checkReload();
        }

        string R_Ammo =RemainAmmo.ToString();
        string Ammo = magAmmo.ToString();
        Ammo_rest.text ="bullet : " + Ammo + " / " + R_Ammo ;

    }


}