using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class setdestination : MonoBehaviour
{
    public LayerMask ground;
    public List<Transform> wp;
    NavMeshAgent agent;
    int wpIndex;
    
    public Vector3 dir;


    // Update is called once per frame
    private void Start()
    {
         agent = GetComponent<NavMeshAgent>(); //prende la navmesh 
        

    }


    private void Update()
    {
        groundCheck();
        followWP();

    }
    void followWP()
    {
        if (wpIndex == wp.Count) //se l index è uguale al count di WP
        {
            wpIndex = 0;  //ricomincia da capo
        }

        agent.SetDestination(wp[wpIndex].position); //setta la destinazione al waypoint attuale

        


        if( Vector3.Distance(transform.position,wp[wpIndex].position) < 4) //se la distanza è minore di 4
        {
            
            wpIndex++; //prossimo waypoint

        }


       
        
    }



    void groundCheck()
    {
        
        //crea un RayCast        
        RaycastHit hit;         
        //fa partire il raycast dal centro del kart e lo fa andare verso sotto, se entro la distanza impostata c'è del terreno...        
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.7f,ground/*, groundLayer*/))         {
            //...ruota il kart in base alla pendenza dell'oggetto su cui si è...             
            Debug.Log("ORA MI PIEGO");
         transform.GetChild(0).rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) *                 
          transform.rotation, 100f * Time.deltaTime); } 



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 1.7f, transform.position.z));


    }


  

}
