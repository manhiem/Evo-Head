using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Selection : MonoBehaviour
{
    public void LevelSelect(int levelId)
    {
        Debug.Log("BUTTON WORKS");
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelId);
    }
}
