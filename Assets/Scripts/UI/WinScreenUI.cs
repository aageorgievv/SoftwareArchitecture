using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the win screen UI, including its visibility and button functionality.
/// </summary>
/// <remarks>
/// - Initially hides the win screen on start.
/// - Toggles the win screen's visibility when `ShowWinScreen()` is called.
/// - Handles button clicks for restarting the current scene and quitting the game.
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

    /// <summary>
    /// Shows the Win UI screen
    /// </summary>
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

    /// <summary>
    /// Restarts the scene
    /// </summary>
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    private void QuitGame()
    {
        Application.Quit();
    }
}
