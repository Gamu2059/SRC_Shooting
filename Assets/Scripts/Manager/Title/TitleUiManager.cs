using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUiManager : ControllableMonoBehavior
{
    private const string BGM_TITLE = "BGM_Title";
    private const string SE_START = "SE_System_Start";

	[SerializeField]
	private Button m_StartButton;

	public override void OnStart()
	{
		base.OnStart();

        AudioManager.Instance.PlayBgm(BGM_TITLE);
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
        AudioManager.Instance.StopBgm();
        AudioManager.Instance.PlaySe(AudioManager.E_SE_GROUP.SYSTEM, SE_START);
		BaseSceneManager.Instance.LoadScene( BaseSceneManager.E_SCENE.STAGE1 );
	}
}
