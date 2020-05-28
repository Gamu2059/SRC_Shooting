
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 単一のアニメーションで動作するオブジェクトを制御する
/// </summary>
public class SimpleAnimatorController : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private string m_PlayAnimationName;

    [SerializeField]
    private bool m_ControlUpdate;

    #endregion

    #region Field

    public Subject<Unit> OnComplete { get; private set; }
    private IDisposable m_Disposable;

    #endregion

    public override void OnInitialize()
    {
        base.OnInitialize();
        OnComplete = new Subject<Unit>();

        if (m_ControlUpdate)
        {
            // Updateを自分で回すためにdisableにする
            m_Animator.enabled = false;
        }
    }

    public override void OnFinalize()
    {
        OnComplete?.Dispose();
        OnComplete = null;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_ControlUpdate && m_Animator.gameObject.activeSelf)
        {
            m_Animator.Update(Time.deltaTime);
        }
    }

    public void Play()
    {
        m_Animator.gameObject.SetActive(true);
        m_Animator.Play(m_PlayAnimationName);
        m_Disposable = m_Animator
            .ObserveEveryValueChanged(x => x.GetCurrentAnimatorStateInfo(0).normalizedTime)
            .Where(x => x >= 1)
            .Subscribe(x =>
            {
                m_Disposable?.Dispose();
                m_Animator.gameObject.SetActive(false);
                OnComplete.OnNext(Unit.Default);
                OnComplete.OnCompleted();
            });
    }
}
