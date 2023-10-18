using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSensitivity : MonoBehaviour
{
    public Slider slider;
    public GameObject presentCamera;
    public GameObject pastCamera;
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Sensitivty", 2f) * 4;
    }
    public void UpdateSensitivity(float sliderValue)
    {
        PlayerPrefs.SetFloat("Sensitivty", sliderValue * 0.25f);
        presentCamera.GetComponent<CameraControllerFirst>().Sensitivity = PlayerPrefs.GetFloat("Sensitivty", 2f);
        pastCamera.GetComponent<CameraControllerFirst>().Sensitivity = PlayerPrefs.GetFloat("Sensitivty", 2f);
    }
}
