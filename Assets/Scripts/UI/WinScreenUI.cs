using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the win screen UI and its interactions.
/// </summary>
/// <remarks>
/// - Controls visibility of the win screen.
/// - Provides buttons for restarting the game and quitting.
/// - Listens for button clicks to reload the scene or exit the application.
/// </remarks>


public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        winScreen.SetActive(false);
        playAgainButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ShowWinScreen()
    {
        if(winScreen.activeInHierarchy)
        {
            winScreen.SetActive(false);
        } else
        {
            winScreen.SetActive(true);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
