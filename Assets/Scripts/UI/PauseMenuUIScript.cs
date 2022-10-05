using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuUIScript : MonoBehaviour
{
    


    public void OnShowMenu()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void OnReturnToMenuClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickOptionMenu()
    {

    }

    public void OnContinueGameClick()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
