using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class BaseScene : MonoBehaviour
{

	[SerializeField]
	private BaseSceneManager.E_SCENE m_Scene;

	public BaseSceneManager.E_SCENE Scene
	{
		get
		{
			return m_Scene;
		}
	}


	private void Awake()
	{

		if( !BaseSceneManager.CheckExistInstance() )
		{
			SceneManager.LoadScene( 0 );
			return;
		}

		BaseSceneManager.Instance.RegisterScene( this );
	}

	public virtual void OnBeforeHide( Action onComplete = null )
	{
		EventUtility.SafeInvokeAction( onComplete );
	}

	public virtual void OnAfterHide( Action onComplete = null )
	{
		EventUtility.SafeInvokeAction( onComplete );
	}

	public virtual void OnBeforeShow( Action onComplete = null )
	{
		EventUtility.SafeInvokeAction( onComplete );
	}

	public virtual void OnAfterShow( Action onComplete = null )
	{
		EventUtility.SafeInvokeAction( onComplete );
	}
}