using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : ControllableMonoBehavior
{
    [SerializeField]
    private Transform m_Overall;

    [SerializeField]
    private Transform m_MotherColony;

    [SerializeField]
    private float m_OverallRotateSpeed;

    [SerializeField]
    private float m_MotherColonyRotateSpeed;

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_Overall.RotateAround(m_Overall.position, m_Overall.up, m_OverallRotateSpeed * Time.deltaTime);
        m_MotherColony.RotateAround(m_MotherColony.position, m_MotherColony.up, m_MotherColonyRotateSpeed * Time.deltaTime);
    }
}
