using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{
    public void SetWinner(string _nickname, int _score)
    {
        GetComponent<Text>().text = String.Format("{0} is winner with score {1}!", _nickname, _score);
    }
}
