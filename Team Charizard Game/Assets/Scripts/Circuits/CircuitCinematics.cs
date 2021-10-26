//Si occupa di cosa deve accadere durante una cinematica di circuito
using System.Collections;
using UnityEngine;

public class CircuitCinematics : MonoBehaviour
{
    //riferimento al manager delle cinematiche
    [SerializeField]
    private CinematicsManager cm = default;
    
    private Transform moveCameraTo, //riferimento al punto in cui portare la telecamera durante la cinematica
        cameraToMove; //riferimento alla telecamera

    //indica quando tempo deve durare la cinematica
    [SerializeField]
    private float cinematicDuration = 2, 
        waitBeforeMoving = 0.2f;
    //indica se la telecamera deve essere spostata o meno
    private bool moveCamera = false;


    private void FixedUpdate()
    {
        //se si deve muovere la telecamera durante la cinematica...
        if (moveCamera)
        {
            //...la fa muovere verso la nuova posizione lentamente
            cameraToMove.position = Vector3.Lerp(cameraToMove.position, moveCameraTo.position, Time.deltaTime);

        }

    }
    /// <summary>
    /// Fa partire la cinematica con i dovuti tempismi
    /// </summary>
    /// <param name="cam"></param>
    /// <returns></returns>
    public IEnumerator StartCinematic(Transform cam)
    {
        //ottiene il riferimento alla telecamera
        cameraToMove = cam;
        //teletrasporta la telecamera nel nuovo punto di cinematica
        cameraToMove.position = transform.position;
        //ottiene il riferimento al punto in cui portare la telecamera durante la cinematica
        moveCameraTo = transform.GetChild(0);
        //se il riferimento al punto appena ottenuto non è nullo...
        if (moveCameraTo.gameObject.activeSelf)
        {
            //...aspetta un po'...
            yield return new WaitForSeconds(waitBeforeMoving);
            //...e fa in modo che la telecamera si muova verso quel punto
            moveCamera = true;

        }
        //aspetta che finisca la cinematica
        yield return new WaitForSeconds(cinematicDuration);
        //disattiva questa cinematica
        enabled = false;
        //infine, comunica al manager delle cinematiche di andare avanti con le cinematiche
        cm.ToNextCinematic();

    }

}
