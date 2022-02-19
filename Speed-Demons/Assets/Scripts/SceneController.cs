using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime;

public class SceneController : MonoBehaviour
{
    private static string [] Levels = {"MainMenu", "Level1", "GameOver", "GameWin"};
    private static int LevelIndex = 0;
    public void MainMenu()
    {
        LevelIndex = 0;
        SceneManager.LoadScene(Levels[LevelIndex]);
    }
    public void Play()
    {
        LevelIndex = 1;
        SceneManager.LoadScene(Levels[LevelIndex]);
    }
    public static void Loss()
    {
        LevelIndex = 2;
        SceneManager.LoadScene(Levels[LevelIndex]);
    }
    public static void Win()
    {
        LevelIndex = 3;
        SceneManager.LoadScene(Levels[LevelIndex]);
    }
}
