using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class is_PlayerRotate: MonoBehaviour
{

    public float rotSpeed = 200f;

    float mx = 0;



    // Update is called once per frame
    void Update()
    {
        if (is_GManager.gm.gState != is_GManager.GameState.Run) //**10주차 추가 부분
        {
            return;
        }

        float mouse_X = Input.GetAxis("Mouse X");

        mx += mouse_X * rotSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
