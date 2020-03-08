partial class BattleRealManager
{
    private class StartState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.InputManager.OnStart();
            Target.RealTimerManager.OnStart();

            // このタイミングでBattle Loadedがカウント開始する
            Target.EventManager.OnStart();

            Target.CameraManager.RegisterCamera(Target.m_BattleManager.BattleRealBackCamera, E_CAMERA_TYPE.BACK_CAMERA);
            Target.CameraManager.RegisterCamera(Target.m_BattleManager.BattleRealFrontCamera, E_CAMERA_TYPE.FRONT_CAMERA);

            Target.PlayerManager.OnStart();
            Target.EnemyGroupManager.OnStart();
            Target.EnemyManager.OnStart();
            Target.BulletManager.OnStart();
            Target.ItemManager.OnStart();
            Target.EffectManager.OnStart();
            Target.CollisionManager.OnStart();
            Target.CameraManager.OnStart();

            Target.RequestChangeState(E_BATTLE_REAL_STATE.GAME);
        }
    }
}
