using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuUIScript : MonoBehaviour
{
    


    public void OnQuitGame()
    {
        Application.Quit(); 
    }

    public void OnStartGameClicked()
    {
        // SceneManager.LoadScene();
    }

    public void OnShowOptionsClicked()
    {

    }
}
