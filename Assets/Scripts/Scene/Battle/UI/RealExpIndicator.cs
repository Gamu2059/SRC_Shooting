#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RealExpIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    private IntReactiveProperty m_CurrentExp;

    // Start is called before the first frame update
    void Start()
    {
        if(BattleRealPlayerManager.Instance != null){
            RegisterExp();
        }else{
            BattleRealPlayerManager.OnStartAction += RegisterExp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleRealManager.Instance == null){
            return;
        }
    }

    private void RegisterExp(){
        m_CurrentExp = BattleRealPlayerManager.Instance.GetCurrentExp();
        m_CurrentExp.SubscribeToText(m_OutText, _ =>{
            int currentLv = BattleRealPlayerManager.Instance.GetCurrentLevel().Value;
            var expParamSet = BattleRealPlayerManager.Instance.GetRealPlayerExpParamSet();

            if(currentLv == expParamSet.Length){
                return "MAX EXP";
            }else{
                return string.Format("{0}/{1}", m_CurrentExp.Value, expParamSet[currentLv - 1].NextLevelNecessaryExp);    
            }
        });
    }
}
