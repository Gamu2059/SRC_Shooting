using System.Collections;
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
            m_DisplayedValue = GameManager.Instance.PlayerData.m_Last;
            m_OutText.text = m_DisplayedValue.ToString(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance != null){
            int currentLast = GameManager.Instance.PlayerData.m_Last;
            if(m_DisplayedValue != currentLast){
                m_DisplayedValue = currentLast;
                m_OutText.text = m_DisplayedValue.ToString();
            }
        }
    }
}
