using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the display of the game over UI, allowing the player to either restart or quit.
/// </summary>
/// <remarks>
/// - Listens for the game over event from the GameManager.
/// - Displays the game over screen when triggered.
/// - Handles restarting the game or quitting the application.
/// </remarks>


public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button quitButton;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.GetManager<GameManager>();
        gameOverPanel.SetActive(false);

        tryAgainButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);

        gameManager.OnGameOverEvent += ShowGameOverScreen; 
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnGameOverEvent -= ShowGameOverScreen; 
        }
    }

    /// <summary>
    /// Shows Game Over screen
    /// </summary>
    private void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true); 
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
