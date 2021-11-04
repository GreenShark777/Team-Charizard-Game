//Si occupa del comportamento del laser movente
using UnityEngine;

public class MovingLaser : MonoBehaviour, IGiveDamage
{
    //indica la posizione in cui il laser deve andare
    [SerializeField]
    private Vector3 targetPos = default;
    //indica la posizione iniziale del laser, in cui deve tornare dopo aver raggiunto la posizione target
    private Vector3 startPos;
    //indica la posizione in cui deve andare il laser
    private Vector3 currentTarget;
    //indica quanto velocemente deve muoversi verso la prossima posizione il laser
    [SerializeField]
    private float moveSpeed = 1;
    //indica quanto deve essere vicino il laser ad un punto prima di cambiare target
    [SerializeField]
    private float dist = 0.1f;
    //indica quanto danno prende il giocatore quando collide con questo oggetto
    [SerializeField]
    private float dmg = 1;


    private void Awake()
    {
        //ottiene la posizione iniziale di questo laser
        startPos = transform.position;
        //ottiene la posizione target
        targetPos += startPos;
        //imposta la posizione target come prima posizione in cui andare
        currentTarget = targetPos;

    }

    private void FixedUpdate()
    {
        //muove continuamente il laser movente verso la posizione corrente
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
        //se si è arrivati alla posizione target, la cambia a quella opposta alla posizione in cui si è appena arrivati
        if (Vector3.Distance(transform.position, currentTarget) < dist) { currentTarget = (currentTarget == startPos) ? targetPos : startPos; }

    }

    public float GiveDamage()
    {
        //ritorna il danno che dev infliggere al giocatore
        return dmg;

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        if(startPos == Vector3.zero)
        {
            Gizmos.DrawLine(transform.position, targetPos + transform.position);
            Gizmos.DrawWireSphere(targetPos + transform.position, 1);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
        else
        {
            Gizmos.DrawLine(transform.position, targetPos);
            Gizmos.DrawWireSphere(targetPos, 1);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, startPos);
            Gizmos.DrawWireSphere(startPos, 1);
        }

    }

}
