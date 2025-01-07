using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Tooltip("�Q�[���I��")]
    [SerializeField] GameObject gameClearObj;
    [SerializeField] GameObject gameOverObj;


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------
    static private GameObject clearObj;
    static private GameObject overObj;


    //-----protectedField------------------------------------------------------------


    public struct SceneData // �V�[�����̍\����
    {
        public string TITLE;
        public string SELECT;
        public string STAGE01;
    }
    static public SceneData sceneData;

    public enum GameEndStatus // �Q�[���I�����̏��
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
    /// �V�[���̃��[�h
    /// </summary>
    /// <param name="_name">���[�h�V�[����</param>
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

        Debug.LogError("�V�[�������݂��܂���");
    }
    /// <summary>
    /// �V�[���̖��O���擾
    /// </summary>
    /// <returns>�V�[���̖��O�z��</returns>
    static private string[] GetSceneNames()
    {
        // �V�[���̖��O�i�[�p�̔z��
        string[] sceneNameArray = new string[EditorBuildSettings.scenes.Length];

        // �V�[���̖��O���擾�A�i�[
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            string scenePath = EditorBuildSettings.scenes[i].path;
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            sceneNameArray[i] = sceneName;
        }

        return sceneNameArray;
    }


    /// <summary>
    /// �Q�[���I��
    /// </summary>
    /// <param name="_status">�I�����</param>
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
