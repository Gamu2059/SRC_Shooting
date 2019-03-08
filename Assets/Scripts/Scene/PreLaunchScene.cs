using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLaunchScene : BaseScene
{

	[SerializeField]
	private BaseSceneManager.E_SCENE m_NextScene;

	private void Start()
	{
		BaseSceneManager.Instance.OnInitialize();

		BaseSceneManager.Instance.LoadScene( m_NextScene );
	}
}
