using SimplicitySuite.FirstPersonController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    //Option tabs
    public GameObject audioTab;
    public GameObject videoTab;

    //Video settings variables
    public TMP_Dropdown windowModeDropdown;
    int windowModeValue = 1;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fpsDropdown;
    public TMP_Dropdown antiAliasingDropdown;
    public Toggle vSyncToggle;

    //Load options
    private void Start()
    {
        LoadOptions();
    }

    private void Update()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed && !resolutionDropdown.interactable)
            resolutionDropdown.interactable = true;
        else if (Screen.fullScreenMode != FullScreenMode.Windowed && resolutionDropdown.interactable)
            resolutionDropdown.interactable = false;
    }

    //Change options tab function
    public void ChangeTab(Button tabButton)
    {
        //Change the active tab to whichever button is selected
        if (tabButton.name == "Audio Button")
        {
            audioTab.SetActive(true);
            videoTab.SetActive(false);
        }
        else if (tabButton.name == "Video Button")
        {
            videoTab.SetActive(true);
            audioTab.SetActive(false);

            LoadOptions();
        }
    }

    //Change fullscreen mode function
    public void ChangeFullScreenMode(TMP_Dropdown value)
    {
        //Change fullscreen mode
        if (value.value == 0)
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else if (value.value == 1)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else if (value.value == 2)
            Screen.fullScreenMode = FullScreenMode.Windowed;

        //Store fullscreen mode
        PlayerPrefs.SetInt("windowMode", value.value);
    }

    //Change resolution function
    public void ChangeResolution(TMP_Dropdown value)
    {
        string[] resolutionString = value.options[value.value].text.Split('x');

        Screen.SetResolution(int.Parse(resolutionString[0]), int.Parse(resolutionString[1]), false);
        PlayerPrefs.SetString("resolution", int.Parse(resolutionString[0]) + "x" + int.Parse(resolutionString[1]));
    }
    //Change FPS max function
    public void ChangeFPSMax(TMP_Dropdown value)
    {
        if (value.value == 0)
        {
            Application.targetFrameRate = -1;
            PlayerPrefs.SetInt("fpsMax", 0);
        }
        else
        {
            Application.targetFrameRate = int.Parse((value.options[value.value].text).Split()[0]);
            PlayerPrefs.SetInt("fpsMax", int.Parse((value.options[value.value].text).Split()[0]));
        }
    }
    
    //Change antialiasing function
    public void ChangeAntiAliasing(TMP_Dropdown value)
    {
        Camera[] cameras = FindObjectsOfType<Camera>();
        foreach (Camera camera in cameras){
            if (value.options[value.value].text == "SMAA")
                camera.gameObject.GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            else if (value.options[value.value].text == "FXAA")
                camera.gameObject.GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.FastApproximateAntialiasing;
            else
                camera.gameObject.GetComponent<UniversalAdditionalCameraData>().antialiasing = AntialiasingMode.None;
        }

        PlayerPrefs.SetString("antiAliasing", value.options[value.value].text);
    }

    //Cange vsync function
    public void ChangeVSync(Toggle toggle)
    {
        if (toggle.isOn)
            QualitySettings.vSyncCount = 1;
        else if (!toggle.isOn)
            QualitySettings.vSyncCount = 0;

        PlayerPrefs.SetInt("vSync", QualitySettings.vSyncCount);
    }

    //Load options function
    private void LoadOptions()
    {
        //Display the current window mode
        if (PlayerPrefs.HasKey("windowMode"))
            windowModeValue = PlayerPrefs.GetInt("widnowMode");
        if (windowModeDropdown)
            windowModeDropdown.value = windowModeValue;

        //Display the solution options
        resolutionDropdown.ClearOptions();
        Resolution[] resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        Array.Reverse(resolutions);
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutions[i].width + "x" + resolutions[i].height));
        }
        //Display the current resolution option
        if (PlayerPrefs.HasKey("resolution"))
        {
            for (int i = 0; i < resolutionDropdown.options.Count; i++)
            {
                if (resolutionDropdown.options[i].text == PlayerPrefs.GetString("resolution"))
                {
                    resolutionDropdown.value = i;
                }
            }
        }
        else
            resolutionDropdown.value = 0;

        //Display the current max fps option
        if (PlayerPrefs.HasKey("fpsMax"))
        {
            if (PlayerPrefs.GetInt("fpsMax") == 0)
                fpsDropdown.value = 0;
            else
            {
                for (int i = 0; i < fpsDropdown.options.Count; i++)
                {
                    if (fpsDropdown.options[i].text == PlayerPrefs.GetInt("fpsMax").ToString() + " FPS")
                        fpsDropdown.value = i;
                }
            }
        }
        else
            fpsDropdown.value = 0;

        //Display the current anti-aliasing mode
        if (PlayerPrefs.HasKey("antiAliasing"))
        {
            for (int i = 0; i < antiAliasingDropdown.options.Count; i++)
            {
                if (antiAliasingDropdown.options[i].text == PlayerPrefs.GetString("antiAliasing"))
                    antiAliasingDropdown.value = i;
            }
        }
        else
        {
            for (int i = 0; i < antiAliasingDropdown.options.Count; i++)
            {
                if (antiAliasingDropdown.options[i].text == "SMAA")
                    antiAliasingDropdown.value = i;
            }
        }

        //Display the current V-Sync toggle
        if (PlayerPrefs.HasKey("vSync"))
        {
            if (PlayerPrefs.GetInt("vSync") == 0)
                vSyncToggle.isOn = false;
            else if (PlayerPrefs.GetInt("vSync") == 1)
                vSyncToggle.isOn = true;
        }
        else
            vSyncToggle.isOn = false;

        //Refresh dropdowns
        windowModeDropdown.RefreshShownValue();
        resolutionDropdown.RefreshShownValue();
    }
}
