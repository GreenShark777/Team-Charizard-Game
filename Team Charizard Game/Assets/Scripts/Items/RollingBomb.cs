//Si occupa del comportamento delle bombe rotolanti
using System.Collections;
using UnityEngine;

public class RollingBomb : MonoBehaviour, IUsableItem
{
    //riferimento all'Animator della bomba
    private Animator bombAnim;
    //riferimento al Rigidbody di questa bomba rotolante
    private Rigidbody rb;
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
        //ottiene il riferimento all'Animator della bomba
        bombAnim = GetComponent<Animator>();
        //ottiene il riferimento al padre iniziale della bomba rotolante
        previousParent = transform.parent;

    }

    private void FixedUpdate()
    {
        //fa in modo che la bomba non rallenti mai
        if (rb.velocity.x < rollVelocity.x || rb.velocity.z < rollVelocity.z) { rb.velocity = new Vector3(rollVelocity.x, rb.velocity.y, rollVelocity.z); }
        //Debug.Log("Velocity: " + rb.velocity + " : " + rollVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        //se colpisce un nemico o un ostacolo, la bomba esplode
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacles")) { Explode(); }

    }

    private void Explode()
    {
        //viene rimossa ogni forza che agisce sulla bomba
        rb.velocity = Vector3.zero;
        //fa partire l'animazione d'esplosione della bomba
        bombAnim.SetTrigger("Explode");
        //disattiva questo script
        enabled = false;

    }

    public void UseThisItem()
    {
        //all'attivazione, la bomba non è più figlia del giocatore fino a quando non esplode
        transform.parent = null;
        //la bomba riceve una spinta verso la direzione in cui è direzionata
        rb.velocity = transform.forward * rollSpeed;
        //infine, viene salvata la velocità della bomba, in modo che non rallenti mai
        rollVelocity = rb.velocity;
        Debug.Log("Usato Fagiolo Bomba. Velocity = " + rollVelocity);
    }

    public void ResetBomb()
    {
        //la bomba torna ad essere figlia del suo parent originale
        transform.parent = previousParent;
        //la bomba viene disattivata
        gameObject.SetActive(false);

    }

}
