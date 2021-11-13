using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float explosionSpeed;
  

    // Start is called before the first frame update
    void Start()
    {
        // aggiunge una forza al gameOBj per lanciarla
        this.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
