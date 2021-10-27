//Si occupa di come la telecamera segue il giocatore e come si ruota in base a quello che fa quest'ultimo
//Ispirato dal video di Ishaan35:https://www.youtube.com/watch?v=q0cUClufuKE&t=16s
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //riferimento al giocatore
    [SerializeField]
    private Transform player = default;
    //riferimento alla telecamera
    private Transform cam;
    //indica quanto velocemente la telecamera cambia posizione e rotazione
    [SerializeField]
    private float camSpeed = 3;
    //indica quanto più lontano la telecamera si deve mettere quando si è in boost...
    [SerializeField]
    private float camBoostOffsetY = 0, //...nell'asse Y...
        camBoostOffsetZ = 0; //...e nell'asse Z

    //riferimento allo script di movimento del kart del giocatore
    private PlayerKartCtrl playerScript;
    //indica la posizione locale iniziale della telecamera
    private Vector3 originalCamPos;
    //indica la posizione locale della telecamera mentre il giocatore è in boost
    private Vector3 boostCamPos;

    private bool lookingBack = false;


    void Start()
    {
        //ottiene il riferimento allo script di movimento del kart del giocatore
        playerScript = player.GetComponent<PlayerKartCtrl>();
        //ottiene il riferimento alla telecamera
        cam = transform.GetChild(0);
        //ottiene il riferimento alla posizione locale iniziale della telecamera
        originalCamPos = cam.localPosition;
        //ottiene il riferimento alla posizione locale della telecamera mentre il giocatore è in boost
        boostCamPos = new Vector3(originalCamPos.x, originalCamPos.y + camBoostOffsetY, originalCamPos.z + camBoostOffsetZ);

    }

    private void Update()
    {
        //se il giocatore tiene premuto il tasto per guardare indietro...
        if (Input.GetKey(KeyCode.F))
        {
            //...se non lo sta già facendo...
            if (!lookingBack)
            {
                //...ruota il gameObject al contrario, mettendo la telecamera(sua figlia) davanti al giocatore...
                transform.localRotation = new Quaternion(transform.localRotation.x, 180, transform.localRotation.z, transform.localRotation.w);
                //...e comunica che la telecamera è stata ruotata
                lookingBack = true;

            }

        } //altrimenti, se non si sta più tenendo premuto il tasto per guardare indietro ma la telecamera è ancora ruotata...
        else if (lookingBack)
        {
            //...la riporta alla rotazione originale...
            //transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            transform.localRotation = new Quaternion(transform.localRotation.x, 0, transform.localRotation.z, transform.localRotation.w);
            //...e comunica che la telecamera non è più ruotata
            lookingBack = false;

        }

    }

    void LateUpdate()
    {
        //transform.position = player.position + offset;
        /*
        //se il giocatore non sta volando in glide...
        if (!playerScript.GLIDER_FLY)
        {
            //...la rotazione della telecamera segue quella del giocatore normalmente
            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(player.rotation.x, player.rotation.y,
                player.rotation.z, player.rotation.w), camSpeed * Time.deltaTime); //normal

        } //altrimenti, fa in modo che la rotazione della telecamera cambi solo nell'asse Y
        else
        {
            //we only want the camera rotate on the y axis. Not the x or z axis since when the player is turning in the air while gliding, the player is tilting a lot, so we do not want the camera to tilt sideways too.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, player.eulerAngles.y, 0), camSpeed * Time.deltaTime);

        }
        */

        //se il giocatore è in boost, la posizione della telecamera cambia in base alla posizione in boost calcolata
        if (playerScript.GetBoostTime() > 0)
        { cam.localPosition = Vector3.Lerp(cam.localPosition, boostCamPos, camSpeed * Time.deltaTime); }
        //altrimenti, la telecamera resterà nella posizione originale
        else
        { cam.localPosition = Vector3.Lerp(cam.localPosition, originalCamPos, camSpeed * Time.deltaTime); }
        
    }

}
