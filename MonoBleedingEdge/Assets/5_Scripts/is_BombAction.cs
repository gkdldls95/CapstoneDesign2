using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class is_BombAction : MonoBehaviourPun
{
   public GameObject bombEffect;
    public int attackPower = 50;
    public int shockPower = 20;

    public float explosionRadius = 5f;
    private float timer = 0;
    //  private void OnCollisionStay(Collision collision)
    private int count =0; // 네트워크 지연시 중첩 버그 방지
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1.5 && count == 0)
        {
            bombSound.instance.PlaySound();
            GameObject eff = Instantiate(bombEffect);
           //GameObject eff = PhotonNetwork.InstantiateSceneObject("BigExplosion", new Vector3(0, 0, 0), Quaternion.identity);
            eff.transform.position = transform.position;
            count++;


            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);    // 12Layer
            for (int i = 0; i < cols.Length; i++)
            {
                //cols[i].GetComponent<EFSM>().HitEnemy(attackPower);
                //cols[i].GetComponent<is_BoomHit>().photonView.RPC("HitByBoom", RpcTarget.All, shockPower); 
                cols[i].GetComponent<is_BoomHit>().HitByBoom(shockPower);
                //cols[i].GetComponent<is_PlayerController>().hp -= attackPower;
            }

            Collider[] cols2 = Physics.OverlapSphere(transform.position, explosionRadius/2, 1 << 8);
            for (int i = 0; i < cols2.Length; i++)
            {
                cols[i].GetComponent<is_BoomHit>().HitByBoom(attackPower);
                //cols[i].GetComponent<is_BoomHit>().photonView.RPC("HitByBoom", RpcTarget.All, attackPower);
            }


            /*for (int i = 0; i < cols2.Length; i++)
            {
                cols2[i].GetComponent<EFSM>().HitEnemy(attackPower);
                cols2[i].GetComponent<is_PlayerController>().hp -= attackPower;
            }*/


            //PhotonNetwork.Destroy(gameObject); 
            Destroy(gameObject);
        }
    }
}
