//Si occupa dell'effetto del silezionatore
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Silencer : MonoBehaviour, IUsableItem
{
    //riferimento allo script della vita del giocatore
    [SerializeField]
    private PlayerHealth ph = default;
    //riferimento al mixer degli audio
    [SerializeField]
    private AudioMixer masterMixer = default;
    
    [SerializeField]
    private float actionTimer = 3, //indica quanto dura l'effetto del silenziatore
        silencedVolume = -10; //indica quanto i suoni del gioco vengano diminuiti di volume

    //indica il volume iniziale dell'audio di gioco
    private float startVolume;


    private void Awake()
    {
        //ottiene il riferimento al volume iniziale dell'audio di gioco
        masterMixer.GetFloat("globalVolume", out startVolume);

    }

    public void UseThisItem()
    {
        //fa partire la coroutine di silenziamento
        StartCoroutine(SilencerEffect());

    }
    /// <summary>
    /// Attiva l'effetto del silenziatore, rendendo il giocatore invincibile per un po' e diminuendo il volume della musica
    /// </summary>
    /// <returns></returns>
    private IEnumerator SilencerEffect()
    {
        //il giocatore diventa invincibile
        ph.IsPlayerInvincible(true);
        //diminuisce il volume globale
        masterMixer.SetFloat("globalVolume", silencedVolume);
        //aspetta che finisca l'effetto
        yield return new WaitForSeconds(actionTimer);
        //il giocatore non è più invincibile
        ph.IsPlayerInvincible(false);
        //riporta il volume globale al valore originale
        masterMixer.SetFloat("globalVolume", startVolume);
        //il silenziatore torna al suo stato originale
        ResetSilencer();

    }
    /// <summary>
    /// Riporta il silenziatore al suo stato originale
    /// </summary>
    private void ResetSilencer()
    {
        //disattiva il silenziatore
        gameObject.SetActive(false);

    }

}
