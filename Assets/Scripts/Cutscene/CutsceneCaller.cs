using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneCaller : MonoBehaviour
{
    [SerializeField]
    private string m_SceneName;

    private void Start()
    {
        Debug.Log("On Start");
        SceneManager.sceneLoaded += OnLoaded;
        SceneManager.LoadScene(m_SceneName, LoadSceneMode.Additive);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoaded;
    }

    private void OnLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
    }
}
