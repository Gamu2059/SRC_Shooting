using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RealLevelIndicator : MonoBehaviour
{
    [SerializeField]
    private Text m_OutText;

    private IntReactiveProperty m_Level;

    // Start is called before the first frame update
    void Start()
    {
        if(BattleRealPlayerManager.Instance != null){
            RegisterLevel();
        }else{
            BattleRealPlayerManager.OnStartAction += RegisterLevel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleRealManager.Instance == null){
            return;
        }
    }

    private void RegisterLevel(){
        m_Level = BattleRealPlayerManager.Instance.GetCurrentLevel();
        m_Level.SubscribeToText(m_OutText);
    }
}
