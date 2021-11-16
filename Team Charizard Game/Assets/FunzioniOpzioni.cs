using System.Collections;using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class FunzioniOpzioni : MonoBehaviour, IUpdateData
{
    public Dropdown graficaDropdown, risoluzioneDropdown;
    Resolution[] resolutions;
    public Toggle toggleFullSchermo;

    [SerializeField]
    private GameManag g = default;

    //[SerializeField]
    //private Slider sliderVol_Musica = null, sliderVol_SFX = null, sliderVol_GLobal = null;
    //[SerializeField]
    //private AudioMixer master= default;
    private void Start()
    {
        // qui stiammo dicendo di prendere i valori salvati
      //  if(sliderVol_Musica) sliderVol_Musica.value = g.savedMusicVolume;
      //if(sliderVol_GLobal) sliderVol_GLobal.value = g.savedMasterVolume;
      // if(sliderVol_SFX) sliderVol_SFX.value= g.savedSfxVolume;
        //
        resolutions = Screen.resolutions;
        risoluzioneDropdown.ClearOptions();

        List<string> opzioni = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            opzioni.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        risoluzioneDropdown.AddOptions(opzioni);
        risoluzioneDropdown.value = currentResolutionIndex;
        risoluzioneDropdown.RefreshShownValue();
        graficaDropdown.value = g.qualita;
        risoluzioneDropdown.value = g.risoluzione;
        toggleFullSchermo.isOn = g.fullSchermo;
        gameObject.SetActive(false);
    }

    //public void CambiaVolumeMusic(float newvolume)
    //{
    //    master.SetFloat("musicVolume",newvolume);
    
    //}
    //public void CambiaVolumeSFX(float newvolume)
    //{
    //    master.SetFloat("sfxVolume", newvolume);

    //}
    //public void CambiaVolumeGlobal(float newvolume)
    //{
    //    master.SetFloat("globalVolume", newvolume);

    //}
    public void SettaRisoluzione(int resoluzioneIndex)
    {
        Resolution resolution = resolutions[resoluzioneIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

    }
    public void SetaQualità( int qualitaIndex)
    {
        QualitySettings.SetQualityLevel(qualitaIndex);
    }

    public void SetFullShermo(bool isFullSchermo)
    {
        Screen.fullScreen = isFullSchermo;
    }

    public void UpdateData()
    {
        // serve per salvare la qualita selezionata 
        g.qualita = graficaDropdown.value;
        g.risoluzione = risoluzioneDropdown.value;
        Debug.Log("Valiu"+graficaDropdown.value);
        g.fullSchermo = toggleFullSchermo.isOn;
        //if(sliderVol_SFX)  g.savedSfxVolume = sliderVol_SFX.value;
        //if(sliderVol_Musica)  g.savedMusicVolume = sliderVol_Musica.value;
        //if(sliderVol_GLobal) g.savedMasterVolume = sliderVol_GLobal.value;
    }
}
