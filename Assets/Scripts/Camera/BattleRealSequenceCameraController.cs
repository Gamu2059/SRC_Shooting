#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードのSequenceControllerを用いたカメラコントローラ。
/// </summary>
public class BattleRealSequenceCameraController : BattleRealCameraController
{
    [SerializeField]
    private SequenceController m_SequenceController;

    [SerializeField]
    private SequenceGroup m_DefaulGroup;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_SequenceController.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_SequenceController.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        // 意図的に処理を握りつぶす
        //base.OnStart();
        m_SequenceController.BuildSequence(m_DefaulGroup);
    }

    public override void OnUpdate()
    {
        // 意図的に処理を握りつぶす
        //base.OnUpdate();
        m_SequenceController.OnUpdate();
    }
}
