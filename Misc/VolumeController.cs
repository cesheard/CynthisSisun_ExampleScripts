using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] string volumeParameter;
    [SerializeField] AudioMixerGroup mixer;
    [SerializeField] Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
        HandleSliderValueChanged(slider.value);
    }

    public void HandleSliderValueChanged(float value)
    {
        mixer.audioMixer.SetFloat(volumeParameter, Mathf.Log10(value) * 20);
    }
}