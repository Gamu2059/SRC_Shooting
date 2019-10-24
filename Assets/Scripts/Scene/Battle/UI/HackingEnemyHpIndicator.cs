using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ハッキングモードの敵HPを表示するインジケーター
/// </summary>
public class HackingEnemyHpIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    // Start is called before the first frame update
    void Start()
    {
        m_OutText.text = string.Format("{0}/{1}", 9999, 9999); 
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleHackingManager.Instance == null){
            return;
        }   
    }
}
