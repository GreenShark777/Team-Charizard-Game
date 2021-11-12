using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    
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
            
               // fa partire la coroutine
                StartCoroutine(turnLeft());
                
            





            
            //se si sta driftando verso sinistra, e non destra...           

            //...ruota il kart nell'asse Y fino ad arrivare al valore massimo impostato per il drift sinistro
            newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, -maxDriftSteer, 0), driftSteerSpeed * Time.deltaTime);


            



        }
        else if (driftRight == true)
        {
            
                // fa partire la coroutine
                StartCoroutine(turnRight());
                
           





            
            //se si sta driftando verso sinistra, e non destra...           

            //...ruota il kart nell'asse Y fino ad arrivare al valore massimo impostato per il drift sinistro
            newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, maxDriftSteer, 0), driftSteerSpeed * Time.deltaTime);


            


        }
        else
        { 
            //calcola la nuova rotazione
            newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, 0f, 0), maxDriftSteer * Time.deltaTime); 
        
        
        
        }
        //imposta la rotazione del kart alla nuova rotazione calcolata
        kart.transform.localRotation = newKartRotation;



    }

    private void OnTriggerEnter(Collider other)
    {
        //crea una variabile falsa
        bool b = false;

        



        if (other.CompareTag("turnLeft") && doOnce == true)
        {
            //imposta b a true per calcolare il tempo per cui deve driftare
            b = true;
            //fa driftare a destra o a sinisttra
            driftLeft = true;
            //impediscce di driftare a destra o a sinistra
            driftRight = false;

        }

        if (other.CompareTag("turnRight") && doOnce == true)
        {
            //imposta b a true per calcolare il tempo per cui deve driftare
            b = true;
            //Debug.Log("DESTRA");
            //fa driftare a destra o a sinisttra
            driftLeft = false;
            //impediscce di driftare a destra o a sinistra
            driftRight = true;


        }

        if (b)
        {
            //prende il nome del gameObject con cui collide
            string name = other.name; 
            //crea una variabile locale e calcola la lunghezza di "name"
            float f = name.ToCharArray().Length;
            //imposta time = ad F
            time = f;
            //Debug.Log(time);

        }

    }


    IEnumerator turnLeft()
    {
        //Debug.Log("SGOMMO A SINISTRA");
        //imposta doOnce a false in modo che il trigger si attivi una volta sola
        doOnce = false;
        // aspetta "time"
        yield return new WaitForSeconds(time);
        //imposta driftRight a false per interrompere la rotazione
        driftLeft = false;
        //imposta doOnce a true in modo da poter ripetere il ciclo
        doOnce = true;
        //Debug.Log("FINE SINISTRA");
        
    }


    IEnumerator turnRight()
    {
        //imposta doOnce a false in modo che il trigger si attivi una volta sola
        doOnce = false;
        // aspetta "time"
        yield return new WaitForSeconds(time);
        //imposta driftRight a false per interrompere la rotazione
        driftRight = false;
        //imposta doOnce a true in modo da poter ripetere il ciclo
        doOnce = true;
        //Debug.Log("FINE DESTRA");


    }

    


   
}
