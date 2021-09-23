using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class is_BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    public int attackPower = 10;

    public float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        bombSound.instance.PlaySound();
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);
       
        for (int i=0; i<cols.Length; i++)
        {
            cols[i].GetComponent<EFSM>().HitEnemy(attackPower);
        }

        GameObject eff = Instantiate(bombEffect);

        eff.transform.position = transform.position;



        Destroy(gameObject);


    }
}
