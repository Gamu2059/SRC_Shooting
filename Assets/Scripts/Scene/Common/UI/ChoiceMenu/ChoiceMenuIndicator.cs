using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UniRx;

/// <summary>
/// ChoiceMenuの表示する値を制御する
/// </summary>
public abstract class ChoiceMenuIndicator : MonoBehaviour
{
    public Action OnChangeValue;

    protected void Awake()
    {
        OnAwake();
        ResetDefault();
    }

    protected virtual void OnAwake() { }

    protected abstract void SubscribeChangeValue();

    public abstract void ResetDefault();

    public abstract bool CanGoPrev();

    public abstract bool CanGoNext();

    public void GoPrev()
    {
        if (!CanGoPrev())
        {
            return;
        }

        InternalGoPrev();
    }

    public void GoNext()
    {
        if (!CanGoNext())
        {
            return;
        }

        InternalGoNext();
    }

    protected abstract void InternalGoPrev();

    protected abstract void InternalGoNext();

    public abstract string GetStringValue();
}
