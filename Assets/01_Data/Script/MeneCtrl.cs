using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeneCtrl : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------
    [Header("���j���[")]
    [SerializeField] private GameObject menuObj;    // ���j���[���

    [Header("�e�L�X�g")]
    [SerializeField] private TMP_Text menuKeyText;  // ���j���[�̑���L�[�e�L�X�g

    [Header("�L�����o�X�O���[�v")]
    [SerializeField] private List<CanvasGroup> canvasGroups; 
    //-----privateField--------------------------------------------------------------
    private bool isOpenMenu = false;    // ���j���[�J�t���O (true => �J���Ă��� false => ���Ă���)


    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    /// <summary>
    /// �Ώ�Key���������烁�j���[�J��
    /// </summary>
    /// <param name="_context">�Ώ�Key</param>
    public void OnMenu(InputAction.CallbackContext _context)
    {
        if (!_context.performed) { return; }

        float speed = 0.2f; // ���j���[�J���x
        var rectPos = menuObj.GetComponent<RectTransform>();

        if (!isOpenMenu) // ���j���[�����Ă�����
        {
            isOpenMenu = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;

            // ���j���[�ȊO��UI�𓮂��Ȃ�����
            foreach(var group in canvasGroups)
            {
                group.blocksRaycasts = false;
            }

            menuKeyText.SetText("����");

            // ���j���[���ړ�
            rectPos.DOAnchorPos(new Vector2(0, 0), speed)
                                .SetEase(Ease.Linear)
                                .SetUpdate(true);
        }
        else
        {
            isOpenMenu = false;
            CursorState();
            Time.timeScale = 1f;

            // ���j���[�ȊO��UI�𓮂���
            foreach (var group in canvasGroups)
            {
                group.blocksRaycasts = true;
            }

            menuKeyText.SetText("�ݒ�");

            // ���j���[���ړ�
            rectPos.DOAnchorPos(new Vector3(0, -600), speed)
                                .SetEase(Ease.Linear)
                                .SetUpdate(true);
        }
    }

    /// <summary>
    /// �V�[���ɂ���ăJ�[�\���̏�Ԃ�ς���
    /// </summary>
    private void CursorState()
    {
        switch(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case MySceneManager.SceneData.TITLE:
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case MySceneManager.SceneData.SELECT:
                Cursor.lockState = CursorLockMode.Confined;
                break;
                
            case MySceneManager.SceneData.STAGE01:
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case MySceneManager.SceneData.STAGE02:
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

}
