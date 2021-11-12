//Si occupa del comportamento del cubo elettrico
using System.Collections;
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

    //indica l'indice da figlio del cubo
    private int cubeSiblingIndex = -1;
    //indica che il cubo elettrico sta bloccando un nemico
    private bool isBlockingAnEnemy = false;
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
        //ottiene l'indice da figlio del cubo
        cubeSiblingIndex = transform.GetSiblingIndex();

    }

    private void FixedUpdate()
    {
        //se il cubo elettrico non sta bloccando alcun nemico...
        if (!isBlockingAnEnemy)
        {
            //...fa in modo che non rallenti mai
            if (rb.velocity.x < flyVelocity.x || rb.velocity.z < flyVelocity.z) { rb.velocity = new Vector3(flyVelocity.x, rb.velocity.y, flyVelocity.z); }

        }
        //Debug.Log("Velocity: " + rb.velocity + " : " + rollVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        //se colpisce un nemico, il cubo lo ferma per un po'(se non sta già fermando un altro)
        if (other.CompareTag("Enemy") && !isBlockingAnEnemy) { StartCoroutine(BlockEnemy(other.transform)); }

    }

    /// <summary>
    /// Attiva il cubo elettrico, lanciandolo di fronte al giocatore
    /// </summary>
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
        //Debug.Log("Usato Cubo Elettrico. Velocity = " + flyVelocity);
    }
    /// <summary>
    /// Ferma il nemico con cui ha colliso per un po'
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private IEnumerator BlockEnemy(Transform enemy)
    {
        //comunica che il cubo sta bloccando un nemico
        isBlockingAnEnemy = true;
        //viene rimossa ogni forza che agisce sul cubo elettrico
        rb.velocity = Vector3.zero;
        //ferma la coroutine di disattivazione
        StopCoroutine(deactivateRoutine);
        //fa partire l'animazione di stop al nemico
        cubeAnim.SetBool("BlockEnemy", true);

        //IL NEMICO VIENE BLOCCATO

        //aspetta un po'
        yield return new WaitForSeconds(enemyBlockTimer);

        //IL NEMICO TORNA AL SUO NORMALE COMPORTAMENTO

        //riporta il cubo al suo stato originale
        ResetCube();

    }
    /// <summary>
    /// Disattiva il cubo se passa troppo tempo da quando è stato lanciato
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeactivateCube()
    {
        //aspetta il tempo di disattivazione
        yield return new WaitForSeconds(deactivateTimer);
        //riporta il cubo al suo stato originale
        ResetCube();

    }
    /// <summary>
    /// Riporta il cubo allo stato originale
    /// </summary>
    private void ResetCube()
    {
        //il cubo torna ad essere figlio del suo parent originale all'indice in cui era prima
        transform.parent = previousParent;
        transform.SetSiblingIndex(cubeSiblingIndex);
        //il cubo viene disattivato
        gameObject.SetActive(false);
        //l'animazione torna a quella di idle
        cubeAnim.SetBool("BlockEnemy", false);
        //comunica che il cubo non sta più bloccando un nemico
        isBlockingAnEnemy = false;

    }

}
