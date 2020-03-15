using UnityEngine;

partial class BattleHackingManager
{
    private class ToRealState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            var player = BattleHackingPlayerManager.Instance.Player;
            var centerPos = BattleHackingStageManager.Instance.CalcViewportPosFromWorldPosition(player.transform, false);

            // StageManagerは原点が中央にあるため、原点をずらす
            centerPos += Vector2.one * 0.5f;
            BattleHackingUiManager.Instance.GridHoleEffect.PlayEffect(centerPos);
            BattleHackingUiManager.Instance.PlayToReal();
        }

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
