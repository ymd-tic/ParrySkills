using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public class SceneData // �V�[�����f�[�^
    {
        public const string TITLE = "Title";
        public const string SELECT = "StageSelect";
        public const string STAGE01 = "Stage01";
        public const string STAGE02 = "Stage02";
    }
    public enum GameEndStatus // �Q�[���I�����̏��
    {
        CLEAR,
        OVER
    }


    //-----SerializeField------------------------------------------------------------
    [Tooltip("�Q�[���I��")]
    [SerializeField] GameObject gameClearObj;   // �N���A
    [SerializeField] GameObject gameOverObj;    // �I�[�o�[


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------
    static private GameObject clearObj;
    static private GameObject overObj;


    //-----protectedField------------------------------------------------------------


    private void Awake()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case SceneData.TITLE:
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case SceneData.SELECT:
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case SceneData.STAGE01:
            case SceneData.STAGE02:
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
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(_name,LoadSceneMode.Single);

        //foreach (var _scene in sceneData))
        //{
        //    if (_name == _scene)
        //    {
        //        Time.timeScale = 1;
        //        SceneManager.LoadSceneAsync(_name, LoadSceneMode.Single);
        //        return;
        //    }
        //}

        //Debug.LogError("�V�[�������݂��܂���");
    }
    /// <summary>
    /// �V�[���̖��O���擾
    /// </summary>
    /// <returns>�V�[���̖��O�z��</returns>
    //static private string[] GetSceneNames()
    //{
    //    // �V�[���̖��O�i�[�p�̔z��
    //    string[] sceneNameArray = new string[EditorBuildSettings.scenes.Length];

    //    // �V�[���̖��O���擾�A�i�[
    //    for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
    //    {
    //        string scenePath = EditorBuildSettings.scenes[i].path;
    //        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

    //        sceneNameArray[i] = sceneName;
    //    }

    //    return sceneNameArray;
    //}


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
