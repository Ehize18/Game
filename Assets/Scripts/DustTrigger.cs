using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustTrigger : MonoBehaviour
{
    private Robot robot;

    private void Start()
    {
        robot = GameObject.Find("Robot").GetComponent<Robot>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        robot.CollectedObjectsCount++;
        gameObject.SetActive(false);
    }
}
