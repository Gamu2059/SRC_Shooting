using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ChoiceNumMenuIndicator : ChoiceMenuIndicator
{
    [SerializeField]
    private int m_MinNum;

    [SerializeField]
    private int m_MaxNum;

    [SerializeField]
    private int m_DefaultNum;

    private ReactiveProperty<int> m_Num = new ReactiveProperty<int>();

    public int Num => m_Num.Value;

    protected override void SubscribeChangeValue()
    {
        m_Num.Subscribe(_ => OnChangeValue?.Invoke()).AddTo(this);
    }

    public override void ResetDefault()
    {
        m_Num.Value = Mathf.Clamp(m_DefaultNum, m_MinNum, m_MaxNum);
        OnChangeValue?.Invoke();
    }

    public override bool CanGoPrev()
    {
        return Num > m_MinNum;
    }

    public override bool CanGoNext()
    {
        return Num < m_MaxNum;
    }

    protected override void InternalGoPrev()
    {
        m_Num.Value--;
    }

    protected override void InternalGoNext()
    {
        m_Num.Value++;
    }

    public override string GetStringValue()
    {
        return Num.ToString();
    }

    public void SetNum(int num)
    {
        m_Num.Value = num;
    }
}
