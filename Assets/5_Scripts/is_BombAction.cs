using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class is_BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    public int attackPower = 1;

    public float explosionRadius = 5f;
    private float timer = 0;
    //  private void OnCollisionStay(Collision collision)




    void Update()
    {
        timer += Time.deltaTime;


        if (timer > 1.5)
        {
            bombSound.instance.PlaySound();
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);
            //Collider[] cols2 = Physics.OverlapSphere(transform.position, 2*explosionRadius, 1 << 10);
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].GetComponent<EFSM>().HitEnemy(attackPower);
                //cols[i].GetComponent<is_PlayerController>().hp -= attackPower;
            }
            /*for (int i = 0; i < cols2.Length; i++)
            {
                cols2[i].GetComponent<EFSM>().HitEnemy(attackPower);
                cols2[i].GetComponent<is_PlayerController>().hp -= attackPower;
            }*/

            GameObject eff = Instantiate(bombEffect);

            eff.transform.position = transform.position;



            Destroy(gameObject);
        }
    }
}
