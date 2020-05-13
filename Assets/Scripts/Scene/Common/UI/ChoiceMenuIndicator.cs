using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UniRx;

/// <summary>
/// ChoiceMenuの表示する値を制御する
/// </summary>
public class ChoiceMenuIndicator : MonoBehaviour
{
    #region Define

    [Serializable]
    public enum E_TYPE
    {
        NUM,
        DIFFICULTY,
        CHAPTER,
    }

    #endregion

    #region Field Inspector

    [SerializeField]
    private E_TYPE m_Type;
    public E_TYPE Type => m_Type;

    [SerializeField]
    private int m_MinNum;

    [SerializeField]
    private int m_MaxNum;

    [SerializeField]
    private int m_DefaultNum;

    [SerializeField]
    private List<E_DIFFICULTY> m_Difficulties;

    [SerializeField]
    private E_DIFFICULTY m_DefaultDifficulty;

    [SerializeField]
    private List<E_CHAPTER> m_Chapters;

    [SerializeField]
    private E_CHAPTER m_DefaultChapter;

    #endregion

    #region Field

    public int NumValue { get; private set; }
    public E_DIFFICULTY DifficultyValue { get; private set; }
    public E_CHAPTER ChapterValue { get; private set; }

    public Action OnChangeValue;

    #endregion

    private void Awake()
    {
        ResetDefault();
    }

    public void ResetDefault()
    {
        switch (m_Type)
        {
            case E_TYPE.NUM:
                NumValue = Mathf.Clamp(m_DefaultNum, m_MinNum, m_MaxNum);
                break;
            case E_TYPE.DIFFICULTY:
                DifficultyValue = m_DefaultDifficulty;
                break;
            case E_TYPE.CHAPTER:
                ChapterValue = m_DefaultChapter;
                break;
        }

        OnChangeValue?.Invoke();
    }

    public bool CanGoPrev()
    {
        switch (m_Type)
        {
            case E_TYPE.NUM:
                return NumValue > m_MinNum;
            case E_TYPE.DIFFICULTY:
                var diffIdx = m_Difficulties.IndexOf(DifficultyValue);
                return diffIdx != -1 && diffIdx > 0;
            case E_TYPE.CHAPTER:
                var chapIdx = m_Chapters.IndexOf(ChapterValue);
                return chapIdx != -1 && chapIdx > 0;
        }

        return false;
    }

    public bool CanGoNext()
    {
        switch (m_Type)
        {
            case E_TYPE.NUM:
                return NumValue < m_MaxNum;
            case E_TYPE.DIFFICULTY:
                var diffIdx = m_Difficulties.IndexOf(DifficultyValue);
                return diffIdx != -1 && diffIdx < m_Difficulties.Count - 1;
            case E_TYPE.CHAPTER:
                var chapIdx = m_Chapters.IndexOf(ChapterValue);
                return chapIdx != -1 && chapIdx < m_Chapters.Count - 1;
        }

        return false;
    }

    public void GoPrev()
    {
        if (!CanGoPrev())
        {
            return;
        }

        switch (m_Type)
        {
            case E_TYPE.NUM:
                NumValue--;
                break;
            case E_TYPE.DIFFICULTY:
                DifficultyValue = m_Difficulties[m_Difficulties.IndexOf(DifficultyValue) - 1];
                break;
            case E_TYPE.CHAPTER:
                ChapterValue = m_Chapters[m_Chapters.IndexOf(ChapterValue) - 1];
                break;
        }

        OnChangeValue?.Invoke();
    }

    public void GoNext()
    {

        switch (m_Type)
        {
            case E_TYPE.NUM:
                NumValue++;
                break;
            case E_TYPE.DIFFICULTY:
                DifficultyValue = m_Difficulties[m_Difficulties.IndexOf(DifficultyValue) + 1];
                break;
            case E_TYPE.CHAPTER:
                ChapterValue = m_Chapters[m_Chapters.IndexOf(ChapterValue) + 1];
                break;
        }

        OnChangeValue?.Invoke();
    }
}
