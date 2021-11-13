//Si occupa dell'effetto del silezionatore
using System.Collections;
using UnityEngine;

public class Silencer : MonoBehaviour, IUsableItem
{
    //riferimento allo script della vita del giocatore
    [SerializeField]
    private PlayerHealth ph = default;
    //indica quanto dura l'effetto del silenziatore
    [SerializeField]
    private float actionTimer = 3;


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
        //aspetta che finisca l'effetto
        yield return new WaitForSeconds(actionTimer);
        //il giocatore non è più invincibile
        ph.IsPlayerInvincible(false);
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
