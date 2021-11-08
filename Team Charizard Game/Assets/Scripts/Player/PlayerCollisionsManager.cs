//Si occupa delle collisioni del giocatore
using UnityEngine;

public class PlayerCollisionsManager : MonoBehaviour
{
    //riferimento allo script della vita del giocatore
    private PlayerHealth ph;
    //riferimento allo script dell'abilità del giocatore
    private PlayerAbility pa;


    private void Awake()
    {
        //ottiene il riferimento allo script della vita del giocatore
        ph = GetComponent<PlayerHealth>();
        //ottiene il riferimento allo script dell'abilità del giocatore
        pa = GetComponent<PlayerAbility>();

    }

    private void OnTriggerEnter(Collider other)
    {
        //cerca di prendere il riferimento all'interfaccia di danno dell'oggetto con cui si è colliso
        IGiveDamage dmgGiver = other.GetComponent<IGiveDamage>();
        //se il riferimento esiste, il giocatore riceve danno
        if (dmgGiver != null) { PlayerGotHit(dmgGiver); }
        //altrimenti...
        else
        {
            //...cerca di prendere il riferimento allo script da collezionabile dell'oggetto con cui si è colliso...
            Collectable collectable = other.GetComponent<Collectable>();
            //...e, se esiste, ricarica l'abilità del giocatore
            if (collectable) { pa.Recharge(); }

        }
        //Debug.Log("Collision: " + dmgGiver);
    }

    private void PlayerGotHit(IGiveDamage dmgGiver)
    {
        //il giocatore riceve danno in base al danno dell'oggetto di cui si ha riferimento
        ph.ChangeHealth(-dmgGiver.GiveDamage());
        Debug.Log("Player Got Hit: " + dmgGiver.GiveDamage());
    }

}
