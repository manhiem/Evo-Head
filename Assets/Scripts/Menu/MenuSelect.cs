using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{
    public void Menu_Select(int menuID)
    {
        string menuName = "Menu " + menuID;
        SceneManager.LoadScene(menuID);
    }
    
    public void doExitGame()
    {
        Application.Quit();
    }
}
