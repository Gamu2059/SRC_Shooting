using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

/// <summary>
/// Timeline上で、AdvEndineManagerに処理を渡すための処理。<br/>
/// カットシーン専用。
/// </summary>
public class TalkCallerPlayableBehavior : PlayableBehaviour
{
    public string ScenarioLabel;
    public GameObject PlayableDirectorObject;

    private TalkCaller m_TalkCaller;
    private CutsceneController m_CutsceneController;
    private bool m_ShouldPause;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);

        if (AdvEngineManager.Instance == null)
        {
            Debug.LogWarningFormat("{0} : AdvEngineManager is null. Talk is invalid!", GetType().Name);
            return;
        }

        m_CutsceneController = GetCutsceneController();
        if (m_CutsceneController == null)
        {
            Debug.LogWarningFormat("{0} : CutsceneController was not found.", GetType().Name);
            return;
        }

        m_ShouldPause = true;
        m_TalkCaller = new TalkCaller(ScenarioLabel, null, Resume, OnStartErrorTalkCaller);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);
        
        if (!m_ShouldPause)
        {
            return;
        }

        var deltaTime = m_CutsceneController.GetDeltaTime();
        if (playable.GetTime() + deltaTime >= playable.GetDuration())
        {
            Pause();
        }
    }

    private CutsceneController GetCutsceneController()
    {
        if (PlayableDirectorObject == null)
        {
            return null;
        }

        var scene = PlayableDirectorObject.scene;
        foreach (var obj in scene.GetRootGameObjects())
        {
            var controller = obj.GetComponent<CutsceneController>();
            if (controller != null)
            {
                return controller;
            }
        }

        return null;
    }

    private void Pause()
    {
        m_ShouldPause = false;
        m_CutsceneController?.Pause();
    }

    private void Resume()
    {
        m_ShouldPause = false;
        m_CutsceneController?.Resume();
        m_TalkCaller?.StopTalk();
        m_TalkCaller = null;
    }

    private void OnStartErrorTalkCaller()
    {
        Debug.LogWarningFormat("{0} : Talk Caller start failed.", GetType().Name);
    }
}
