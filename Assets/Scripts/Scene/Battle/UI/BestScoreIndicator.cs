#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// BestScoreを表示する
/// </summary>
public class BestScoreIndicator : ControllableMonoBehavior
{
    [SerializeField]
    private Text m_OutText;

    private FloatReactiveProperty m_DisplayedBestScore;

    // Start is called before the first frame update
    void Start()
    {
        if (BattleRealPlayerManager.Instance != null && BattleManager.Instance != null)
        {
            RegisterBestScore();
        }
        else
        {
            BattleRealPlayerManager.OnStartAction += RegisterBestScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleRealPlayerManager.Instance == null || GameManager.Instance == null)
        {
            return;
        }

        var battleData = DataManager.Instance.BattleData;

        if (battleData.Score >= battleData.BestScore)
        {
            battleData.UpdateBestScore(battleData.Score);
            //m_DisplayedBestScore.SetValueAndForceNotify(battleData.BestScore);
        }
    }

    private void RegisterBestScore()
    {
        //m_DisplayedBestScore = new FloatReactiveProperty(DataManager.Instance.BattleData.BestScore);
        //m_DisplayedBestScore.SubscribeToText(m_OutText);
    }
}
