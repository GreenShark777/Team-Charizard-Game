using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ai_Progliettile : MonoBehaviour, IUsableItem
{

    //qui assegniamo il nemico bersaglio del nostro progliettile 
    private Transform bersaglio;
    //riferimento al bersaglio di default, nel caso non si trovino nemici come obiettivi
    private Transform bersaglioDefault;
    //riferimento al parent iniziale dell'oggetto
    private Transform previousParent;
    //riferimento alla linea del traguardo
    [SerializeField]
    private FinishLine finishLine = default;
    //array di riferimenti a tutti e 3 i nemici nella scena
    private Transform[] enemies = new Transform[3];
    //qui assegniamo la distanza di vista del progliettile 
    [SerializeField]
    private float distance = default;

    public int visione;

    public bool vedo;
    //indica se il missile ha ottenuto un nuovo bersaglio
    private bool gotNewTarget = false;
    //riferimento al NavMeshAgent del missile
    public NavMeshAgent agente;
    //indica l'indice da figlio del missile
    private int missileSiblingIndex = -1;


    private void Awake()
    {
        //ottiene il riferimento al parent iniziale dell'oggetto
        previousParent = transform.parent;
        //ottiene l'indice da figlio del missile
        missileSiblingIndex = transform.GetSiblingIndex();
        //ottiene il riferimento ai nemici in gara
        var enemiesInfo = finishLine.GetEnemiesInfos();
        int i = 0;
        foreach (EnemyCircuitInfos enemy in enemiesInfo) { enemies[i] = enemy.transform; i++; }
        //ottiene il riferimento al bersaglio di default
        bersaglioDefault = transform.GetChild(0);

    }

    private void FixedUpdate()
    {
        //se si è ancora con il bersaglio di default, il missile continua a cercare un nemico da colpire
        if (!gotNewTarget) { bersaglio = LockOnTarget(); Debug.Log("Cerca nuovo bersaglio"); }

        //distance = Vector3.Distance(bersaglio.position, this.transform.position);
        /*
        if (distance <= visione)
        {
            vedo = true;
        }
        if (distance > visione)
        {
            vedo = false;
        }

        if (vedo)
        {
            agente.isStopped = false;
            agente.SetDestination(bersaglio.position);
        }
        if (!vedo)
        {
            agente.isStopped = true;
        }
        */
    }

    public void UseThisItem()
    {
        //fa in modo che l'oggetto non sia figlio di nessuno
        transform.parent = null;
        //ottiene il riferimento al bersaglio da colpire
        bersaglio = LockOnTarget();

    }
    /// <summary>
    /// Ritorna il bersaglio per il missile
    /// </summary>
    /// <returns></returns>
    private Transform LockOnTarget()
    {

        float minDist = 0;

        int closestEnemy = -1;

        for (int enemy = 0; enemy < enemies.Length; enemy++)
        {

            float distToEnemy = Vector3.Distance(transform.position, enemies[enemy].position);

            if (distToEnemy < distance && (distToEnemy < minDist || minDist == 0)) { closestEnemy = enemy; minDist = distToEnemy; gotNewTarget = true; }

        }

        Transform target = (closestEnemy != -1) ? enemies[closestEnemy] : bersaglioDefault;

        agente.SetDestination(target.position);

        return target;

    }

    public void ResetMissile()
    {
        //riporta il missile figlio del suo parent originale all'indice in cui era prima
        transform.parent = previousParent;
        transform.SetSiblingIndex(missileSiblingIndex);

        gameObject.SetActive(false);

        gotNewTarget = false;

        bersaglio = bersaglioDefault;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }

}
