using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ChoiceChapterMenuIndicator : ChoiceMenuIndicator
{
    [SerializeField]
    private List<E_CHAPTER> m_Chapters;

    [SerializeField]
    private E_CHAPTER m_DefaultChapter;

    private ReactiveProperty<E_CHAPTER> m_Chapter = new ReactiveProperty<E_CHAPTER>();

    public E_CHAPTER Chapter => m_Chapter.Value;

    protected override void SubscribeChangeValue()
    {
        m_Chapter.Subscribe(_ => OnChangeValue?.Invoke()).AddTo(this);
    }

    public override void ResetDefault()
    {
        m_Chapter.Value = m_DefaultChapter;
    }

    private int GetIndex()
    {
        return m_Chapters.IndexOf(Chapter);
    }

    public override bool CanGoPrev()
    {
        var idx = GetIndex();
        return idx != -1 && idx > 0;
    }

    public override bool CanGoNext()
    {
        var idx = GetIndex();
        return idx != -1 && idx < m_Chapters.Count - 1;
    }

    protected override void InternalGoPrev()
    {
        m_Chapter.Value = m_Chapters[GetIndex() - 1];
    }

    protected override void InternalGoNext()
    {
        m_Chapter.Value = m_Chapters[GetIndex() + 1];
    }

    public override string GetStringValue()
    {
        return Chapter.ToString().Replace("_", " ");
    }
}
