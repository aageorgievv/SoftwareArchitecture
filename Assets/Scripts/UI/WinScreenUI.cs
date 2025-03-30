using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        winScreen.SetActive(true);
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
