using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging : MonoBehaviour
{


    private void Start()
    {

        var allCheckpoints = FindObjectsOfType<Checkpoints>();

        for (int i = 0; i < allCheckpoints.Length; i++)
        {

            for (int j = i; j < allCheckpoints.Length; j++)
            {

                if (allCheckpoints[i] != allCheckpoints[j] && allCheckpoints[i].GetThisCheckpointID() == allCheckpoints[j].GetThisCheckpointID())
                {
                    Debug.LogError("I checkpoint " + allCheckpoints[i] + " e " + allCheckpoints[j] + " hanno lo stesso ID: " + allCheckpoints[i].GetThisCheckpointID());
                    Debug.LogError("Per caso uno dei 2 è una scorciatoia?");
                }

            }

        }
        
    }



}
