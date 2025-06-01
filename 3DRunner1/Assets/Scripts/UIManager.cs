using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject startMenuUI;
    public GameObject collapseMenuUI;
    private static bool isRestarted = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        collapseMenuUI.SetActive(false);
        //if it retry, deactivate the start menu and start game
        if (isRestarted)
        {
            startMenuUI.SetActive(false);   
            Time.timeScale = 1f;            
            isRestarted = false;            
            return;
        }
        else
        {
            startMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    //Method for retry button
     public static void Retry()
    {
        isRestarted = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //Method for quit application(on IOS probably won't work)
    public void Quitgame()
    {
        Application.Quit();
    }
    //Method for start button
    public void StartLevel()
    {
        startMenuUI.SetActive(false);
        Time.timeScale = 1f;

    }
    //Obstacle collision method
    public void ShowCollapseMenu()
    {
        collapseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
