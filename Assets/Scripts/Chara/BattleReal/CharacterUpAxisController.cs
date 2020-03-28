using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクタのUP方向の角度を制御する。
/// </summary>
public class CharacterUpAxisController : ControllableMonoBehavior, IAutoControlOnCharaController
{
    [SerializeField]
    private Transform m_CharacterView;

    [SerializeField]
    private float m_OffsetAngle;

    [SerializeField, Tooltip("向きによらずオフセット角度を同じ方向に適用するかどうか")]
    private bool m_ApplyOffsetAngleWithoutForward = true;

    private Transform m_FrontCamera;

    public override void OnStart()
    {
        base.OnStart();
        m_FrontCamera = BattleRealCameraManager.Instance.GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA).transform;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        
        var delta = m_FrontCamera.position - m_CharacterView.position;

        // X軸の差分はあえて0にする
        delta.x = 0;
        var angle = Vector3.Angle(m_CharacterView.forward, delta);

        var offset = m_OffsetAngle;
        if (m_ApplyOffsetAngleWithoutForward)
        {
            offset *= Mathf.Sign(m_CharacterView.forward.z);
        }

        m_CharacterView.RotateAround(m_CharacterView.position, m_CharacterView.right, 90 - angle + offset);
    }
}
