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
    //riferimento allo script che si occupa dell'abilità del giocatore
    private PlayerAbility pa;
    //riferimento all'animator del kart del giocatore
    [SerializeField]
    private Animator playerKartAnim = default;
    //riferimento al Rigidbody del giocatore
    private Rigidbody playerRb;

    [SerializeField]
    private float health = 100, //indica la vita corrente del giocatore
        reduceHitTimer = 0.5f, //indica per quanto tempo il giocatore riceve meno danni da attacchi(ciò avviene quando il giocatore viene colpito)
        reductionRate = 0.5f; //indica di quanto il danno ricevuto viene diminuito per attacchi in sequenza

    //indica la vita massima del giocatore
    private float maxHealth;

    private bool reduceDmg = false, //indica se i danni che il giocatore deve ricevere devono essere ridotti o meno
        shieldActive = false, //indica se lo scudo del giocatore è attivo
        invincible = false; //indica se il giocatore è invincibile o meno(attivando il silenziatore)

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
        //ottiene il riferimento allo script che si occupa dell'abilità del giocatore
        pa = GetComponent<PlayerAbility>();

        playerRb = GetComponentInParent<Rigidbody>();

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
                //...se non è invincibile il giocatore può ricevere danno, quindi...
                if (!invincible)
                {
                    //...se lo scudo non è attivo...
                    if (!shieldActive)
                    {
                        //...controlla se il danno deve essere ridotto, nel qual caso la vita viene ricalcolata...
                        if (reduceDmg) { calculatedHealth -= value * reductionRate; /*Debug.Log("Reduced Damage");*/ }
                        //...e fa partire la coroutine per il timer che indica per quanto tempo i danni ricevuti vengono diminuiti
                        //if (reduceDmgCoroutine != null) { StopCoroutine(reduceDmgCoroutine); }
                        /*reduceDmgCoroutine = */
                        StartCoroutine(ReduceDamage());

                    }
                    else //altrimenti...
                    {
                        //...lo scudo viene disattivato...
                        pa.EndOfAbility();
                        //...e il giocatore non subisce danno
                        calculatedHealth = health;

                    }

                } //altrimenti, non subisce danno dato che è invincibile
                else { calculatedHealth = health; }

            }
            else //altrimenti sta ricevendo vita, quindi...
            {
                //...controlla se ha superato la vita massima, nel qual caso glielo impedisce
                calculatedHealth = Mathf.Clamp(calculatedHealth, maxHealth, maxHealth);

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
        //fa partire l'animazione di distruzione del giocatore
        playerKartAnim.SetBool("destroyed", true);
        //rimuove tutti i constraint alla rotazione del rigidbody del giocatore
        playerRb.constraints = RigidbodyConstraints.None;
        //fa in modo che la barra della vita si scarichi più velocemente per questa volta
        healthSlider.FastenChangeRate();
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
        //carica al massimo l'abilità del giocatore
        pa.RefillCharge();
        //fa terminare l'animazione di distruzione del giocatore
        playerKartAnim.SetBool("destroyed", false);
        //riporta tutti i constraint alla rotazione del rigidbody del giocatore
        playerRb.constraints = RigidbodyConstraints.FreezeRotation;

    }
    /// <summary>
    /// Permette di comunicare allo script se lo scudo del giocatore è attivo o meno
    /// </summary>
    /// <param name="status"></param>
    public void ShieldStatus(bool status) { shieldActive = status; }
    /// <summary>
    /// Ritorna lo stato dello scudo del giocatore
    /// </summary>
    /// <returns></returns>
    public bool IsShieldActive() { return shieldActive; }
    /// <summary>
    /// Permette di rendere il giocatore invincibile o meno
    /// </summary>
    /// <param name="isInvincible"></param>
    public void IsPlayerInvincible(bool isInvincible) { invincible = isInvincible; }

}
