//Si occupa di gestire la vita del giocatore e ciò che deve succedere quando viene colpito
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //riferimento allo Slider che indica la vita rimanente al giocatore
    [SerializeField]
    private Slider healthSlider = default;
    //riferimento allo script di respawn del giocatore, in caso di morte
    [SerializeField]
    private RespawnPlayer respawner = default;

    [SerializeField]
    private float health = 100, //indica la vita corrente del giocatore
        sliderValueChangeRate = 10, //indica quanto velocemente la barra della vita deve cambiare
        fasterSliderChangeRate = 100, //indica quanto velocemente la barra della vita deve cambiare quando deve essere più veloce
        acceptableDist = 0.1f, //indica quanto vicino deve almeno essere lo slider della vita per smettere di cambiare
        reduceHitTimer = 0.5f, //indica per quanto tempo il giocatore riceve meno danni da attacchi(ciò avviene quando il giocatore viene colpito)
        reductionRate = 0.5f; //indica di quanto il danno ricevuto viene diminuito per attacchi in sequenza

    private float maxHealth, //indica la vita massima del giocatore
        startSliderChangeRate; //indica il valore iniziale del valore di aumento dello slider della vita

    //indica se i danni che il giocatore deve ricevere devono essere ridotti o meno
    private bool reduceDmg = false;

    private Coroutine sliderChangeCoroutine, //riferimento alla Coroutine di cambio valore di slider attivo
        reduceDmgCoroutine; //riferimento alla Coroutine di riduzione del danno


    private void Awake()
    {
        //ottiene la vita massima
        maxHealth = health;
        //imposta il valore massimo dello slider della vita
        healthSlider.maxValue = maxHealth;
        //porta il valore attuale dello slider alla vita massima del giocatore
        healthSlider.value = maxHealth;
        //ottiene il valore iniziale del valore di aumento dello slider della vita
        startSliderChangeRate = sliderValueChangeRate;

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
                if (reduceDmgCoroutine != null) { StopCoroutine(reduceDmgCoroutine); }
                reduceDmgCoroutine = StartCoroutine(ReduceDamage());

            }
            else //altrimenti sta ricevendo vita, quindi...
            {



            }
            //in ogni caso, la vita del giocatore viene cambiata in base al valore ricevuto
            health = calculatedHealth;
            //la barra della vita del giocatore viene lentamente cambiata per combaciare con la vita attuale del giocatore
            if (sliderChangeCoroutine != null) { StopCoroutine(sliderChangeCoroutine); Debug.LogError("Stop changing"); }
            sliderChangeCoroutine = StartCoroutine(ChangeSliderValue(health > healthSlider.value));
            //infine, se il giocatore ha perso tutta la vita viene fatta partire la coroutine di distruzione temporanea
            if (health <= 0) { StartCoroutine(PlayerDestroyed()); }

        }

    }
    /// <summary>
    /// Cambia lentamente il valore dello slider, fino ad arrivare al valore corrente della vita del giocatore
    /// </summary>
    /// <param name="increment"></param>
    /// <returns></returns>
    private IEnumerator ChangeSliderValue(bool increment)
    {
        //cambia il valore dello slider della vita per pareggiare la vita attuale del giocatore
        healthSlider.value += sliderValueChangeRate * (increment ? Time.deltaTime : -Time.deltaTime);
        //aspetta la fine del frame
        yield return new WaitForEndOfFrame();
        Debug.Log("Dist: " + (healthSlider.value - health));
        //se la differenza tra il valore nello slider e la vita del giocatore è ancora troppo grande, continua a cambiare il valore dello slider
        if (Mathf.Abs(healthSlider.value - health) >= acceptableDist) { StartCoroutine(ChangeSliderValue(health > healthSlider.value)); yield break; }
        else if (sliderValueChangeRate != startSliderChangeRate) { sliderValueChangeRate = startSliderChangeRate; }
        
    }
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

    private IEnumerator PlayerDestroyed()
    {
        //porta la vita al valore minimo possibile
        health = 0;
        //comunica allo script di respawn che il giocatore è stato sconfitto
        respawner.PlayerFellOrWasDefeated();
        //aspetta un po'
        yield return new WaitForSeconds(respawner.GetActualRespawnTime());
        //fa in modo che la barra della vita si ricarichi più velocemente per questa volta
        sliderValueChangeRate = fasterSliderChangeRate;
        //riporta la vita al valore massimo
        health = 0.1f;
        ChangeHealth(maxHealth);

    }

}
