using UnityEngine;

partial class BattleHackingManager
{
    private class ToRealState : StateCycle
    {
        public override void OnEnd()
        {
            base.OnEnd();
            BattleHackingPlayerManager.Instance.OnPutAway();
            BattleHackingEnemyManager.Instance.OnPutAway();
            BattleHackingBulletManager.Instance.OnPutAway();
            BattleHackingEffectManager.Instance.OnPutAway();

            BattleHackingUiManager.Instance.GridHoleEffect.StopEffect();
        }
    }
}
