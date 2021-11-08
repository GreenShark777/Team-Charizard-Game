using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gestione_Menu_pausa : MonoBehaviour
{
    [SerializeField]
    private GameObject pausa;
    private void Start()
    {
        pausa.SetActive(false);
    }
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale !=0)
        {
            AttivazioneMenuPausa(true);
        }   
    }
    public void AttivazioneMenuPausa(bool atti)
    {
        if (atti)
        {
            pausa.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
    

}
