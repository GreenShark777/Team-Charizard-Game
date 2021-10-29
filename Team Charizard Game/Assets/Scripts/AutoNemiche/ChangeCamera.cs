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
            
            i += 1;
            changeCamera();

        }


    }

    void changeCamera()
    {

        transform.position = points[i].position;
        

    }

}
