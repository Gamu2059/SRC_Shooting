using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneCaller : MonoBehaviour
{
    [SerializeField]
    private string m_SceneName;

    private Scene m_CalledCutscene;

    private void Start()
    {
        Debug.Log("On Start");
        SceneManager.sceneLoaded += OnLoaded;
        SceneManager.LoadScene(m_SceneName, LoadSceneMode.Additive);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoaded;
        SceneManager.sceneUnloaded -= OnUnloaded;
    }

    private void OnLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        m_CalledCutscene = scene;
        SceneManager.sceneLoaded -= OnLoaded;
        SceneManager.SetActiveScene(m_CalledCutscene);
        CutsceneManager.Instance.OnCompleteCutscene += OnCompleteCutscene;
        CutsceneManager.Instance.Play();
    }

    private void OnCompleteCutscene()
    {
        SceneManager.sceneUnloaded += OnUnloaded;
        SceneManager.UnloadSceneAsync(m_CalledCutscene);
    }

    private void OnUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnUnloaded;
        FadeManager.Instance.FadeIn(0.5f);
    }
}
