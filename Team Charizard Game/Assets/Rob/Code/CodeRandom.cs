using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeRandom : MonoBehaviour
{
    public GameObject[] items;
    private int rand;
    private int temp=0;
    public GameObject proc;

    void RandomOggetto()
    {
        rand = Random.Range(0, items.Length);
        GameObject oggetto = Instantiate(items[rand], gameObject.transform.position, Quaternion.identity);
        Debug.Log("Oggetto Scelto: " + oggetto.name);
        // o  Debug.Log("Oggetto Scelto: " + items[rand].name);
        proc.SetActive(false);
      

    }
    private void OnTriggerEnter(Collider other)
    {
        RandomOggetto();

    }
}

