using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Scr_MenuManager : MonoBehaviour
{
    public static Scr_MenuManager instance = null;

    [Header("UI References")]
    public Slider[] sliders;
    public TMP_InputField carNameInputField;
    public Dropdown driveTypeDropdown;

    [Header("Data Values")]
    public float[] slidersValues;
    public string carName;
    public int driveTypeIndex;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SaveCustomValue()
    {
        slidersValues = new float[sliders.Length];

        for (int i = 0; i < sliders.Length; i++)
        {
            slidersValues[i] = sliders[i].value;
        }

        carName = carNameInputField.text;

        driveTypeIndex = driveTypeDropdown.value;


        Invoke("GoToTrainingScene", 5f);
    }

    private void GoToTrainingScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void SaveLoadout()
    {
        List<Scr_CustomLoadout> loadouts = new List<Scr_CustomLoadout>();

        //for (int i = 0; i < slidersValues.Length; i++)
        //{
        //    nets.Add(agents[i].net);
        //    loadouts.Add(slidersValues[i])
        //}


    }


}
