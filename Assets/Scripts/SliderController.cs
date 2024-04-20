using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    public AudioController audioController;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(HandleVolumeChange);

    }

    // Update is called once per frame
    void Update()
    {
    }

    void HandleVolumeChange(float volume)
    {
        // Appeler la m�thode SetVolume du script AudioController pour mettre � jour le volume
        audioController.SetVolume(volume);
    }
}
