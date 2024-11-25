using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevel : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Exit()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
