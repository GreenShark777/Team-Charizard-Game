using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ai_Progliettile : MonoBehaviour, IUsableItem
{

    //qui assegniamo il nemico bersaglio del nostro progliettile 
    public GameObject bersaglio;
    //qui assegniamo la distanza di vista del progliettile 
    public float distance;

    public int visione;
    public bool vedo;

    public NavMeshAgent agente;

    public void UseThisItem()
    {
        throw new System.NotImplementedException();
    }

    private void OnEnable()
    {
        //(GABRIELE)ogni volta che viene attivato, ottiene il riferimento al bersaglio

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

}
