using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject image;
    [SerializeField]
    private float time;
    [SerializeField]
    private bool isImageVisible = false;
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isImageVisible = true;

            StartCoroutine(showImage());
        }


    }

    IEnumerator showImage()
    {
       
            image.SetActive(true);
            Time.timeScale = 0.2f;
    
            yield return new WaitForSeconds(time);
            image.SetActive(false);
            Time.timeScale = 1;


        

    }

}
