partial class BattleManager
{
    private class EndState : StateCycle
    {
        public override void OnStart()
        {
            base.OnStart();
            Target.m_RealManager.RequestChangeState(E_BATTLE_REAL_STATE.END);
            Target.m_HackingManager.RequestChangeState(E_BATTLE_HACKING_STATE.END);
            
            // とりあえず呼び出しているが、クリアしたかどうかとかは判定していないので、それは今後やる
            DataManager.Instance.OnChapterEnd(Target.m_RealManager.IsChapterClear);
            // 何をもってしてストーリークリアかはまだ未定義なのでコメントアウト
            // DataManager.Instance.OnStoryEnd();

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
                    Target.ExitGame();
                }
            }
            else
            {
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
