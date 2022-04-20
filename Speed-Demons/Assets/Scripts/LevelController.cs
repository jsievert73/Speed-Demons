using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update

    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
