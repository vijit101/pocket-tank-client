﻿using UnityEngine;

public class MonoSingletonGeneric<T> : MonoBehaviour where T : MonoSingletonGeneric<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Destroy(this);
        }
        
    }
}
