using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttivazioneAscenzore : MonoBehaviour
{
    [SerializeField]
    private Animator aim;
  public PlayerKartCtrl kart;
    public GameObject menu,pLayer;
   


    // quando siamo in gioco il Mouse devve essere Bloccato più non visibile al giocatore e nel canvas GameObject menu sara gia dissativato
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        menu.gameObject.SetActive(false);
    }

    //quando i player entra in contatto con il collider renderemo Visibile il GameObject menu e il mouse e nel mentre lo libereremo dal lock perchè
    //ci serve per accetare o no per fare la gara o no  
    private void OnTriggerEnter(Collider other)
    {
        //pLayer.transform.parent = transform;
        pLayer.transform.SetParent(transform,true);
        kart.STOOOOp();      
        kart.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menu.gameObject.SetActive(true);
       
    }
    // qui  faremo opposto ovvero bloccheremo il mouse e lo rendiamo invisibile 
    private void OnTriggerExit(Collider other)
    {
        pLayer.transform.parent = null;
        kart.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menu.gameObject.SetActive(false);
    }

    public void Nograzie()
    {
        kart.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menu.gameObject.SetActive(false);
        
    }
    public void VaiAnimazione()
    {
    
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        menu.gameObject.SetActive(false);
        aim.SetTrigger("TriggerAscenzore");

        aim.SetTrigger("TriggerAscenzore1");

        aim.SetTrigger("TriggerAscenzore2");

    }
    //  aim.SetTrigger("TriggerAscenzore");


}
