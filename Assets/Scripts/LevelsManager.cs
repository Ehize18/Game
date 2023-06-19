using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private GameObject nextLevelButton;

    public void UnlockNextLevel()
    {
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex >= PlayerPrefs.GetInt("unlockedLevels"))
            PlayerPrefs.SetInt("unlockedLevels", currentLevelIndex + 1);
    }

    public void ShowNextLevelButton()
    {
        nextLevelButton.SetActive(true);
    }

    public void LoadNextLevel()
    {
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex + 1);
    }
}
