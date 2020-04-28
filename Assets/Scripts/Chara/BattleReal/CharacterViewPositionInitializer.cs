#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// キャラクタの見た目の位置を初期化するコンポーネント。
/// </summary>
public class CharacterViewPositionInitializer : ControllableMonoBehavior, IAutoControlOnCharaController
{
    #region Field Inspector

    [SerializeField]
    private Transform m_Target;

    [SerializeField]
    private Vector3 m_InitializePosition;

    [SerializeField]
    private float m_Lerp;

    #endregion

    public bool IsEnableController { get; set; }

    public override void OnStart()
    {
        base.OnStart();
        IsEnableController = true;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (m_Target == null || !IsEnableController)
        {
            return;
        }

        m_Target.localPosition = Vector3.Lerp(m_Target.localPosition, m_InitializePosition, m_Lerp);
    }
}
