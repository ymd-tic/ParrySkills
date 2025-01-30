using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public class SceneData // シーン名データ
    {
        public const string TITLE = "Title";
        public const string SELECT = "StageSelect";
        public const string STAGE01 = "Stage01";
        public const string STAGE02 = "Stage02";
    }
    public enum GameEndStatus // ステージ終了時の状態
    {
        CLEAR,
        OVER
    }


    //-----SerializeField------------------------------------------------------------
    [Tooltip("ゲーム終了")]
    [SerializeField] GameObject gameClearObj;   // クリア
    [SerializeField] GameObject gameOverObj;    // オーバー


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------


    //-----protectedField------------------------------------------------------------


    private void Awake()
    {
        // 現在のシーン名によってカーソルの状態を変更
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
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
                break;
        }
    }

    /// <summary>
    /// シーンのロード
    /// </summary>
    /// <param name="_name">ロードシーン名</param>
    public void OnLoadScene(string _name)
    {

        // ゲームプレイ終了
        if(_name == "Finish")
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();//ゲームプレイ終了
#endif
        }

        // 遷移先がステージの場合敵を格納しているリストをクリア
        switch (_name)
        {
            case MySceneManager.SceneData.STAGE01:
            case MySceneManager.SceneData.STAGE02:
            AreaManager.enemyList.Clear();
                break;
        }

        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_name, LoadSceneMode.Single);
    }


    /// <summary>
    /// ステージ終了
    /// </summary>
    /// <param name="_status">終了状態</param>
    public void GameFinish(GameEndStatus _status)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        AreaManager.enemyList.Clear();

        switch (_status)
        {
            case GameEndStatus.CLEAR:
                gameClearObj.SetActive(true);
                break;
            case GameEndStatus.OVER:
                gameOverObj.SetActive(true);
                break;
        }
    }
}
