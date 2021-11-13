using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeeplaser : MonoBehaviour
{
    [SerializeField]
    private Transform target;
   
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float maxRotation;
    [SerializeField]
    private Transform firePoint;

    public LineRenderer lr;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //recupera il lineRenderer automaticamente
        //lr = GetComponent<LineRenderer>();
        
        //StartCoroutine(moveLaser());
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (transform.localRotation.y > maxRotation)
        //{
        //    rotationSpeed = -Mathf.Abs(rotationSpeed);
        //    Debug.Log("rotazionePositivo");
        //}
        //else if(transform.localRotation.y < -maxRotation)
        //{

        //    rotationSpeed = Mathf.Abs(rotationSpeed);
        //    Debug.Log("rotazioneNegativo");

        //}

        //imposta il primo punto del line renderer
        lr.SetPosition(0, firePoint.position);
        //crea una variabile raycast hit
        RaycastHit hit;
        //crea un vettore che indica la direzione verso cui far andare il raycast
        Vector3 dir = (target.position - firePoint.position).normalized;
        //crea il raycast e se colpisce qualcosa
        if(Physics.Raycast ( firePoint.position,dir,out hit, 150f))
        {
            //imposta il secondo punto del line renderer al punto di hit del raycast
            
            lr.SetPosition(1, hit.point);
            //ses colpisce il player
            if (hit.collider.CompareTag("Player"))
            {
                //richiama la funzione per togliere vita al player
                hit.collider.GetComponent<PlayerHealth>().ChangeHealth(-0.2f);


            }



        }

    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(firePoint.position,target.position -firePoint.position);


    }
 
    //IEnumerator moveLaser()
    //{

    //    Debug.Log("inizioCOROUTINE");
    //    cr = StartCoroutine(rotatelaser(rotationSpeed));
    //    yield return new WaitForSeconds(maxRotation);
    //    StopCoroutine(cr);
    //    StartCoroutine(rotatelaser(-rotationSpeed));


    //    Debug.Log("mezzoCOROUTINE");
    //    yield return new WaitForSeconds(maxRotation);
    //    StopCoroutine(cr);
    //    StartCoroutine(moveLaser());
    //    Debug.Log("fineCOROUTINE");

    //}

    //IEnumerator rotatelaser(float rotation)
    //{
    //    Debug.Log("inizioROTATE");
    //    transform.Rotate(transform.eulerAngles, rotation * Time.deltaTime);
    //    yield return new WaitForEndOfFrame();
    //    Debug.Log("fineROTATE");

    //    cr = StartCoroutine(rotatelaser(rotation));
    //    yield break;

    //}
}
