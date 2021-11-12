//Si occupa del comportamento delle bombe rotolanti
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
    //indica quanto velocemente rotola la bomba
    [SerializeField]
    private float rollSpeed = default;
    //indica l'indice da figlio della bomba
    private int bombSiblingIndex = -1;


    private void Awake()
    {
        //ottiene il riferimento al Rigidbody di questa bomba rotolante
        rb = GetComponent<Rigidbody>();
        //ottiene il riferimento all'Animator della bomba
        bombAnim = GetComponent<Animator>();
        //ottiene il riferimento al padre iniziale della bomba rotolante
        previousParent = transform.parent;
        //ottiene l'indice da figlio della bomba
        bombSiblingIndex = transform.GetSiblingIndex();

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

    /// <summary>
    /// Attiva la bomba, venendo lanciata di fronte al giocatore
    /// </summary>
    public void UseThisItem()
    {
        //all'attivazione, la bomba non è più figlia del giocatore fino a quando non esplode
        transform.parent = null;
        //la bomba riceve una spinta verso la direzione in cui è direzionata
        rb.velocity = transform.forward * rollSpeed;
        //infine, viene salvata la velocità della bomba, in modo che non rallenti mai
        rollVelocity = rb.velocity;
        //Debug.Log("Usato Fagiolo Bomba. Velocity = " + rollVelocity);
    }
    /// <summary>
    /// La bomba esplode
    /// </summary>
    private void Explode()
    {
        //viene rimossa ogni forza che agisce sulla bomba
        rb.velocity = Vector3.zero;
        //fa partire l'animazione d'esplosione della bomba
        bombAnim.SetTrigger("Explode");
        //disattiva questo script
        enabled = false;

    }
    /// <summary>
    /// Riporta la bomba al suo stato originale(viene richiamato dall'Animator della bomba)
    /// </summary>
    public void ResetBomb()
    {
        //la bomba torna ad essere figlia del suo parent originale all'indice in cui era prima
        transform.parent = previousParent;
        transform.SetSiblingIndex(bombSiblingIndex);
        //la bomba viene disattivata
        gameObject.SetActive(false);

    }

}
