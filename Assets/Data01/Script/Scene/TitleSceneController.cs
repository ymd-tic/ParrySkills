using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------



    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------



    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnPushMainGame()
    {
        SceneManager.LoadScene("MainGameScene",LoadSceneMode.Single);
    }
}
