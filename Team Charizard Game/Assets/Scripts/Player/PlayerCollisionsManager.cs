using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionsManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //cerca di prendere il riferimento all'interfaccia dell'oggetto con cui si è colliso
        IGiveDamage dmgGiver = other.GetComponent<IGiveDamage>();
        //se il riferimento esiste, il giocatore riceve danno
        if (dmgGiver != null) { PlayerGotHit(dmgGiver); }

    }

    private void PlayerGotHit(IGiveDamage dmgGiver)
    {

        //FARE PRENDERE DANNO AL GIOCATORE IN BASE AL FLOAT RICEVUTO DALLA FUNZIONE
        //vitaGiocatore(o riferimento al PlayerHealth e richiamare un suo metodo) -= dmgGiver.GiveDamage();

    }

}
