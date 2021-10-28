using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puntatore_Controller : MonoBehaviour
{

    public Texture2D cursore;

    public Texture2D curoreClicca;

    private CursoreControll controllo;
    private void Awake()
    {
        controllo = new CursoreControll();
        CambiaCursore(cursore);
        Cursor.lockState = CursorLockMode.Confined;

    }

    private void OnEnable()
    {
        controllo.Enable();
    }

    private void OnDisable()
    {
        controllo.Disable();
    }
    

    private void Start()
    {
        controllo.Mouse.Click.started += _ => InizioClick();
        controllo.Mouse.Click.performed += _ => FineClick();

    }

    private  void InizioClick()
    {
        CambiaCursore(curoreClicca);
    }

    private void FineClick()
    {
        CambiaCursore(cursore);
    }
    private void CambiaCursore(Texture2D TipoCursore)
    {
        Vector2 hotSpot = new Vector2(TipoCursore.width / 3, TipoCursore.height / 3);
        //oppure Vector2.zero
        Cursor.SetCursor(TipoCursore, hotSpot, CursorMode.Auto );

    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
