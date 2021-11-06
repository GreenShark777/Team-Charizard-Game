//Si occupa di cosa deve accadere durante una cinematica di circuito
using System.Collections;
using UnityEngine;

public class CircuitCinematics : MonoBehaviour
{
    //riferimento al manager delle cinematiche
    //[SerializeField]
    private CinematicsManager cm = default;

    private Transform moveCameraTo, //riferimento al punto in cui portare la telecamera durante la cinematica
        cameraToMove; //riferimento alla telecamera

    //riferimento alla rotazione che dovrà avere la telecamera durante la cinematica
    Quaternion rotateTo;
    //indica quando tempo deve durare la cinematica
    [SerializeField]
    private float cinematicDuration = 2, 
        waitBeforeMoving = 0.2f;

    //indica se la telecamera deve essere spostata o meno
    private bool moveCamera = false;
    //indica se la telecamera deve essere ruotata o meno
    [SerializeField]
    private bool rotateCamera = false;

    //indica se questa cinematica è di fine o inizio gara
    //[SerializeField]
    //private bool isEndRaceCinematic = false;


    private void Awake()
    {
        //ottiene il riferimento al Manager delle cinematiche
        cm = GetComponentInParent<CinematicsManager>();

    }

    private void FixedUpdate()
    {
        //se si deve muovere la telecamera durante la cinematica, la fa muovere verso la nuova posizione lentamente
        if (moveCamera) { cameraToMove.position = Vector3.Lerp(cameraToMove.position, moveCameraTo.position, Time.deltaTime); }
        //se la telecamera deve essere ruotata durante la cinematica, la fa ruotare lentamente
        if (rotateCamera) { cameraToMove.rotation = Quaternion.Lerp(cameraToMove.rotation, rotateTo, Time.deltaTime); }

    }
    /// <summary>
    /// Fa partire la cinematica con i dovuti tempismi
    /// </summary>
    /// <param name="cam"></param>
    /// <returns></returns>
    public IEnumerator StartCinematic(Transform cam)
    {
        //attiva questa cinematica
        enabled = true;
        //ottiene il riferimento alla telecamera
        cameraToMove = cam;
        //teletrasporta la telecamera nel nuovo punto di cinematica
        cameraToMove.position = transform.position;
        //ruota la telecamera in base alla rotazione della cinematica
        cameraToMove.rotation = transform.rotation;
        //ottiene il riferimento al punto in cui portare la telecamera durante la cinematica
        moveCameraTo = transform.GetChild(0);
        //se il punto appena ottenuto è attivo...
        if (moveCameraTo.gameObject.activeSelf)
        {
            //...aspetta un po'...
            yield return new WaitForSeconds(waitBeforeMoving);
            //...e fa in modo che la telecamera si muova verso quel punto
            moveCamera = true;

        }
        //se bisogna ruotare la telecamera, ottiene il riferimento al punto che determina come deve ruotare la telecamera
        if (rotateCamera) { rotateTo = (moveCamera) ? moveCameraTo.rotation : transform.rotation; }
        //aspetta che finisca la cinematica
        yield return new WaitForSeconds(cinematicDuration);
        //disattiva questa cinematica
        enabled = false;
        //infine, comunica al manager delle cinematiche di andare avanti con le cinematiche
        cm.ToNextCinematic(/*isEndRaceCinematic*/);

    }

}
