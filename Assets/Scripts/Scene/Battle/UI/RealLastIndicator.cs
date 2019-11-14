﻿using System.Collections;
#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リアルモードのプレイヤーの残機を表示する
/// (今のところは数字で)
/// </summary>
public class RealLastIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    private int m_DisplayedValue;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance != null){
            m_DisplayedValue = GameManager.Instance.PlayerData.Remain;
            m_OutText.text = m_DisplayedValue.ToString(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance != null){
            int currentLast = GameManager.Instance.PlayerData.Remain;
            if(m_DisplayedValue != currentLast){
                m_DisplayedValue = currentLast;
                m_OutText.text = m_DisplayedValue.ToString();
            }
        }
    }
}
