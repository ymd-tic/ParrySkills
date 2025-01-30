using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioCtrl : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------
    [Header("スライダー")]
    [SerializeField] private Slider bgmSlider;

    [Header("オーディオミキサー")]
    [SerializeField] private AudioMixer audioMixer;

    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------


    //-----protectedField------------------------------------------------------------

    private void Start()
    {
        //ミキサーのvolumeにスライダーのvolumeを入れてます。

        //BGM
        audioMixer.GetFloat("BGM", out float bgmVolume);
        bgmSlider.value = bgmVolume;
        //SE
    }

    public void SetValumeBGM(float _volume)
    {
        audioMixer.SetFloat("BGM", _volume);
    }
}
