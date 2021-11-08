//Si occupa del comportamento dei collezionabili
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    //riferimento al testo che ndica il numero di collezionabili ottenuti
    [SerializeField]
    private Text nCollectedText = default;
    //riferimento all'Animator di questo collezionabile
    private Animator collectableAnim;
    //indica quanto velocemente, o lentamente, deve andare l'animazione di ottenimento
    [SerializeField]
    private float animSpeed = 1;
    //indica il numero massimo di collezionabili che il giocatore può tenere
    [SerializeField]
    private int maxCollectables = 10;
    //indica quanti collezionabili di questo tipo sono stati presi dal giocatore
    private static int nCollected = 0;


    private void Awake()
    {
        //ottiene il riferimento all'Animator di questo collezionabile
        collectableAnim = GetComponent<Animator>();
        //cambia la velocità d'animazione del collezionabile
        collectableAnim.speed = animSpeed;
        //resetta all'Awake il numero di collezionabili raccolti
        nCollected = 0;
        //resetta il testo che indica il numero di collezionabili ottenuti dal giocatore
        nCollectedText.text = "" + nCollected;

    }

    private void OnTriggerEnter(Collider other)
    {
        //se si collide con il giocatore, viene ottenuto da quest'ultimo
        if (other.CompareTag("Player")) { Obtained(true); }
        //altrimenti, se si collide con un nemico, viene ottenuto ma non aggiorna il testo che indica quanti collezionabili sono stati presi dal giocatore
        else if (other.CompareTag("Enemy")) { Obtained(false); }

    }

    private void Obtained(bool gotByPlayer)
    {
        //se il giocatore non è già al numero massimo di collezionabili presi, effettua i vari controlli
        if (nCollected < maxCollectables)
        {
            //se è stato ottenuto dal giocatore...
            if (gotByPlayer)
            {
                //...incrementa il numero di collezionabili presi da quest'ultimo...
                nCollected++;
                //...e aggiorna il testo
                nCollectedText.text = "" + nCollected;

            }

        }
        //se siamo al numero massimo di collezionabili, il colore del testo diventa rosso
        if (nCollected == maxCollectables) { nCollectedText.color = Color.red; }
        //altrimenti, se per qualche motivo il numero di collezionabili è oltre il massimo, lo riporta al valore massimo
        else if (nCollected > maxCollectables) { nCollected = maxCollectables; }
        //viene fatto diventare, in ogni caso, invisibile e non interagibile questo collezionabile per un po' di tempo
        collectableAnim.SetTrigger("obtained");
        Debug.Log("Ottenuto collezionabile");
    }

}
