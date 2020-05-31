using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ChoiceRankingCategoryMenuIndicator : ChoiceMenuIndicator
{

    [SerializeField]
    private List<E_RANKING_CATEGORY> m_Categories;

    [SerializeField]
    private E_RANKING_CATEGORY m_DefaultCategory;

    private ReactiveProperty<E_RANKING_CATEGORY> m_Category;

    public E_RANKING_CATEGORY Category => m_Category.Value;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Category = new ReactiveProperty<E_RANKING_CATEGORY>();
    }

    protected override void SubscribeChangeValue()
    {
        m_Category.Subscribe(_ => OnChangeValue?.Invoke()).AddTo(this);
    }

    public override void ResetDefault()
    {
        m_Category.Value = m_DefaultCategory;
    }

    private int GetIndex()
    {
        return m_Categories.IndexOf(Category);
    }

    public override bool CanGoPrev()
    {
        var idx = GetIndex();
        return idx != -1 && idx > 0;
    }

    public override bool CanGoNext()
    {
        var idx = GetIndex();
        return idx != -1 && idx < m_Categories.Count - 1;
    }

    protected override void InternalGoPrev()
    {
        m_Category.Value = m_Categories[GetIndex() - 1];
    }

    protected override void InternalGoNext()
    {
        m_Category.Value = m_Categories[GetIndex() + 1];
    }

    public override string GetStringValue()
    {
        return Category.ToString().Replace("_", " ");
    }
}
