using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// カットシーンの呼び出しを制御する。
/// 1カットシーンにつき、1インスタンスでの使用を想定。
/// </summary>
public class CutsceneCaller
{
    private Action<CutsceneController> m_OnLoaded;
    private Action m_OnUnloaded;
    private Action m_OnLoadError;

    private string m_CutsceneName;
    private Scene m_CalledCutscene;
    private CutsceneController m_CutsceneController;

    /// <summary>
    /// カットシーンを呼び出す。
    /// </summary>
    /// <param name="cutsceneName">カットシーンの名前</param>
    /// <param name="onLoaded">カットシーンの読み込みに成功した時のコールバック</param>
    /// <param name="onUnloaded">カットシーンの破棄に成功した時のコールバック</param>
    /// <param name="onLoadError">カットシーンの読み込みに失敗した時のコールバック</param>
    public CutsceneCaller(string cutsceneName, Action<CutsceneController> onLoaded, Action onUnloaded, Action onLoadError = null)
    {
        m_CutsceneName = cutsceneName;
        m_OnLoaded = onLoaded;
        m_OnUnloaded = onUnloaded;
        m_OnLoadError = onLoadError;

        SceneManager.sceneLoaded += OnLoaded;
        SceneManager.LoadScene(m_CutsceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// 強制的にカットシーンを止めて削除する。
    /// 主にFinalize用の処理。
    /// </summary>
    public void StopCutscene()
    {
        m_CutsceneController.Stop();
        SceneManager.sceneLoaded -= OnLoaded;
        SceneManager.sceneUnloaded -= OnUnloaded;
        SceneManager.UnloadSceneAsync(m_CalledCutscene);
    }

    private void OnLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        var name = scene.name;
        if (name != null && (name.Contains(m_CutsceneName) || name == m_CutsceneName))
        {
            m_CalledCutscene = scene;
            SceneManager.sceneLoaded -= OnLoaded;
            SceneManager.SetActiveScene(m_CalledCutscene);

            m_CutsceneController = null;
            var objects = m_CalledCutscene.GetRootGameObjects();
            if (objects == null)
            {
                m_OnLoadError?.Invoke();
                return;
            }

            foreach (var obj in m_CalledCutscene.GetRootGameObjects())
            {
                var controller = obj.GetComponent<CutsceneController>();
                if (controller != null)
                {
                    m_CutsceneController = controller;
                    break;
                }
            }

            if (m_CutsceneController == null)
            {
                m_OnLoadError?.Invoke();
                return;
            }

            m_CutsceneController.OnCompleteCutscene += OnCompleteCutscene;
            m_CutsceneController.Play();

            m_OnLoaded?.Invoke(m_CutsceneController);
        }
    }

    private void OnCompleteCutscene()
    {
        SceneManager.sceneUnloaded += OnUnloaded;
        SceneManager.UnloadSceneAsync(m_CalledCutscene);
    }

    private void OnUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnUnloaded;
        m_OnUnloaded?.Invoke();
    }
}
