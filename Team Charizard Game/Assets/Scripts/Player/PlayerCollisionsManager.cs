//Si occupa delle collisioni del giocatore
using UnityEngine;

public class PlayerCollisionsManager : MonoBehaviour
{
    //riferimento allo script della vita del giocatore
    private PlayerHealth ph;


    private void Awake()
    {
        //ottiene il riferimento allo script della vita del giocatore
        ph = GetComponent<PlayerHealth>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //cerca di prendere il riferimento all'interfaccia dell'oggetto con cui si è colliso
        IGiveDamage dmgGiver = other.GetComponent<IGiveDamage>();
        //se il riferimento esiste, il giocatore riceve danno
        if (dmgGiver != null) { PlayerGotHit(dmgGiver); }
        //Debug.Log("Collision: " + dmgGiver);
    }

    private void PlayerGotHit(IGiveDamage dmgGiver)
    {
        //il giocatore riceve danno in base al danno dell'oggetto di cui si ha riferimento
        ph.ChangeHealth(-dmgGiver.GiveDamage());
        Debug.Log("Player Got Hit: " + dmgGiver.GiveDamage());
    }

}
