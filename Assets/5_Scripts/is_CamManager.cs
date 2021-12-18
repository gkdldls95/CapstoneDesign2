using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class is_CamManager : MonoBehaviour
{
    //회전 속도 변수
    public float rotSpeed = 1500f;
    float mx = 0;
    float my = 0;
    float mxx = 0;
    float myy = 0;
    public static float XRebound = 0;
    public static float YRebound = 0;
    public static float Rebound_Power = 1f;



    private void view()
    {
        
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");
        //회전 값 변수에 마우스 입력 값만큼 미리 누적시킨다.
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;
        mxx = mx + XRebound;
        myy = my + YRebound;

        //마우스 상하 이동 회전 변수(my)의 값을 -90도~90도 사이로 제한한다.
        myy = -Mathf.Clamp(myy, -50f, 70f);


        transform.eulerAngles = new Vector3(myy, mxx, 0);
        
    }

    public static void Rebound()
    {
        XRebound += Random.Range(-Rebound_Power, Rebound_Power);
        YRebound += Random.Range(0, Rebound_Power * 1.2f);
    }



    void Update()
    {
        if (is_GManager.gm.gState != is_GManager.GameState.Run)
        {
            return;
        }
        /*if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            GameObject.Find("Canvas").SetActive(false);
           return; 
       }*/

        view();
        //}
        //transform.position = player.GetComponent<CamPosition>().transform.position;
        //사용자의 마우스 입력을 받아 물체를 회전시키고 싶다.
        //마우스 입력을 받는다.

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}