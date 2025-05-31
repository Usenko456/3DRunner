using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject startMenuUI;
    public GameObject collapseMenuUI;
    private void Start()
    {
        startMenuUI.SetActive(true);
        Time.timeScale = 0f;
        collapseMenuUI.SetActive(false);
    }
    public static void LoadCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quitgame()
    {
        Application.Quit();
    }
    public void StartLevel()
    {
        startMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
