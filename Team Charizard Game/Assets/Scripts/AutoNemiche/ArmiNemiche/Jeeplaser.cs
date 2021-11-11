using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeeplaser : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform point1;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float maxRotation;

    LineRenderer lr;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //recupera il lineRenderer automaticamente
        lr = GetComponent<LineRenderer>();
        
        StartCoroutine(moveLaser());
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


        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        Vector3 dir = (target.position - transform.position).normalized;
        if(Physics.Raycast ( transform.position,dir,out hit, 150f))
        {
            
            Debug.Log("colpitoPdsdadsdaddasdasr");
            lr.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("colpitoPLayer");
                hit.collider.GetComponent<PlayerHealth>().ChangeHealth(-0.2f);


            }



        }

    }

    Ray hit2;
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(hit2);


    }
    Coroutine cr;
    IEnumerator moveLaser()
    {
        

        cr = StartCoroutine(rotatelaser(rotationSpeed));
        yield return new WaitForSeconds(maxRotation);
        StopCoroutine(cr);
        StartCoroutine(rotatelaser(-rotationSpeed));



        yield return new WaitForSeconds(maxRotation);
        StopCoroutine(cr);
        StartCoroutine(moveLaser());

    }

    IEnumerator rotatelaser(float rotation)
    {
        transform.Rotate(transform.eulerAngles, rotation * Time.deltaTime);
        yield return new WaitForEndOfFrame();
        cr = StartCoroutine(rotatelaser(rotation));
        yield break;

    }
}
