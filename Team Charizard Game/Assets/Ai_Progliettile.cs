//Si occupa del comportamento del missile a ricerca
using UnityEngine;
using UnityEngine.AI;

public class Ai_Progliettile : MonoBehaviour, IUsableItem
{
    //riferimento all'Animator del missile
    private Animator missileAnimator;
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
    //indica se il missile ha ottenuto un nuovo bersaglio
    private bool gotNewTarget = false;
    //riferimento al NavMeshAgent del missile
    public NavMeshAgent agente;
    //indica l'indice da figlio del missile
    private int missileSiblingIndex = -1;

    private Quaternion startRotation;


    private void Awake()
    {
        //ottiene il riferimento al parent iniziale dell'oggetto
        previousParent = transform.parent;
        //ottiene l'indice da figlio del missile
        missileSiblingIndex = transform.GetSiblingIndex();

        startRotation = transform.localRotation;
        //ottiene il riferimento ai nemici in gara
        var enemiesInfo = finishLine.GetEnemiesInfos();
        int i = 0;
        foreach (EnemyCircuitInfos enemy in enemiesInfo) { if(enemy) enemies[i] = enemy.transform; i++; }
        //ottiene il riferimento al bersaglio di default
        bersaglioDefault = transform.GetChild(0);
        //ottiene il riferimento all'Animator del missile
        missileAnimator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        //se si è ancora con il bersaglio di default, il missile continua a cercare un nemico da colpire
        if (!gotNewTarget) { /*bersaglio = */LockOnTarget(); /*Debug.Log("Cerca nuovo bersaglio");*/ }

    }

    private void OnTriggerEnter(Collider other)
    {
        //se il missile colpisce un nemico o un ostacolo, esplode
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacles")) { Explode(); }

    }

    /// <summary>
    /// Attiva il missile
    /// </summary>
    public void UseThisItem()
    {
        //fa in modo che l'oggetto non sia figlio di nessuno
        transform.parent = null;
        //ottiene il riferimento al bersaglio da colpire
        LockOnTarget();

    }
    /// <summary>
    /// Ritorna il bersaglio per il missile
    /// </summary>
    /// <returns></returns>
    private void LockOnTarget()
    {
        //indica la distanza del nemico più vicino controllato finora
        float minDist = 0;
        //indica l'indice nell'array di nemici del nemico più vicino
        int closestEnemy = -1;
        //cicla ogni nemico nell'array
        for (int enemy = 0; enemy < enemies.Length; enemy++)
        {

            if (enemies[enemy])
            {
                //calcola la distanza tra il missile e il nemico
                float distToEnemy = Vector3.Distance(transform.position, enemies[enemy].position);
                //se la distanza tra i 2 è poca e l'ultimo nemico controllato(se ne è stato controllato qualcuno) era più lontano, viene indicato questo nemico come quello più vicino
                if (distToEnemy < distance && (distToEnemy < minDist || minDist == 0)) { closestEnemy = enemy; minDist = distToEnemy; gotNewTarget = true; }

            }

        }
        //se nel ciclo è stato trovato un nuovo bersaglio, imposta quello come bersaglio(altrimenti verrà impostato quello di default)
        Transform target = (gotNewTarget) ? enemies[closestEnemy] : bersaglioDefault;
        //infine, viene impostato il nuovo bersaglio come destinazione del missile
        agente.SetDestination(target.position);

    }

    private void Explode()
    {
        //ferma l'agente del missile
        agente.isStopped = true;
        //fa partire l'animazione di esplosione del missile
        missileAnimator.SetTrigger("Explode");

    }
    /// <summary>
    /// Riporta il missile al suo stato originale(verrà richiamato dall'Animator)
    /// </summary>
    public void ResetMissile()
    {
        //riporta il missile figlio del suo parent originale all'indice in cui era prima
        transform.parent = previousParent;
        transform.SetSiblingIndex(missileSiblingIndex);

        transform.localRotation = startRotation;
        //l'agente del missile viene riattivato
        agente.isStopped = false;
        //viene disattivato il missile
        gameObject.SetActive(false);
        //viene impostato nuovamente come bersaglio il bersaglio di default
        gotNewTarget = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }

}
