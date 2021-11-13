using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApriTriboli : MonoBehaviour
{
    [SerializeField]
    private GameObject feedbackPoint;
    [SerializeField]
    private GameObject triboli;
    [SerializeField]
    private float time;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private Transform spawnPoint;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            // fa partire la coroutine
            StartCoroutine(releaseTriboli());

        }


    }


    IEnumerator releaseTriboli()
    {
        //rende visibile il punto di spawn della trappola
        feedbackPoint.SetActive(true);
        //aspetta del tempo
        yield return new WaitForSeconds(time);
        //fa spawnare i triboli
        Instantiate(triboli, spawnPoint.position + offset, Quaternion.identity);
        //rended invisibile il punto di spawn della trappoal
        feedbackPoint.SetActive(false);


    }


    
}
