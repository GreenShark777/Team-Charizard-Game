//Si occupa del comportamento delle trappole che sparano un proiettile continuamente
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrapBehaviour : MonoBehaviour
{
    //riferimento al proiettile da sparare
    private Transform projectile;
    //riferimento al Rigidbody del proiettile
    private Rigidbody projectileRb = default;
    //riferimento alla posizione in cui il proiettile deve essere quando deve essere sparato
    private Vector3 startProjectilePos;

    [SerializeField]
    private float projectileSpeed = 2, //indica quanto velocemente deve andare il proiettile
        reshootTimer = 2; //indica quanto tempo deve passare prima che questa trappola possa nuovamente sparare


    private void Awake()
    {
        //ottiene il riferimento al proiettile da sparare
        projectile = transform.GetChild(0);
        //ottiene il riferimento al Rigidbody del proiettile
        projectileRb = projectile.GetComponent<Rigidbody>();
        //ottiene la posizione in cui il proiettile deve essere quando deve essere sparato
        startProjectilePos = projectile.position;
        //se la velocità impostata del proiettile è maggiore di 0, viene impostata al valore negativo del valore
        if (projectileSpeed > 0) { projectileSpeed = -projectileSpeed; }

    }

    void Start()
    {
        //fa partire la coroutine di sparo
        StartCoroutine(Shoot());
        
    }

    private IEnumerator Shoot()
    {
        //resetta ogni forza che agisce sul proiettile
        projectileRb.velocity = Vector3.zero;
        //riporta il proiettile alla posizione iniziale
        projectile.position = startProjectilePos;
        //spinge il proiettile verso la direzione in cui deve essere sparato
        projectileRb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(reshootTimer);
        //fa ripartire la stessa coroutine, facendo in modo che il loop non si interrompa mai
        StartCoroutine(Shoot());

    }

}
