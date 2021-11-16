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
    [SerializeField]
    private KeyCode comando;
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(comando))
        {
            removeImage();

        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isImageVisible == false)
        {
            //isImageVisible = true;
            showImage();
        }


    }

    //IEnumerator showImage()
    //{
       
    //        image.SetActive(true);
    //        Time.timeScale = 0.2f;
    
    //        yield return new WaitForSeconds(time);
    //        image.SetActive(false);
    //        Time.timeScale = 1;


        

    //}

    void showImage()
    {
        isImageVisible = true;
        image.SetActive(true);
        Time.timeScale = 0;
    }

    void removeImage()
    {
        isImageVisible = false;
        image.SetActive(false);
        Time.timeScale = 1;

    }

}
