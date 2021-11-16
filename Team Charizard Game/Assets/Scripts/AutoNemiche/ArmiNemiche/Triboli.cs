using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triboli : MonoBehaviour, IGiveDamage
{
    [SerializeField]
    private float dmg = default;
    private void Start()
    {
        //fa partire la coroutine allo spawn di questo GO
        StartCoroutine(destroy());

    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    //se viene colpito il player
    //    if (other.CompareTag("Player"))
    //    {
    //        //richiama la funzione di danno del player
    //        Debug.Log("colpitoPLayer");
    //        other.GetComponent<PlayerHealth>().ChangeHealth(-5f);

    //    }

    //    if (other.CompareTag("Enemy"))
    //    {

    //        other.GetComponent<enemyCarHealth>().slowDown(3);

    //    }

    //}



    IEnumerator destroy()
    {
        //aspetta tot secondi e poi viene distrutto
        yield return new WaitForSeconds(5);
        Destroy(gameObject);

    }

    public float GiveDamage()
    {
      return dmg;
    }
}
