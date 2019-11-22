using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconChanger : MonoBehaviour
{
    [SerializeField]
    private Animator m_IconAnimator;

    private bool m_IsLaserType = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_IconAnimator != null)
            {
                if (m_IsLaserType)
                {
                    m_IconAnimator.Play("select_to_bomb", 0);
                }
                else
                {
                    m_IconAnimator.Play("select_to_laser", 0);
                }
                m_IsLaserType = !m_IsLaserType;
            }
        }
    }
}
