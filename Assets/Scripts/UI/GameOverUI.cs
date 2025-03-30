using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true); 
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
