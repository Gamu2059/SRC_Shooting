#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RealEnergyLevelIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    private IntReactiveProperty m_CurrentEnergyLevel;

    // Start is called before the first frame update
    void Start()
    {
        if(BattleRealPlayerManager.Instance != null){
            RegisterEnergyLevel();
        }else{
            BattleRealPlayerManager.OnStartAction += RegisterEnergyLevel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleRealManager.Instance == null){
            return;
        }
    }

    private void RegisterEnergyLevel(){
        m_CurrentEnergyLevel = BattleRealPlayerManager.Instance.GetCurrentBombNum();
        m_CurrentEnergyLevel.SubscribeToText(m_OutText);
    }
}
