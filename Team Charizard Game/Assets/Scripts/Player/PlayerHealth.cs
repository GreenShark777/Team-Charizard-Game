//Si occupa di gestire la vita del giocatore e ciò che deve succedere quando viene colpito
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //riferimento allo script di cambio dello Slider che indica la vita rimanente al giocatore
    [SerializeField]
    private SliderChange healthSlider = default;
    //riferimento allo script di respawn del giocatore, in caso di morte
    [SerializeField]
    private RespawnPlayer respawner = default;

    [SerializeField]
    private float health = 100, //indica la vita corrente del giocatore
        reduceHitTimer = 0.5f, //indica per quanto tempo il giocatore riceve meno danni da attacchi(ciò avviene quando il giocatore viene colpito)
        reductionRate = 0.5f; //indica di quanto il danno ricevuto viene diminuito per attacchi in sequenza

    private float maxHealth; //indica la vita massima del giocatore

    //indica se i danni che il giocatore deve ricevere devono essere ridotti o meno
    private bool reduceDmg = false;

    /*private Coroutine sliderChangeCoroutine, //riferimento alla Coroutine di cambio valore di slider attivo
        reduceDmgCoroutine; //riferimento alla Coroutine di riduzione del danno*/


    private void Start()
    {
        //ottiene la vita massima
        maxHealth = health;
        //imposta il valore massimo dello slider della vita
        healthSlider.ChangeSliderMaxValue(maxHealth);
        //porta il valore attuale dello slider alla vita massima del giocatore
        healthSlider.ChangeSliderValue(maxHealth);

    }
    /// <summary>
    /// Cambia la vita corrente del giocatore in base al parametro ricevuto, capendo anche se sta ricevendo danno o meno
    /// </summary>
    /// <param name="value"></param>
    public void ChangeHealth(float value)
    {
        //se il giocatore non è ancora stato distrutto, effettua i cambia la sua vita in base al parametro ricevuto
        if (health > 0)
        {
            //calcola la vita che il giocatore dovrebbe avere ricevendo il nuovo valore
            float calculatedHealth = health + value;
            //se il valore ricevuto è minore di 0, vuol dire che sta ricevendo danno, quindi...
            if (value < 0)
            {
                //...se il danno deve essere ridotto, la vita viene ricalcolata...
                if (reduceDmg) { calculatedHealth -= value * reductionRate; /*Debug.Log("Reduced Damage");*/ }
                //...e fa partire la coroutine per il timer che indica per quanto tempo i danni ricevuti vengono diminuiti
                //if (reduceDmgCoroutine != null) { StopCoroutine(reduceDmgCoroutine); }
                /*reduceDmgCoroutine = */StartCoroutine(ReduceDamage());

            }
            else //altrimenti sta ricevendo vita, quindi...
            {

                //DARE UN FEEDBACK?

            }
            //in ogni caso, la vita del giocatore viene cambiata in base al valore ricevuto
            health = calculatedHealth;
            //la barra della vita del giocatore viene lentamente cambiata per combaciare con la vita attuale del giocatore
            //if (sliderChangeCoroutine != null) { StopCoroutine(sliderChangeCoroutine); /*Debug.LogError("Stop changing");*/ }
            /*sliderChangeCoroutine = */StartCoroutine(healthSlider.ChangeSliderValueSlowly(health > healthSlider.GetSliderValue(), health));
            //infine, se il giocatore ha perso tutta la vita viene fatta partire la coroutine di distruzione temporanea
            if (health <= 0) { StartCoroutine(PlayerDestroyed()); }

        }

    }

    /*
    /// <summary>
    /// Cambia lentamente il valore dello slider, fino ad arrivare al valore corrente della vita del giocatore
    /// </summary>
    /// <param name="increment"></param>
    /// <returns></returns>
    private IEnumerator ChangeSliderValue(bool increment)
    {
        //cambia il valore dello slider della vita per pareggiare la vita attuale del giocatore
        healthSlider.ChangeSliderValue(healthSlider.GetSliderValue() + (sliderValueChangeRate * (increment ? Time.deltaTime : -Time.deltaTime)));
        //aspetta la fine del frame
        yield return new WaitForEndOfFrame();
        //Debug.Log("Dist: " + (healthSlider.value - health));
        //se la differenza tra il valore nello slider e la vita del giocatore è ancora troppo grande, continua a cambiare il valore dello slider
        if (Mathf.Abs(healthSlider.GetSliderValue() - health) >= acceptableDist) { StartCoroutine(ChangeSliderValue(health > healthSlider.GetSliderValue())); yield break; }
        else if (sliderValueChangeRate != startSliderChangeRate) { sliderValueChangeRate = startSliderChangeRate; }
        
    }
    */

    /// <summary>
    /// Fa ridurre i danni che il giocatore subisce per un po' di tempo
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReduceDamage()
    {
        //Debug.Log("Start reduction");
        //comunica che i danni che il giocatore deve ricevere devono essere per ora diminuiti
        reduceDmg = true;
        //aspetta un po'
        yield return new WaitForSeconds(reduceHitTimer);
        //comunica che i danni che il giocatore deve ricevere non devono più essere diminuiti
        reduceDmg = false;
        //Debug.Log("No more reduction");
    }
    /// <summary>
    /// Comunica al Respawner che il giocatore è stato distrutto
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerDestroyed()
    {
        //porta la vita al valore minimo possibile
        health = 0;
        //comunica allo script di respawn che il giocatore è stato sconfitto
        respawner.PlayerFellOrWasDefeated();
        //aspetta un po'
        yield return new WaitForSeconds(respawner.GetActualRespawnTime());
        //fa in modo che la barra della vita si ricarichi più velocemente per questa volta
        healthSlider.FastenChangeRate();
        //riporta la vita al valore massimo
        health = 0.1f;
        ChangeHealth(maxHealth);

    }

}
