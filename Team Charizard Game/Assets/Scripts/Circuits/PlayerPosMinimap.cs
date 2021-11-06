using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosMinimap : MonoBehaviour
{
    //indica di quanto il movimento deve essere diviso
    [SerializeField]
    public float xMovementOffset = 0.01f,
        yMovementOffset = 0.01f;


    public void MovePlayerDot(Vector3 newVel)
    {
        //sposta il pallino del giocatore in base al vettore ricevuto diviso per l'offset
        transform.position = new Vector2(transform.position.x + (newVel.x * xMovementOffset), transform.position.y + (newVel.z * yMovementOffset));

    }

}
