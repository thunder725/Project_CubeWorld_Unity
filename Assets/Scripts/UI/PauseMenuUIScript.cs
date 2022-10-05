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

    }

    public void OnContinueGameClick()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
