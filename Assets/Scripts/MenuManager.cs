using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private List<Menu> _menus;

    void Awake()
    {
        instance = this;
    }
    
    public void OpenMenu(string menuName)
    {
        foreach (Menu menu in _menus)
        {
            //Debug.Log(menu.menuName);
            if (menu.menuName == menuName)
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }
    public void CloseMenu(string menuName)
    {
        foreach (Menu menu in _menus)
        {
            Debug.Log(menu.menuName);
            if (menu.menuName == menuName)
            {
                menu.Close();
            }
           
        }
    }

    

    
}
