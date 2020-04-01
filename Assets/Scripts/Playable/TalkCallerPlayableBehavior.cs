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

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (AdvEngineManager.Instance == null)
        {
            Debug.LogWarningFormat("{0} : AdvEngineManager is null. Talk is invalid!", GetType().Name);
            return;
        }

        m_TalkCaller = new TalkCaller(
            ScenarioLabel,
            () =>
            {
                m_CutsceneController = GetCutsceneController();
                if (m_CutsceneController == null)
                {
                    Debug.LogWarningFormat("{0} : CutsceneController was not found.", GetType().Name);
                    return;
                }
                m_CutsceneController?.Pause();
            },
            () =>
            {
                m_CutsceneController?.Resume();
            },
            () =>
            {
                Debug.LogWarningFormat("{0} : Talk Caller start failed.", GetType().Name);
            }
            );
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        m_TalkCaller?.StopTalk();
        m_TalkCaller = null;
        base.OnPlayableDestroy(playable);
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
}
