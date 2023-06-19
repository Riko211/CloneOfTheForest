using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public IEnumerator LoadScene(string name, Action onLoaded = null)
    {
        AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);

        while (!waitNextScene.isDone) yield return null;

        onLoaded?.Invoke();
    }
    
}
