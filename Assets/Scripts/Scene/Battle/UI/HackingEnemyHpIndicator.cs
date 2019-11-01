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
    private BattleHackingEnemyController m_Target;
    private float m_TargetNowHp;
    private float m_TargetMaxHp;

    private BattleHackingEnemyController GetTarget(){
        foreach (var enemy in BattleHackingEnemyManager.Instance.Enemies)
        {
            if(enemy.IsBoss){
                return enemy;
            }
        }
        return null;
    }

    public void ResetIndicator(){
        m_Target = GetTarget();

        if(m_Target == null){
            return;    
        }

        m_TargetNowHp = m_Target.NowHp;
        m_TargetMaxHp = m_Target.MaxHp;

        m_OutText.text = string.Format("{0}/{1}", m_TargetNowHp, m_TargetMaxHp);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleHackingManager.Instance == null){
            return;
        }

        if(m_Target == null){
            ResetIndicator();
        }else{
            float currentNowHp = m_Target.NowHp;
            if(m_TargetNowHp != currentNowHp){
                m_TargetNowHp = currentNowHp;
                m_OutText.text = m_OutText.text = string.Format("{0}/{1}", m_TargetNowHp, m_TargetMaxHp);
            }
        }
    }
}
