using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static void LoadAnyScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
