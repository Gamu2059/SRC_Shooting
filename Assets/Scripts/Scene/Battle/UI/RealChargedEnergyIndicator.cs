#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RealChargedEnergyIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    private FloatReactiveProperty m_CurrentChargedEnergy;

    // Start is called before the first frame update
    void Start()
    {
        if(BattleRealPlayerManager.Instance != null){
            RegisterChargedEnergy();
        }else{
            BattleRealPlayerManager.OnStartAction += RegisterChargedEnergy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleRealManager.Instance == null){
            return;
        }
    }

    private void RegisterChargedEnergy(){
        //m_CurrentChargedEnergy = BattleRealPlayerManager.Instance.GetCurrentBombCharge();
        //m_CurrentChargedEnergy.SubscribeToText(m_OutText, _ =>{
        //    return string.Format("{0}/{1}", m_CurrentChargedEnergy.Value, "9999");
        //});
    }
}
