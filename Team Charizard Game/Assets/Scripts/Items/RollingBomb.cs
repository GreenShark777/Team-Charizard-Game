//Si occupa del comportamento delle bombe rotolanti
using System.Collections;
using UnityEngine;

public class RollingBomb : MonoBehaviour, IUsableItem
{
    //riferimento al Rigidbody di questa bomba rotolante
    private Rigidbody rb;

    //riferimento al giocatore, da cui otterrà la direzione di lancio
    //private Transform player;

    //riferimento al padre iniziale della bomba rotolante
    private Transform previousParent;
    //indica la velocity che la bomba rotolante deve continuare ad avere
    private Vector3 rollVelocity;
    
    [SerializeField]
    private float rollSpeed = default, //indica quanto velocemente rotola la bomba
        explosionTimer = 1; //indica dopo quanto tempo la bomba torna al suo stato originale dopo l'esplosione


    private void Awake()
    {
        //ottiene il riferimento al Rigidbody di questa bomba rotolante
        rb = GetComponent<Rigidbody>();
        //ottiene il riferimento al padre iniziale della bomba rotolante
        previousParent = transform.parent;

    }

    private void FixedUpdate()
    {

        if (rb.velocity.x < rollVelocity.x || rb.velocity.z < rollVelocity.z) { rb.velocity = new Vector3(rollVelocity.x, rb.velocity.x, rollVelocity.x); }

    }

    private IEnumerator Explode()
    {
        //viene rimossa ogni forza che agisce sulla bomba
        rb.velocity = Vector3.zero;

        //il modello della bomba scompare

        //parte il particellare di esplosione

        yield return new WaitForSeconds(1);
        //la bomba torna ad essere figlia del suo parent originale
        transform.parent = previousParent;
        //la bomba viene disattivata
        gameObject.SetActive(false);

    }

    public void UseThisItem()
    {
        //all'attivazione, la bomba non è più figlia del giocatore fino a quando non esplode
        transform.parent = null;
        //infine, la bomba riceve una spinta verso la direzione in cui è direzionata
        rb.AddForce(transform.forward * rollSpeed, ForceMode.VelocityChange);

        rollVelocity = rb.velocity;

        Debug.Log("Usato Fagiolo Bomba");
    }

}
