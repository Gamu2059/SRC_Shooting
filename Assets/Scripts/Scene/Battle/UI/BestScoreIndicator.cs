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

	private float m_BestScore;

	private FloatReactiveProperty m_DisplayedBestScore;

	// Start is called before the first frame update
	void Start()
	{
		m_BestScore = 0.0f;

		if (BattleRealPlayerManager.Instance != null){
            RegisterBestScore();
        } else {
            BattleRealPlayerManager.OnStartAction += RegisterBestScore;
        }
	}

	// Update is called once per frame
	void Update()
	{
		if(BattleRealPlayerManager.Instance == null){
			return;
		}

		var currentScore = BattleRealPlayerManager.Instance.GetCurrentScore();
		
		if(currentScore != null){
			var currentScoreValue = currentScore.Value;
			if(currentScoreValue >= m_BestScore){
				m_BestScore = currentScoreValue;
				m_DisplayedBestScore.SetValueAndForceNotify(m_BestScore);
			}
		}
	}

	private void RegisterBestScore(){
		m_DisplayedBestScore = new FloatReactiveProperty(0.0f);
		m_DisplayedBestScore.SubscribeToText(m_OutText);
	}
}
