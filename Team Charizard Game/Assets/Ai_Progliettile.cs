using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ai_Progliettile : MonoBehaviour, IUsableItem
{

    //qui assegniamo il nemico bersaglio del nostro progliettile 
    public GameObject bersaglio;
    //riferimento al parent iniziale dell'oggetto
    private Transform previousParent;
    //qui assegniamo la distanza di vista del progliettile 
    public float distance;

    public int visione;

    public bool vedo;

    public NavMeshAgent agente;


    private void Awake()
    {
        //ottiene il riferimento al parent iniziale dell'oggetto
        previousParent = transform.parent;

    }

    void Update()
    {
        distance = Vector3.Distance(bersaglio.transform.position, this.transform.position);
        if (distance <= visione)
        {
            vedo = true;
        }
        if (distance > visione)
        {
            vedo = false;
        }

        if (vedo)
        {
            agente.isStopped = false;
            agente.SetDestination(bersaglio.transform.position);
        }
        if (!vedo)
        {
            agente.isStopped = true;
        }
    }

    public void UseThisItem()
    {
        //fa in modo che l'oggetto non sia figlio di nessuno
        transform.parent = null;

        //OTTIENE IL RIFERIMENTO AL BERSAGLIO

    }

}
