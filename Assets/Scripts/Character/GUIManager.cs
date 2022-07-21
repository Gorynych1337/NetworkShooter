using UnityEngine;
using Photon.Pun;

public enum UIs
{
    InGame = 0,
    EscMenu,
    TabMenu,
    WinnerScreen,
}

public class GUIManager : MonoBehaviour
{
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject escUI;
    [SerializeField] GameObject tabUI;
    [SerializeField] GameObject winnerUI;

    public void ShowUI(UIs uiToShow)
    {
        inGameUI.SetActive(false);
        escUI.SetActive(false);
        tabUI.SetActive(false);
        winnerUI.SetActive(false);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        
        switch (uiToShow)
        {
            case UIs.InGame:
                inGameUI.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            
            case UIs.TabMenu:
                tabUI.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            
            case UIs.EscMenu:
                escUI.SetActive(true);
                break;
            
            case UIs.WinnerScreen:
                winnerUI.SetActive(true);
                break;
        }
    }
}
