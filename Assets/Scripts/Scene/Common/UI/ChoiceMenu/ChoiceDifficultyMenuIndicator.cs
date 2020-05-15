using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ChoiceDifficultyMenuIndicator : ChoiceMenuIndicator
{
    [SerializeField]
    private List<E_DIFFICULTY> m_Difficulties;

    [SerializeField]
    private E_DIFFICULTY m_DefaultDifficulty;

    private ReactiveProperty<E_DIFFICULTY> m_Difficulty;

    public E_DIFFICULTY Difficulty => m_Difficulty.Value;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Difficulty = new ReactiveProperty<E_DIFFICULTY>();
    }

    protected override void SubscribeChangeValue()
    {
        m_Difficulty.Subscribe(_ => OnChangeValue?.Invoke()).AddTo(this);
    }

    public override void ResetDefault()
    {
        m_Difficulty.Value = m_DefaultDifficulty;
    }

    private int GetIndex()
    {
        return m_Difficulties.IndexOf(Difficulty);
    }

    public override bool CanGoPrev()
    {
        var idx = GetIndex();
        return idx != -1 && idx > 0;
    }

    public override bool CanGoNext()
    {
        var idx = GetIndex();
        return idx != -1 && idx < m_Difficulties.Count - 1;
    }

    protected override void InternalGoPrev()
    {
        m_Difficulty.Value = m_Difficulties[GetIndex() - 1];
    }

    protected override void InternalGoNext()
    {
        m_Difficulty.Value = m_Difficulties[GetIndex() + 1];
    }

    public override string GetStringValue()
    {
        return Difficulty.ToString().Replace("_", " ");
    }
}
