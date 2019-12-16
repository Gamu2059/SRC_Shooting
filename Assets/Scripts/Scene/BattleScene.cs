using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleScene : BaseScene
{
    public override void OnBeforeShow( Action onComplete )
	{
		OnInitializeManagers();
		base.OnBeforeShow( onComplete );
	}

    public override void OnAfterShow(Action onComplete)
    {
        base.OnAfterShow(onComplete);
    }

    public override void OnBeforeHide(Action onComplete)
    {
        base.OnBeforeHide(onComplete);

        // 念のために他のシーンに移動する前に時間を戻しておく
        Time.timeScale = 1;
    }

    public override void OnAfterHide( Action onComplete )
	{
		OnFinalizeManagers();
		base.OnAfterHide( onComplete );
	}
}
