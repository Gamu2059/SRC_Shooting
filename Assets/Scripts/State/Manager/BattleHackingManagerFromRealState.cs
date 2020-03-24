using UnityEngine;

partial class BattleHackingManager
{
    private class FromRealState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            var levelParamSet = HackingDataHolder.HackingLevelParamSet;
            if (levelParamSet == null)
            {
                Debug.LogError("Hacking Level Param Set is null!");
                Target.RequestChangeState(E_BATTLE_HACKING_STATE.GAME_OVER);
                return;
            }

            BattleHackingStageManager.Instance.gameObject.SetActive(true);
            BattleHackingUiManager.Instance.PlayToHacking();

            Target.m_IsDeadPlayer = false;
            Target.m_IsDeadBoss = false;
            Target.m_IsTimeout = false;

            BattleHackingPlayerManager.Instance.OnPrepare(levelParamSet);
            BattleHackingEnemyManager.Instance.OnPrepare(levelParamSet);
        }
    }
}
