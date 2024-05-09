using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    public AudioController audioController;
    private Slider slider;
    private bool initializing = true; 

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(HandleVolumeChange);

        if (PlayerPrefs.HasKey("Volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            slider.value = savedVolume;
            audioController.SetVolume(savedVolume);
        }
        initializing = false;
    }

    void Update()
    {
    }

    void HandleVolumeChange(float volume)
    {
        if (!initializing)
        {
            audioController.SetVolume(volume);
        }
    }
}
