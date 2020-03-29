using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class BattleHackingManager
{
    private class StayRealState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            BattleHackingStageManager.Instance.gameObject.SetActive(false);
            BattleHackingUiManager.Instance.SetAlpha(0);
        }
    }
}
