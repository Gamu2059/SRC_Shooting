
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// Scoreを表示する
/// </summary>
public class ScoreIndicator : MonoBehaviour
{
    private enum DisplayOnConsole{
        ENABLE,
        DISABLE,
    }

    [SerializeField]
    private DisplayOnConsole m_DisplayOnConsole;

    [SerializeField]
    private Text m_OutText;

    private FloatReactiveProperty m_Score;  
    
    // Start is called before the first frame update
    void Start()
    {
        if (BattleRealPlayerManager.Instance != null){
            RegisterScore();
        } else {
            BattleRealPlayerManager.OnStartAction += RegisterScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleRealManager.Instance == null){
            return;
        }
        
        if(m_DisplayOnConsole == DisplayOnConsole.ENABLE){
            
            if(m_Score == null){
                return;
            }

            Debug.Log(string.Format("Score : {0}", m_Score.Value));
        }
    }

    private void RegisterScore()
    {
        m_Score = BattleRealPlayerManager.Instance.GetCurrentScore();
        m_Score.SubscribeToText(m_OutText, x => m_OutText.text = string.Format("Score : {0}", m_Score.Value));
    }
}
