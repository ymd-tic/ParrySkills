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
    [Header("メニュー")]
    [SerializeField] private GameObject menuObj;    // メニュー画面

    [Header("テキスト")]
    [SerializeField] private TMP_Text menuKeyText;  // メニューの操作キーテキスト

    [Header("キャンバスグループ")]
    [SerializeField] private List<CanvasGroup> canvasGroups; 
    //-----privateField--------------------------------------------------------------
    private bool isOpenMenu = false;    // メニュー開閉フラグ (true => 開いている false => 閉じている)


    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    /// <summary>
    /// 対象Keyを押したらメニュー開閉
    /// </summary>
    /// <param name="_context">対象Key</param>
    public void OnMenu(InputAction.CallbackContext _context)
    {
        if (!_context.performed) { return; }

        float speed = 0.2f; // メニュー開閉速度
        var rectPos = menuObj.GetComponent<RectTransform>();

        if (!isOpenMenu) // メニューが閉じていたら
        {
            isOpenMenu = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;

            // メニュー以外のUIを動かなくする
            foreach(var group in canvasGroups)
            {
                group.blocksRaycasts = false;
            }

            menuKeyText.SetText("閉じる");

            // メニューを移動
            rectPos.DOAnchorPos(new Vector2(0, 0), speed)
                                .SetEase(Ease.Linear)
                                .SetUpdate(true);
        }
        else
        {
            isOpenMenu = false;
            CursorState();
            Time.timeScale = 1f;

            // メニュー以外のUIを動かす
            foreach (var group in canvasGroups)
            {
                group.blocksRaycasts = true;
            }

            menuKeyText.SetText("設定");

            // メニューを移動
            rectPos.DOAnchorPos(new Vector3(0, -600), speed)
                                .SetEase(Ease.Linear)
                                .SetUpdate(true);
        }
    }

    /// <summary>
    /// シーンによってカーソルの状態を変える
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
