using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired.Utils.Libraries.TinyJson;

/// <summary>
/// 現在の座標からプレイヤーの座標へと移動する
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Unit/PlayerPosition", fileName = "player_position.sequence_unit.asset", order = 2)]
public class SequenceUnitPlayerPosition : SequenceUnit
{
    #region Define

    /// <summary>
    /// プレイヤーの座標(X,Y,Z)のうち、チェックした成分のみに追従する
    /// </summary>
    [Serializable]
    private struct ReferringCoord
    {
        public bool X;
        public bool Y;
        public bool Z;
    }

    #endregion

    #region Inspector Field

    [Header("Animation Parameter")]

    [SerializeField, Tooltip("チェックした成分のみを参照する")]
    private ReferringCoord m_ReferringPlayerCoord;

    [SerializeField]
    private float m_Duration;

    [SerializeField]
    private Vector3 m_Offset;

    [SerializeField]
    private AnimationCurve m_PositionLerp;

    #endregion

    #region Field

    private Vector3 m_OnStartPosition;
    private Vector3 m_NextPosition;

    #endregion

    protected override void OnStart()
    {
        base.OnStart();
        
        m_OnStartPosition = m_SpaceType == Space.World ? Target.position : Target.localPosition;

        var playerT = BattleRealPlayerManager.Instance.Player.transform;        

        m_NextPosition = m_SpaceType == Space.World ? playerT.position : playerT.localPosition + m_Offset;

        if (!m_ReferringPlayerCoord.X)
        {
            if(m_SpaceType == Space.World)
            {
                m_NextPosition.x = Target.position.x;
            }
            else
            {
                m_NextPosition.x = Target.localPosition.x;
            }
        }
        if (!m_ReferringPlayerCoord.Y)
        {
            if (m_SpaceType == Space.World)
            {
                m_NextPosition.y = Target.position.y;
            }
            else
            {
                m_NextPosition.y = Target.localPosition.y;
            }
        }
        if (!m_ReferringPlayerCoord.Z)
        {
            if (m_SpaceType == Space.World)
            {
                m_NextPosition.z = Target.position.z;
            }
            else
            {
                m_NextPosition.z = Target.localPosition.z;
            }
        }
    }

    protected override void OnUpdate(float deltatime)
    {
        base.OnUpdate(deltatime);

        var posLerp = m_PositionLerp.Evaluate(CurrentTime);
        var pos = Vector3.Lerp(m_OnStartPosition, m_NextPosition, posLerp);

        if(m_SpaceType == Space.World)
        {
            Target.SetPositionAndRotation(pos, Target.rotation);
        }
        else
        {
            Target.localPosition = pos;
        }
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= m_Duration;
    }


}
