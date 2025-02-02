using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioCtrl : MonoBehaviour
{
    [Serializable]
    private class AudioData // ���f�[�^
    {
        public float volume; // ����
        public AudioClip[] clips; // �炷SE
    }

    //-----SerializeField------------------------------------------------------------
    [Header("�I�[�f�B�I�\�[�X")]
    [SerializeField] private AudioSource audioSource;

    [Header("���鉹")]
    [SerializeField] private AudioData runSE;  // ����
    [Header("����U�鉹")]
    [SerializeField] private AudioData atackSE;  // �U����
    [Header("������鉹")]
    [SerializeField] private AudioData rollingSE;  // �U����

    [Header("�_���[�W��")]
    [SerializeField] private AudioData damageVoice;  // �_���[�W��


    //-----privateField--------------------------------------------------------------


    //-----publicField---------------------------------------------------------------


    //-----staticField---------------------------------------------------------------



    //-----ComponentField------------------------------------------------------------


    /// <summary>
    /// ���鉹�̍Đ�
    /// </summary>
    private void SoundRunSE()
    {
        SetSE(runSE);
    }

    /// <summary>
    /// �U�����̍Đ�
    /// </summary>
    private void SoundAtackSE()
    {
        SetSE(atackSE);
    }

    /// <summary>
    /// ������̍Đ�
    /// </summary>
    private void SoundRollingSE()
    {
        SetSE(rollingSE);
    }

    private void SoundDamageVoice()
    {
        SetSE(damageVoice);
    }

    /// <summary>
    /// SE�̐ݒ�
    /// </summary>
    /// <param name="_data">�炷SE</param>
    private void SetSE(AudioData _data)
    {
        // �炷��
        int se = UnityEngine.Random.Range(0, _data.clips.Length);

        audioSource.clip = _data.clips[se];    // SE�ݒ�
        audioSource.volume = _data.volume;     // ���ʐݒ�
        audioSource.Play();
    }
}
