using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ai_Progliettile : MonoBehaviour
{

    //qui assegniamo il nemico o il player come bersaglio del nostro progliettile 
    public GameObject bersaglio;
    //qui assegniamo la velocita del progliettile 
    public float distance;

    public int visione;
    public bool vedo;

    public NavMeshAgent agente;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
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
