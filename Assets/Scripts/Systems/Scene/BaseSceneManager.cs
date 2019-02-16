using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

#if UNITY_EDITOR
	using UnityEditor;
	using System.Linq;
	using System.Text;
	using System.IO;
#endif

public class BaseSceneManager : GlobalSingletonMonoBehavior<BaseSceneManager>
{

	[Serializable]
	public enum E_SCENE
	{
		DEFAULT = -1,
		PRE_LAUNCH = 0,
		MAIN = 1,
	}

	[Serializable]
	public struct TransitionInfo
	{
		public E_SCENE CurrentScene;
		public E_SCENE NextScene;
		public List<E_SCENE> AdditiveScene;
		//public FadeOutBase FadeOutEffect;
		//public LoadingBase LoadingEffect;
		//public FadeInBase FadeInEffect;
	}

	const float MAX_LOAD_PROGRESS = 0.9f;

	[SerializeField]
	List<TransitionInfo> m_TransitionInfos;

	[SerializeField]
	private E_SCENE m_CurrentInfoNextScene;

	[SerializeField]
	private List<E_SCENE> m_CurrentInfoNextAdditiveScenes;

	[SerializeField]
	private BaseScene m_CurrentScene;

	[SerializeField]
	private List<BaseScene> m_CurrentAdditiveScenes;

	public void LoadScene( E_SCENE nextScene )
	{
		int nowIndex = SceneManager.GetActiveScene().buildIndex;
		int nextIndex = ( int )nextScene;

		// デフォルト値を指定している場合は無視
		if( nextIndex < 0 )
		{
			return;
		}

		// 現在のシーンと指定シーンとの組み合わせを探す
		foreach( var info in m_TransitionInfos )
		{
			if( nowIndex == ( int )info.CurrentScene && nextScene == info.NextScene )
			{
				StartCoroutine( LoadSceneSequence( info, nextScene, info.AdditiveScene ) );
				return;
			}
		}

		// 現在のシーンをワイルドカードとして指定シーンとの組み合わせを探す
		foreach( var info in m_TransitionInfos )
		{
			if( info.CurrentScene < 0 && nextScene == info.NextScene )
			{
				StartCoroutine( LoadSceneSequence( info, nextScene, info.AdditiveScene ) );
				return;
			}
		}

		// 現在のシーンと指定したシーンをワイルドカードとして組み合わせを探す
		foreach( var info in m_TransitionInfos )
		{
			if( nowIndex == ( int )info.CurrentScene && info.NextScene < 0 )
			{
				StartCoroutine( LoadSceneSequence( info, nextScene, null ) );
				return;
			}
		}

		// 現在のシーンも指定シーンもワイルドカードとして組み合わせを探す
		foreach( var info in m_TransitionInfos )
		{
			if( info.CurrentScene < 0 && info.NextScene < 0 )
			{
				StartCoroutine( LoadSceneSequence( info, nextScene, null ) );
				return;
			}
		}

		// 完全に見つからなかった場合
		SceneManager.LoadScene( nextIndex );
	}

	public void RegisterScene( BaseScene scene )
	{
		if( scene == null )
		{
			return;
		}

		if( scene.Scene == m_CurrentInfoNextScene )
		{
			m_CurrentScene = scene;
			return;
		}

		if( m_CurrentInfoNextAdditiveScenes.Contains( scene.Scene ) && !m_CurrentAdditiveScenes.Contains( scene ) )
		{
			m_CurrentAdditiveScenes.Add( scene );
			return;
		}
	}

	//シーンを読み込む流れのコルーチン ロード画面だけ非同期
	private IEnumerator LoadSceneSequence( TransitionInfo info, E_SCENE nextScene, List<E_SCENE> nextAdditiveScenes = null )
	{

		if( nextScene < 0 )
		{
			yield break;
		}

		m_CurrentInfoNextScene = nextScene;
		m_CurrentInfoNextAdditiveScenes = nextAdditiveScenes;

		//FadeOutBase fadeOut = info.FadeOutEffect;
		//FadeInBase fadeIn = info.FadeInEffect;

		// フェードアウト前処理
		if( m_CurrentAdditiveScenes != null )
		{
			foreach( var scene in m_CurrentAdditiveScenes )
			{
				if( scene == null )
				{
					continue;
				}

				scene.OnBeforeHide();
			}
		}

		if( m_CurrentScene != null )
		{
			m_CurrentScene.OnBeforeHide();
		}

		// フェードアウト処理
		//if( fadeOut != null )
		//{
		//	var obj = Instantiate( info.FadeOutEffect );
		//	yield return StartCoroutine( obj.FadeOut() );
		//	Destroy( obj );
		//}

		////ロード画面生成
		//var loadingObj = Instantiate( info.LoadingEffect );

		// フェードアウト後処理
		if( m_CurrentAdditiveScenes != null )

		{
			foreach( var scene in m_CurrentAdditiveScenes )
			{
				if( scene == null )
				{
					continue;
				}

				scene.OnAfterHide();
			}
		}

		if( m_CurrentScene != null )
		{
			m_CurrentScene.OnAfterHide();
		}

		m_CurrentScene = null;
		m_CurrentAdditiveScenes = new List<BaseScene>();

		// シーンロード処理
		List<AsyncOperation> asyncList = new List<AsyncOperation>();
		asyncList.Add( SceneManager.LoadSceneAsync( ( int )nextScene ) );

		if( nextAdditiveScenes == null )
		{
			foreach( var scene in nextAdditiveScenes )
			{
				if( scene < 0f )
				{
					continue;
				}

				asyncList.Add( SceneManager.LoadSceneAsync( ( int )scene, LoadSceneMode.Additive ) );
			}
		}

		// ロード中
		while( true )
		{
			yield return null;

			bool isDone = true;
			float progress = 0;

			foreach( var async in asyncList )
			{
				if( !async.isDone )
				{
					isDone = false;
				}

				progress += async.progress;
			}

			progress /= asyncList.Count;

			//loadingObj.m_Progress = async.progress / MAX_LOAD_PROGRESS;
			if( isDone )
			{
				break;
			}
		}

		// ロード完了待機
		while( true )
		{
			yield return null;

			if( m_CurrentScene == null )
			{
				continue;
			}

			if( m_CurrentAdditiveScenes != null && m_CurrentInfoNextAdditiveScenes != null )
			{
				if( m_CurrentAdditiveScenes.Count < m_CurrentInfoNextAdditiveScenes.Count )
				{
					continue;
				}
			}

			break;
		}

		//ロード完了通知
		//loadingObj.OnLoadComplete();

		// フェードイン前処理
		if( m_CurrentScene != null )
		{
			m_CurrentScene.OnBeforeShow();
		}

		if( m_CurrentAdditiveScenes != null )
		{
			foreach( var scene in m_CurrentAdditiveScenes )
			{
				if( scene == null )
				{
					continue;
				}

				scene.OnBeforeShow();
			}
		}

		//フェードイン処理
		//if( fadeIn != null )
		//{
		//	var obj = Instantiate( info.FadeInEffect );
		//	yield return StartCoroutine( obj.FadeIn() );
		//	Destroy( obj );
		//}

		// フェードイン後処理
		if( m_CurrentScene != null )
		{
			m_CurrentScene.OnAfterShow();
		}

		if( m_CurrentAdditiveScenes != null )
		{
			foreach( var scene in m_CurrentAdditiveScenes )
			{
				if( scene == null )
				{
					continue;
				}

				scene.OnAfterShow();
			}
		}
	}
}
