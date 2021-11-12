using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboElettrico : MonoBehaviour, IUsableItem
{
    //riferimento all'Animator del cubo elettrico
    private Animator cubeAnim;

    //riferimento al Rigidbody di questo cubo elettrico
    private Rigidbody rb;
    //riferimento al padre iniziale del cubo elettrico
    private Transform previousParent;
    //indica la velocity che il cubo elettrico deve continuare ad avere
    private Vector3 flyVelocity;

    [SerializeField]
    private float flySpeed = default, //indica quanto velocemente deve andare il cubo elettrico
        deactivateTimer = 10, //indica dopo quanto tempo il cubo viene disattivato, se non ha colpito nessuno
        enemyBlockTimer = 2; //indica per quanto tempo un nemico viene fermato dopo essere colpito

    //riferimento alla coroutine di disattivazione
    private Coroutine deactivateRoutine;


    private void Awake()
    {
        //ottiene il riferimento al Rigidbody di questo cubo elettrico
        rb = GetComponent<Rigidbody>();
        //ottiene il riferimento all'Animator del cubo elettrico
        cubeAnim = GetComponent<Animator>();
        //ottiene il riferimento al padre iniziale della bomba rotolante
        previousParent = transform.parent;

    }

    private void FixedUpdate()
    {
        //fa in modo che il cubo elettrico non rallenti mai
        if (rb.velocity.x < flyVelocity.x || rb.velocity.z < flyVelocity.z) { rb.velocity = new Vector3(flyVelocity.x, rb.velocity.y, flyVelocity.z); }
        //Debug.Log("Velocity: " + rb.velocity + " : " + rollVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        //se colpisce un nemico, il cubo lo ferma per un po'
        if (other.CompareTag("Enemy")) { StartCoroutine(BlockEnemy(other.transform)); }

    }

    public void UseThisItem()
    {
        //all'attivazione, il cubo non è più figlio del giocatore fino a quando non viene disattivato
        transform.parent = null;
        //il cubo riceve una spinta verso la direzione in cui è direzionata
        rb.velocity = transform.forward * flySpeed;
        //viene salvata la velocità del cubo, in modo che non rallenti mai
        flyVelocity = rb.velocity;
        //infine, fa partire la coroutine per disattivare il cubo nel caso non colpisca un nemico
        deactivateRoutine = StartCoroutine(DeactivateCube());
        Debug.Log("Usato Cubo Elettrico. Velocity = " + flyVelocity);
    }

    private IEnumerator BlockEnemy(Transform enemy)
    {
        //ferma la coroutine di disattivazione
        StopCoroutine(deactivateRoutine);

        //IL NEMICO VIENE BLOCCATO

        //aspetta un po'
        yield return new WaitForSeconds(enemyBlockTimer);

        //IL NEMICO TORNA AL SUO NORMALE COMPORTAMENTO

        //il cubo viene disattivato
        gameObject.SetActive(false);

    }

    private IEnumerator DeactivateCube()
    {
        //aspetta il tempo di disattivazione
        yield return new WaitForSeconds(deactivateTimer);
        //riporta il cubo al suo stato originale
        ResetCube();

    }

    private void ResetCube()
    {
        //viene rimossa ogni forza che agisce sul cubo elettrico
        rb.velocity = Vector3.zero;
        //il cubo torna ad essere figlio del suo parent originale
        transform.parent = previousParent;

    }

}
