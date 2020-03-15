#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ハッキングモードの制限時間を表示するインジケーター
/// </summary>
public class HackingRemainedTimeIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    void Start()
    {
        m_OutText.text = string.Format("{0}", 9999); 
    }
}
