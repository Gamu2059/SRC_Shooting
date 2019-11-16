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

    // Start is called before the first frame update
    void Start()
    {
        m_OutText.text = "INF-C-761 : ";
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleHackingManager.Instance == null){
            return;
        }
    }
}
