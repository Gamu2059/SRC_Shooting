using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUiManager : SingletonMonoBehavior<TitleUiManager>
{
	[SerializeField]
	private Button m_StartButton;

	public override void OnStart()
	{
		base.OnStart();

		m_StartButton.onClick.AddListener( GotoMenu );
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if( Input.anyKey )
		{
			GotoMenu();
		}
	}

	private void GotoMenu()
	{
		BaseSceneManager.Instance.LoadScene( BaseSceneManager.E_SCENE.BATTLE );
	}
}
