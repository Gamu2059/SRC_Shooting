#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUiManager : ControllableMonoBehavior
{
	[SerializeField]
	private Button m_StartButton;

    [SerializeField]
    private PlaySoundParam m_TitleBgm;

    [SerializeField]
    private PlaySoundParam m_StartSe;

	public override void OnStart()
	{
		base.OnStart();

        AudioManager.Instance.Play(m_TitleBgm);
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
        DataManager.Instance.BattleData.ResetData(E_STAGE.NORMAL_1);

        AudioManager.Instance.StopAllBgm();
        AudioManager.Instance.StopAllSe();
        AudioManager.Instance.Play(m_StartSe);

        BaseSceneManager.Instance.LoadScene( BaseSceneManager.E_SCENE.STAGE1 );
	}
}
