using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    public AudioController audioController;
    private Slider slider;
    // Start is called before the first frame update
    private bool initializing = true; // Un indicateur pour savoir si le Slider est en cours d'initialisation

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(HandleVolumeChange);

        // Charger la valeur de volume � partir des PlayerPrefs et l'utiliser pour initialiser la position du curseur
        if (PlayerPrefs.HasKey("Volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("Volume");
            slider.value = savedVolume;
            // Assurer que le volume est �galement mis � jour dans AudioController
            audioController.SetVolume(savedVolume);
        }

        // Une fois l'initialisation termin�e, nous ne voulons plus emp�cher la mise � jour du volume
        initializing = false;
    }

    void Update()
    {
    }

    void HandleVolumeChange(float volume)
    {
        // Si nous sommes en train d'initialiser, ne pas mettre � jour le volume
        if (!initializing)
        {
            audioController.SetVolume(volume);
        }
    }
}
