#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TitleManager : ControllableMonoBehavior
{
    [SerializeField]
    private PlaySoundParam m_TitleBgm;

    public override void OnStart()
    {
        base.OnStart();
        AudioManager.Instance.Play(m_TitleBgm);
    }
}
