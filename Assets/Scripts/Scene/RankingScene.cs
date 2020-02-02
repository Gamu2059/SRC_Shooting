using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingScene : BaseScene
{
    public override void OnBeforeShow(Action onComplete)
    {
        OnInitializeManagers();
        base.OnBeforeShow(onComplete);
    }

    public override void OnAfterHide(Action onComplete)
    {
        OnFinalizeManagers();
        base.OnAfterHide(onComplete);
    }
}
