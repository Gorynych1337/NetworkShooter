using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillchatItem : MonoBehaviour
{
    public TMP_Text killer;
    public TMP_Text action;
    public TMP_Text target;
    public void Initialize(string target_)
    {
        killer.text = target_;
        action.text = "killed by the void";
        target.text = "";
    }
    
    public void Initialize(string killer_, string target_)
    {
        killer.text = killer_;
        action.text = "killed";
        target.text = target_;
    }
}
