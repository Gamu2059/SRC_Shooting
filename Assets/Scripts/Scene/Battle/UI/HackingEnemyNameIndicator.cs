#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ハッキングモードの敵の名前を表示するインジケータ
/// </summary>
public class HackingEnemyNameIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    void Start()
    {
        m_OutText.text = "INF-C-761 : ";
    }
}
