using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの入力パラメータ。
/// </summary>
[CreateAssetMenu( menuName = "Param/Input", fileName = "InputParam" )]
public class InputParam : ScriptableObject
{
	[SerializeField]
	private KeyCode[] m_ForwardMove;

	[SerializeField]
	private KeyCode[] m_BackMove;

	[SerializeField]
	private KeyCode[] m_RightMove;

	[SerializeField]
	private KeyCode[] m_LeftMove;

	[SerializeField]
	private KeyCode[] m_ShotBullet;

	[SerializeField]
	private KeyCode[] m_ShotBomb;

	[SerializeField]
	private KeyCode[] m_1stCharaChange;

	[SerializeField]
	private KeyCode[] m_2ndCharaChange;

	[SerializeField]
	private KeyCode[] m_3rdCharaChange;



	public KeyCode[] GetForwardMove()
	{
		return m_ForwardMove;
	}

	public KeyCode[] GetBackMove()
	{
		return m_BackMove;
	}

	public KeyCode[] GetRightMove()
	{
		return m_RightMove;
	}

	public KeyCode[] GetLeftMove()
	{
		return m_LeftMove;
	}

	public KeyCode[] GetShotBullet()
	{
		return m_ShotBullet;
	}

	public KeyCode[] GetShotBomb()
	{
		return m_ShotBomb;
	}

	public KeyCode[] Get1stCharaChange()
	{
		return m_1stCharaChange;
	}

	public KeyCode[] Get2ndCharaChange()
	{
		return m_2ndCharaChange;
	}

	public KeyCode[] Get3rdCharaChange()
	{
		return m_3rdCharaChange;
	}
}
