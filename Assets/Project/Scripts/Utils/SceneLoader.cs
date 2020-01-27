using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int index)
    {
        Utils.LoadAnyScene(index);
    }
}
