using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Tooltip("ゲーム終了")]
    [SerializeField] GameObject gameClearObj;
    [SerializeField] GameObject gameOverObj;


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------
    static private GameObject clearObj;
    static private GameObject overObj;


    //-----protectedField------------------------------------------------------------


    public struct SceneData // シーン名の構造体
    {
        public string TITLE;
        public string SELECT;
        public string STAGE01;
    }
    static public SceneData sceneData;

    public enum GameEndStatus // ゲーム終了時の状態
    {
        CLEAR,
        OVER
    }

    private void Awake()
    {
        sceneData.TITLE = "Title";
        sceneData.SELECT = "Select";
        sceneData.STAGE01 = "Stage01";

        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case "Select":
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case "Stage01":
                Cursor.lockState = CursorLockMode.Locked;
                clearObj = gameClearObj;
                overObj = gameOverObj;
                break;
        }
    }

    /// <summary>
    /// シーンのロード
    /// </summary>
    /// <param name="_name">ロードシーン名</param>
    static public void OnLoadScene(string _name)
    {
        foreach(var _scene in GetSceneNames())
        {
            if(_name == _scene)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(_name,LoadSceneMode.Single);
                return;
            }
        }

        Debug.LogError("シーンが存在しません");
    }
    /// <summary>
    /// シーンの名前を取得
    /// </summary>
    /// <returns>シーンの名前配列</returns>
    static private string[] GetSceneNames()
    {
        // シーンの名前格納用の配列
        string[] sceneNameArray = new string[EditorBuildSettings.scenes.Length];

        // シーンの名前を取得、格納
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            string scenePath = EditorBuildSettings.scenes[i].path;
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            sceneNameArray[i] = sceneName;
        }

        return sceneNameArray;
    }


    /// <summary>
    /// ゲーム終了
    /// </summary>
    /// <param name="_status">終了状態</param>
    static public void GameFinish(GameEndStatus _status)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        AreaManager.enemyList.Clear();

        switch (_status)
        {
            case GameEndStatus.CLEAR:
                clearObj.SetActive(true);
                break;
            case GameEndStatus.OVER:
                overObj.SetActive(true);
                break;
        }
    }
}
