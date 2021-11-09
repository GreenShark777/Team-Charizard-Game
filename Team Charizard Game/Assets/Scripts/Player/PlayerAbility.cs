//Si occupa della gestione dell'abilità del giocatore
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    //riferimento allo Slider che indica la carica dell'abilità del giocatore
    [SerializeField]
    private SliderChange abilitySlider = default;
    //riferimento allo scudo del giocatore, da attivare quando quest'ultimo usa la sua abilità
    [SerializeField]
    private GameObject playerShield = default;
    //riferimento allo script che si occupa della vita del giocatore
    private PlayerHealth ph;

    [SerializeField]
    private float charge = 100, //indica il valore corrente della carica dell'abilità del giocatore
        chargeAmount = 10, //indica quanto deve ricaricarsi l'abilità ogni volta che la funzione Recharge viene richiamata
        recoveryAmount = 5; //indica quanta vita recupera il giocatore, nel caso l'abilità sia completamente carica

    //indica la carica massima della carica del giocatore
    private float maxCharge;
    //indica che l'abilità del giocatore può essere utilizzata
    private bool canUse = true;
    //riferimento al metodo da richiamare quando finisce l'abilità
    private SliderChange.MethodToRecall abilityEnded;


    private void Start()
    {
        //ottiene la carica massima
        maxCharge = charge;
        //imposta il valore massimo dello slider della carica dell'abilità al valore massimo
        abilitySlider.ChangeSliderMaxValue(maxCharge);
        //imposta il valore corrente dello slider dell'abilità al valore massimo
        abilitySlider.ChangeSliderValue(maxCharge);
        //imposta la funzione EndOfAbility come metodo da richiamare
        abilityEnded = EndOfAbility;
        //ottiene il riferimento allo script che si occupa della vita del giocatore
        ph = GetComponent<PlayerHealth>();

    }

    private void Update()
    {
        //se si può utilizzare la abilità del giocatore...
        if (canUse)
        {
            //...premendo il tasto indicato, si potrà utilizzare
            if (Input.GetButtonDown("UseAbility")) { UsePlayerAbility(); }

        }
        
    }

    /// <summary>
    /// Ricarica di un po' l'abilità del giocatore
    /// </summary>
    public void Recharge(bool usedAbility = false)
    {
        //se non si può ancora usare l'abilità e non è in uso...
        if (!canUse && !ph.IsShieldActive())
        {
            //...ricarica l'abilità...
            charge += chargeAmount;
            //...comunica se si può utilizzare o meno l'abilità, in base al livello di carica...
            canUse = charge >= maxCharge;
            //...e cambia il valore dello slider dell'abilità(nel caso l'abilità sia stata usata, passa come riferimento il metodo da richiamare a fine abilità)
            StartCoroutine(abilitySlider.ChangeSliderValueSlowly(true, charge, (usedAbility) ? abilityEnded : null));

        }
        else //altrimenti, non potendo più ricaricare la propria abilità...
        {
            //...il giocatore recupera della vita
            ph.ChangeHealth(recoveryAmount);

        }

    }
    /// <summary>
    /// Attiva l'abilità del giocatore
    /// </summary>
    private void UsePlayerAbility()
    {
        //comunica che l'abilità non può più essere utilizzata
        canUse = false;
        //riporta la carica a 0 e cambia il valore dello slider
        charge = 0 - chargeAmount;
        Recharge(true);
        //attiva lo scudo
        playerShield.SetActive(true);
        //comunica allo script della vita del giocatore che lo scudo è attivo
        ph.ShieldStatus(true);
        //Debug.Log("Used Ability: " + ph.IsShieldActive());
    }
    /// <summary>
    /// Termina il continuo dell'abilità del giocatore
    /// </summary>
    public void EndOfAbility()
    {
        //disattiva lo scudo del giocatore
        playerShield.SetActive(false);
        //comunica allo script della vita del giocatore che lo scudo non è più attivo
        ph.ShieldStatus(false);
        //Debug.Log("End Of Ability: " + ph.IsShieldActive());
    }

    public void RefillCharge()
    {
        //fa in modo che la barra dell'abilità si ricarichi più velocemente per questa volta
        abilitySlider.FastenChangeRate();
        //riporta la carica al massimo e cambia il valore dello slider
        charge = maxCharge - chargeAmount;
        Recharge();

    }

}
