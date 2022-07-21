using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public void SetTime(float timeRemaining)
    {
        GetComponent<Text>().text = string.Format("{0}:{1}", Mathf.FloorToInt(timeRemaining / 60), Mathf.FloorToInt(timeRemaining % 60));
    }
}
