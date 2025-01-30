using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioCtrl : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------
    [Header("�X���C�_�[")]
    [SerializeField] private Slider bgmSlider;

    [Header("�I�[�f�B�I�~�L�T�[")]
    [SerializeField] private AudioMixer audioMixer;

    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------


    //-----protectedField------------------------------------------------------------

    private void Start()
    {
        //�~�L�T�[��volume�ɃX���C�_�[��volume�����Ă܂��B

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
