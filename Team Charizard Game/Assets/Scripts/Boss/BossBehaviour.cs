//Si occupa del comportamento del boss e le sue animazioni
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    //riferimento allo script per impostare la destinazione durante la gara
    private setdestination setDest;
    //riferimento all'Animator del boss
    private Animator bossAnim;
    //riferimento alla pedina da lanciare
    [SerializeField]
    private PedinaBoss pedina = default;
    //indica quanto forte verrà lanciata la pedina
    [SerializeField]
    private float throwForce = 10;


    private void Awake()
    {
        //ottiene il riferimento allo script per impostare la destinazione durante la gara
        setDest = GetComponent<setdestination>();
        //disabilita lo script di destinazione
        setDest.enabled = false;
        //ottiene il riferimento all'Animator della pedina da lanciare
        bossAnim = GetComponent<Animator>();

    }

    private void Update()
    {
        //DEBUG--------------------------------------------------------------------------------------------------------------------------DEBUG
        if (Input.GetKeyDown(KeyCode.F12)) { bossAnim.SetTrigger("Throw"); }
        //DEBUG--------------------------------------------------------------------------------------------------------------------------DEBUG
    }

    /// <summary>
    /// Fa comparire la pedina da lanciare(richiamato dall'animazione di lancio del boss)
    /// </summary>
    public void SpawnPedina() { pedina.Spawn(); }
    /// <summary>
    /// Attiva la fisica alla pedina da lanciare e la lancia(richiamato dall'animazione di lancio del boss)
    /// </summary>
    /// <param name="thrown"></param>
    public void ThrowPedina() { pedina.Thrown(true, transform.forward * throwForce); }

}
