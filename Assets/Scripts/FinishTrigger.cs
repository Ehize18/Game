using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private bool shouldItDisappear;
    [SerializeField] private bool shouldRobotDisappear;
    [SerializeField] private GameObject controller;

    private Robot robot;
    private LevelsManager levelsManager;

    private void Start()
    {
        robot = GameObject.Find("Robot").GetComponent<Robot>();
        levelsManager = controller.GetComponent<LevelsManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (robot.objectsToCollectTargetCount == -1
            || robot.CollectedObjectsCount == robot.ObjectsToCollect.Count)
        {
            levelsManager.UnlockNextLevel();
            levelsManager.ShowNextLevelButton();

            if (shouldItDisappear)
                gameObject.SetActive(false);
            if (shouldRobotDisappear)
                robot.gameObject.SetActive(false);
        }
    }
}