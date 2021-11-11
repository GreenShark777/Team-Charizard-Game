using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    [SerializeField]
    private float maxSteerAngle = 40;
    [SerializeField]
    private WheelCollider wheelFL;
    [SerializeField]
    private WheelCollider wheelFR;
    [SerializeField]
    private WheelCollider wheelRL;
    [SerializeField]
    private WheelCollider wheelRR;                             
    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;
    void Start()
    {
        
        Transform[] pathTransform = path.GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();
        //cicla i waypoint
        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != path.transform)
            {

                nodes.Add(pathTransform[i]);

            }


        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        applySteer();
        
        checkWaypoints();
       
       
    }

    


    private void applySteer()
    {
        //calcola la direzione verso il nuovo waypoint con questa formula
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        
        //cambia l angolazione delle ruote con quest altra formula
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        
        //imposta lo steerangel(rotazione) al float calcolato precedentemente
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;

        
    }


    //private void Drive()
    //{
    //    currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
       
    //    if (currentSpeed < maxSpeed && !isBraking)
    //    {
    //        wheelFL.motorTorque = maxMotorTorque;
    //        wheelFR.motorTorque = maxMotorTorque;
    //    }
    //    else
    //    {
    //        wheelFL.motorTorque = 0;
    //        wheelFR.motorTorque = 0;


    //    }


    //}


    private void checkWaypoints()
    {
        if(Vector3.Distance(transform.position,nodes[currentNode].position) < 0.5f)
        {
            if(currentNode == nodes.Count - 1)
            {

               currentNode = 0;

            }
            else
            currentNode++;

        }


    }


    //public void Braking()
    //{
    //    if (isBraking)
    //    {
    //        fumo.Play();
    //        freno.SetActive(true);
    //        freno2.SetActive(true);
    //        wheelRL.brakeTorque = maxBrakeTorque;
    //        wheelRR.brakeTorque = maxBrakeTorque;
    //    }
    //    else
    //    {
    //        fumo.Stop();
    //        freno.SetActive(false);
    //        freno2.SetActive(false);
    //        wheelRL.brakeTorque = 0;
    //        wheelRR.brakeTorque = 0;

    //    }

    //}



}
