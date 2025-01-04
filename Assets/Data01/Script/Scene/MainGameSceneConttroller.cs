using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneConttroller : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [SerializeField] GameObject clearUI;
    [SerializeField] GameObject overUI;


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------
    static private GameObject gameClear;
    static private GameObject gameOver;


    //-----protectedField------------------------------------------------------------



    void Start()
    {
        gameClear = clearUI;
        gameOver = overUI;

        Cursor.lockState = CursorLockMode.Locked;
        gameClear.SetActive(false);
        gameOver.SetActive(false);
    }


    void Update()
    {
        
    }

    public void OnPushTitle()
    {
        Time.timeScale = 1;
        AreaManager.enemyList.Clear();
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }

    static public void GameFinish(string gameFinish)
    {
        if(gameFinish == "Clear") gameClear.SetActive(true);
        if(gameFinish == "Over")  gameOver.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }
}
