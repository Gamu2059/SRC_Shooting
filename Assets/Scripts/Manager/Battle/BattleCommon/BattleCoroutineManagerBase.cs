using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCoroutineManagerBase<T> : BattleSingletonMonoBehavior<T> where T : BattleCoroutineManagerBase<T>
{
    protected CoroutineController m_CoroutineController;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_CoroutineController = new CoroutineController();
    }

    public override void OnFinalize()
    {
        m_CoroutineController = null;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        m_CoroutineController.OnUpdate();
    }

    /// <summary>
    /// コルーチンを登録する。
    /// </summary>
    public void RegistCoroutine(IEnumerator coroutine)
    {
        m_CoroutineController.RegistCoroutine(coroutine);
    }

    /// <summary>
    /// コルーチンを削除する。
    /// </summary>
    public void RemoveCoroutine(IEnumerator coroutine)
    {
        m_CoroutineController.RemoveCoroutine(coroutine);
    }
}
