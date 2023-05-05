using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public static void LoadALevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
