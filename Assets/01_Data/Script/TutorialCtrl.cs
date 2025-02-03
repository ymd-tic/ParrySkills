using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

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
    [Header("説明パネル")]
    [SerializeField] private TutorialPanel panelLeft;       // 説明パネル左
    [SerializeField] private TutorialPanel panelRight;      // 説明パネル右

    [Header("テキスト")]
    [SerializeField] private TMP_Text tutoriaKeyText;  // パネルの操作Keyテキスト

    [Header("キャンバスグループ")]
    [SerializeField] private List<CanvasGroup> canvasGroups;

    [Header("メニュー")]
    [SerializeField] private MenuCtrl menuCtrl;

    //-----privateField--------------------------------------------------------------
    private bool isOpenTutorial = false;    // パネル開閉フラグ (true => 開いている false => 閉じている)
    private float speed = 0.2f; // パネル開閉速度


    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------


    private void Start()
    {
        panelLeft.rect = panelLeft.panel.GetComponent<RectTransform>();
        panelRight.rect = panelRight.panel.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 対象Keyを押したら説明パネル開閉
    /// </summary>
    /// <param name="_context">対象Key</param>
    public void OnTutorial(InputAction.CallbackContext _context)
    {
        if (!_context.performed) { return; }
        if(menuCtrl.isOpenMenu)  { return; }

        if(!isOpenTutorial) // パネルが閉じていたら
        {
            isOpenTutorial = true;
            menuCtrl.enabled = false;
            tutoriaKeyText.SetText("閉じる");

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
            tutoriaKeyText.SetText("ゲーム説明");

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
