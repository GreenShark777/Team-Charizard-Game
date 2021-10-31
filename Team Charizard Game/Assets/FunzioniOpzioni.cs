using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FunzioniOpzioni : MonoBehaviour
{
    public Dropdown risoluzioneDropdown;
    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions;
        risoluzioneDropdown.ClearOptions();

        List<string> opzioni = new List<string>();

        int currentResolutionIndex = 0;
        for (int i=0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            opzioni.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        risoluzioneDropdown.AddOptions(opzioni);
        risoluzioneDropdown.value = currentResolutionIndex;
        risoluzioneDropdown.RefreshShownValue();

    }


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



}
