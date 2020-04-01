using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Timeline上で、FadeManagerに処理を渡すための動作。
/// </summary>
public class FadePlayableBehaviour : PlayableBehaviour
{
    public FadeParam FadeParam;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (FadeManager.Instance == null)
        {
            Debug.LogWarningFormat("{0} : FadeManager is null. Fade is invalid!", GetType().Name);
            return;
        }

        FadeManager.Instance.Fade(FadeParam);
    }
}
