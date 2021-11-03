using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCheck : MonoBehaviour
{
    public static bool controllerPlugged = default;                 //creo una variabile booleana statica che mi permette di controllare negli altri script se il controller è collegato o no

    private int Xbox_One_Controller = 0;                            //setto la variabile del controller dell'xbox one a 0
    private int PS4_Controller = 0;                                 //setto la variabile del controller della playstation 4 a 0
    void Update()
    {
        string[] names = Input.GetJoystickNames();                  //creo un array di string che prende il nome del controller collegato.
        for (int x = 0; x < names.Length; x++)                      //faccio un for che crea una variabile x, e controlla che la variabile x sia minore della lunghezza del nome del controller collegato.
        {
            //print(names[x].Length);
            if (names[x].Length == 19)                                  //se la string che contiene il nome del controller è UGUALE a 19
            {
                //print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;                                     //il controller della PS4 è collegato
                Xbox_One_Controller = 0;                                //il controller della Xbox One è scollegato
            }
            if (names[x].Length == 33)                                  //se la string che contiene il nome del controller è UGUALE a 33
            {
                //print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;                                     //il controller della PS4 è collegato
                Xbox_One_Controller = 1;                                //il controller della Xbox One è scollegato
            }
            if (names[x].Length == 0)                                   //se la string che contiene il nome del controller è UGUALE a 0
            {
                //print("CONTROLLER IS NOT CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;                                     //il controller della PS4 è collegato
                Xbox_One_Controller = 0;                                //il controller della Xbox One è scollegato
            }
        }


        if (Xbox_One_Controller == 1)                                   //se il controller della Xbox One è collegato
        {
            controllerPlugged = true;                                   //setto la variabile controllerPlugged a true
            //do something
        }
        else if (PS4_Controller == 1)                                   //se il controller della PS4 è collegato
        {
            controllerPlugged = true;                                   //setto la variabile controllerPlugged a true
            //do something
        }
        else                                                            //sennò
        {
            controllerPlugged = false;                                  //setto la variabile controllerPlugged a false
            // there is no controllers
        }
    }
}
