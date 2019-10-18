using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// BestScoreを表示
/// </summary>
public class BestScoreIndicator : MonoBehaviour
{
	[SerializeField]
	private Text m_OutText;

	private FloatReactiveProperty m_DisplayedBestScore;

	// Start is called before the first frame update
	void Start()
	{
		if (BattleRealPlayerManager.Instance != null && BattleManager.Instance != null){
            RegisterBestScore();
        } else {
            BattleRealPlayerManager.OnStartAction += RegisterBestScore;
        }
	}

	// Update is called once per frame
	void Update()
	{
		if(BattleRealPlayerManager.Instance == null || BattleManager.Instance == null){
			return;
		}

		var currentScore = BattleRealPlayerManager.Instance.GetCurrentScore();
		
		if(currentScore != null){
			var currentScoreValue = currentScore.Value;
			
			if(currentScoreValue >= BattleManager.Instance.BestScore){
				BattleManager.Instance.UpdateBestScore(currentScoreValue);
				m_DisplayedBestScore.SetValueAndForceNotify(BattleManager.Instance.BestScore);
			}
		}
	}

	private void RegisterBestScore(){
		m_DisplayedBestScore = new FloatReactiveProperty(BattleManager.Instance.BestScore);
		m_DisplayedBestScore.SubscribeToText(m_OutText);
	}
}
