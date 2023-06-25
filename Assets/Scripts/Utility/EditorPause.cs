using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorPause : MonoBehaviour
{
    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                EditorApplication.isPaused = true;
            }
        }
    }
}
