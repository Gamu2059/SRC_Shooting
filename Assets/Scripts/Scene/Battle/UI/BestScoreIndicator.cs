#pragma warning disable 0649

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
		if(BattleRealPlayerManager.Instance == null || GameManager.Instance == null){
			return;
		}

		var currentScore = BattleRealPlayerManager.Instance.GetCurrentScore();
		
		if(currentScore != null){
			var currentScoreValue = currentScore.Value;
			
			if(currentScoreValue >= GameManager.Instance.PlayerData.BestScore){
				GameManager.Instance.PlayerData.UpdateBestScore(currentScoreValue);
				m_DisplayedBestScore.SetValueAndForceNotify(GameManager.Instance.PlayerData.BestScore);
			}
		}
	}

	private void RegisterBestScore(){
		m_DisplayedBestScore = new FloatReactiveProperty(GameManager.Instance.PlayerData.BestScore);
		m_DisplayedBestScore.SubscribeToText(m_OutText);
	}
}
