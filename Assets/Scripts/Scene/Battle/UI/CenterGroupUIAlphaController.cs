#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CenterGroupUIAlphaController : MonoBehaviour
{
    #region Define

    [Serializable]
    private enum E_MODE
    {
        REAL_MODE,
        HACKING_MODE,
    }

    #endregion

    #region Field Inspector

    [SerializeField]
    private CanvasGroup m_CanvasGroup;

    [SerializeField]
    private E_MODE m_Mode;

    [SerializeField]
    private float m_NormalAlpha;

    [SerializeField]
    private float m_DuplicateAlpha;

    #endregion

    #region Field

    private Rect m_FieldRate;

    #endregion

    #region Game Cycle

    private void Start()
    {
        CalcFieldRate();
    }

    private void LateUpdate()
    {
        if (m_Mode == E_MODE.REAL_MODE)
        {
            CheckDuplicateRealModePlayer();
        }
        else
        {
            CheckDuplicateHackingModePlayer();
        }
    }

    #endregion

    private void CalcFieldRate()
    {
        var rect = (transform as RectTransform).rect;
        var size = (transform.parent as RectTransform).rect.size;
        m_FieldRate = new Rect();
        m_FieldRate.xMin = rect.xMin / size.x;
        m_FieldRate.xMax = rect.xMax / size.x;
        // y軸はsize.y分加算した値で考慮しないといけないので1を足す
        m_FieldRate.yMin = rect.yMin / size.y + 1f;
        m_FieldRate.yMax = rect.yMax / size.y + 1f;
    }

    private void CheckDuplicateRealModePlayer()
    {
        if (BattleRealPlayerManager.Instance == null || BattleRealStageManager.Instance == null)
        {
            return;
        }

        var player = BattleRealPlayerManager.Instance.Player;
        if (player == null)
        {
            return;
        }

        var vPos = BattleRealStageManager.Instance.CalcViewportPosFromWorldPosition(player.transform, false);
        vPos += Vector2.one * 0.5f;
        if (vPos.x >= m_FieldRate.xMin && vPos.x <= m_FieldRate.xMax && vPos.y >= m_FieldRate.yMin && vPos.y <= m_FieldRate.yMax)
        {
            m_CanvasGroup.alpha = m_DuplicateAlpha;
        }
        else
        {
            m_CanvasGroup.alpha = m_NormalAlpha;
        }
    }

    private void CheckDuplicateHackingModePlayer()
    {
        if (BattleHackingPlayerManager.Instance == null || BattleHackingStageManager.Instance == null)
        {
            return;
        }

        var player = BattleHackingPlayerManager.Instance.Player;
        if (player == null)
        {
            return;
        }

        var vPos = BattleHackingStageManager.Instance.CalcViewportPosFromWorldPosition(player.transform, false);
        vPos += Vector2.one * 0.5f;
        if (vPos.x >= m_FieldRate.xMin && vPos.x <= m_FieldRate.xMax && vPos.y >= m_FieldRate.yMin && vPos.y <= m_FieldRate.yMax)
        {
            m_CanvasGroup.alpha = m_DuplicateAlpha;
        }
        else
        {
            m_CanvasGroup.alpha = m_NormalAlpha;
        }
    }
}
