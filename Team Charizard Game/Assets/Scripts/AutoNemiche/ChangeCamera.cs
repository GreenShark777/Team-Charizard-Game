using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField]
    public List<Transform> points;

    int i = 0;

    private void Start()
    {
        transform.position = points[i].position;
    }

    private void Update()
    {
        
        

        if (Input.GetKeyDown(KeyCode.L))
        {
            if(i > points.Count - 1)
            {

                i = 0;

            }
            else
            {
                
                //richiama il void
                changeCamera();

            }

           


        }


    }

    void changeCamera()
    {
        i += 1;
        transform.position = points[i].position;
        

    }

}
