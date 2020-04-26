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

    public bool IsEnableController { get; set; }

    private Transform m_FrontCamera;

    public override void OnStart()
    {
        base.OnStart();
        m_FrontCamera = BattleRealCameraManager.Instance.GetCameraController(E_CAMERA_TYPE.FRONT_CAMERA).transform;
        IsEnableController = true;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        var delta = m_FrontCamera.position - m_CharacterView.position;
        delta.x = 0;
        var angle = Vector3.Angle(Vector3.forward, delta);

        m_CharacterView.localEulerAngles = Vector3.zero;
        m_CharacterView.RotateAround(m_CharacterView.position, Vector3.right, 90 - angle + m_OffsetAngle);
    }
}
