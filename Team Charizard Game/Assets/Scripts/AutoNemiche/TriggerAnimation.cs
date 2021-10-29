using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField]
    private string tagToCheck;
    [SerializeField]
    private float time;
    [SerializeField]
    private GameObject kart;
    [SerializeField]
    private float maxDriftSteer;
    [SerializeField]
    private float driftSteerSpeed;

    Quaternion newKartRotation;
    private bool driftLeft, driftRight;
    private bool doOnce = true;


  
    


    private void Update()
    {
        Quaternion actualKartRotation = kart.transform.localRotation;
        if (driftLeft == true )
        {
            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(turnLeft());
                
            }





            
            //se si sta driftando verso sinistra, e non destra...           

            //...ruota il kart nell'asse Y fino ad arrivare al valore massimo impostato per il drift sinistro
            newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, -maxDriftSteer, 0), driftSteerSpeed * Time.deltaTime);


            



        }
        else if (driftRight == true)
        {
            if (doOnce)
            {
                doOnce = false;
                StartCoroutine(turnRight());
                
            }





            
            //se si sta driftando verso sinistra, e non destra...           

            //...ruota il kart nell'asse Y fino ad arrivare al valore massimo impostato per il drift sinistro
            newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, maxDriftSteer, 0), driftSteerSpeed * Time.deltaTime);


            


        }
        else
        { 
            
            newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, 0f, 0), maxDriftSteer * Time.deltaTime); 
        
        
        
        }

        kart.transform.localRotation = newKartRotation;



    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("turnLeft"))
        {
            Debug.Log("SINISTRA");
            driftLeft = true;
            driftRight = false;

        }

        if (other.CompareTag("turnRight"))
        {
            Debug.Log("DESTRA");
            driftLeft = false;
            driftRight = true;


        }

        

    }


    IEnumerator turnLeft()
    {
        Debug.Log("SGOMMO A SINISTRA");
        
        yield return new WaitForSeconds(time);
        driftLeft = false;
        doOnce = true;
        Debug.Log("FINE SINISTRA");
        
    }


    IEnumerator turnRight()
    {
        
        yield return new WaitForSeconds(time);
        driftRight = false;
        doOnce = true;
        Debug.Log("FINE DESTRA");


    }


   
}
