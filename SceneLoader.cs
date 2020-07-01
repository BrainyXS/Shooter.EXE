using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadTheScene()
    {
        SceneManager.LoadScene(sceneName);
    }

}
