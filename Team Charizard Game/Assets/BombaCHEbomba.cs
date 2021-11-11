using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaCHEbomba : MonoBehaviour
{
    public GameObject bobmba;
    public Rigidbody rig;
    public float speed;
    public Transform spawn;
    bool attivati;
    private void Start()
    {
        rig = GetComponent<Rigidbody>();


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            attivati = true;
        }
       
    }
    private void FixedUpdate()
    {
        if (attivati)
        {
            lanciaBomb();
        }
    }
    private  void lanciaBomb()
    {
        bobmba.transform.rotation = Quaternion.LookRotation(spawn.forward);
        Rigidbody rig = bobmba.GetComponent<Rigidbody>();
        rig.AddForce(spawn.forward * speed,ForceMode.Impulse);
        attivati = false;
    }
}
