using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialCtrl : MonoBehaviour
{
    [Serializable]
    private class TutorialPanel
    {
        public GameObject panel;

        [NonSerialized]
        public RectTransform rect;
    }

    //-----SerializeField------------------------------------------------------------
    [Header("�����p�l��")]
    [SerializeField] private TutorialPanel panelLeft;       // �����p�l����
    [SerializeField] private TutorialPanel panelRight;      // �����p�l���E

    [Header("�ΏۃL�[")]
    [SerializeField] private TMP_Text tutoriaKeyText;  // �p�l���̑���Key�e�L�X�g
    [SerializeField] private Button tutoriaKeyBtn;  // �p�l���̑���KeyBtn

    [Header("�L�����o�X�O���[�v")]
    [SerializeField] private List<CanvasGroup> canvasGroups;

    [Header("���j���[")]
    [SerializeField] private MenuCtrl menuCtrl;

    //-----privateField--------------------------------------------------------------
    private bool isOpenTutorial = false;    // �p�l���J�t���O (true => �J���Ă��� false => ���Ă���)
    private float speed = 0.2f; // �p�l���J���x


    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------


    private void Start()
    {
        panelLeft.rect = panelLeft.panel.GetComponent<RectTransform>();
        panelRight.rect = panelRight.panel.GetComponent<RectTransform>();
    }

    /// <summary>
    /// �Ώ�Key��������������p�l���J��
    /// </summary>
    /// <param name="_context">�Ώ�Key</param>
    public void OnTutorial(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            ExecuteEvents.Execute(tutoriaKeyBtn.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            return; 
        }
        else if(_context.canceled)
        {
            ExecuteEvents.Execute(tutoriaKeyBtn.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
            return ;
        }


        if (menuCtrl.isOpenMenu)  { return; }

        if (!isOpenTutorial) // �p�l�������Ă�����
        {
            isOpenTutorial = true;
            menuCtrl.enabled = false;
            tutoriaKeyText.SetText("����");


            panelLeft.rect.DOAnchorPos(new Vector2(89, -50), speed)
                                .SetEase(Ease.Linear)
                                .SetUpdate(true)
                                .SetLink(gameObject);

            panelRight.rect.DOAnchorPos(new Vector2(-95, -50), speed)
                                .SetEase(Ease.Linear)
                                .SetUpdate(true)
                                .SetLink(gameObject);

            foreach (CanvasGroup group in canvasGroups)
            {
                group.blocksRaycasts = false;
                group.DOFade(0,speed)
                    .SetUpdate(true)
                    .SetLink(gameObject);
            }
        }
        else
        {
            isOpenTutorial = false;
            menuCtrl.enabled = true;
            tutoriaKeyText.SetText("�Q�[������");

            panelLeft.rect.DOAnchorPos(new Vector2(-1130, -50), speed)
                                .SetEase(Ease.Linear)
                                .SetUpdate(true)
                                .SetLink(gameObject);

            panelRight.rect.DOAnchorPos(new Vector2(600, -50), speed)
                                .SetEase(Ease.Linear)
                                .SetUpdate(true)
                                .SetLink(gameObject);

            foreach (CanvasGroup group in canvasGroups)
            {
                group.blocksRaycasts = true;
                group.DOFade(1, speed)
                    .SetUpdate(true)
                    .SetLink(gameObject);
            }
        }
    }
}
