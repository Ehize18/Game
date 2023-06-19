using UnityEngine;
using UnityEngine.UI;

public class LevelButtonsManager : MonoBehaviour
{
    private int lastUnlockedLevelIndex;
    public Button[] Buttons;

    public void Start()
    {
        lastUnlockedLevelIndex = PlayerPrefs.GetInt("unlockedLevels", 1);
        for (var i = lastUnlockedLevelIndex; i < Buttons.Length; i++)
            Buttons[i].interactable = false;
    }

    public void ResetLevels()
    {
        PlayerPrefs.DeleteKey("unlockedLevels");
        Start();
    }
}
