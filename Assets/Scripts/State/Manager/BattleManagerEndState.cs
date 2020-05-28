partial class BattleManager
{
    private class EndState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_RealManager.RequestChangeState(E_BATTLE_REAL_STATE.END);
            Target.m_HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.END);
            
            if (DataManager.Instance.IsDirectTransitionNextChapter())
            {
                if (DataManager.Instance.ExistNextChapter())
                {
                    var nextChapter = DataManager.Instance.GetNextChapter();
                    DataManager.Instance.Chapter = nextChapter;
                    DataManager.Instance.TransitionToCurrentChapterScene();
                }
                else
                {
                    DataManager.Instance.OnShootingEnd();
                    Target.ExitGame();
                }
            }
            else
            {
                DataManager.Instance.OnShootingEnd();
                Target.ExitGame();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.m_RealManager.OnUpdate();
            Target.m_HackingManager.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            Target.m_RealManager.OnLateUpdate();
            Target.m_HackingManager.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Target.m_RealManager.OnFixedUpdate();
            Target.m_HackingManager.OnFixedUpdate();
        }
    }
}
