using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Path : MonoBehaviour
{
    public Color lineColor;

    private List<Transform> waypoints = new List<Transform>();



    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform[] waypointsTransform = GetComponentsInChildren<Transform>();

        waypoints = new List<Transform>();

        for(int i = 0; i< waypointsTransform.Length; i++)
        {
            if(waypointsTransform[i] != transform)
            {

                waypoints.Add(waypointsTransform[i]);

            }


        }

        for(int i = 0; i< waypoints.Count; i++)
        {
            Vector3 currentWP = waypoints[i].position;
            Vector3 previousWP = Vector3.zero;
            if (i > 0) 
            {

                previousWP = waypoints[i - 1].position;
            }
            else if( i == 0 && waypoints.Count > 1)
            {
                previousWP = waypoints[waypoints.Count - 1].position;
            }

            Gizmos.DrawLine(previousWP, currentWP);
        }


    }
}
