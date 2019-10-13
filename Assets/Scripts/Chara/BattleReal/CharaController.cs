using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラの制御を行うコンポーネント。
/// </summary>
public class CharaController : BattleRealObjectBase
{
	#region Field Inspector

	[Header( "キャラの基礎パラメータ" )]

	[SerializeField, Tooltip( "キャラの所属" )]
	private E_CHARA_TROOP m_Troop;

	[SerializeField, Tooltip( "キャラが用いる弾の組み合わせ" )]
	private BulletSetParam m_BulletSetParam;

	[Header( "キャラの基礎ステータス" )]

	[SerializeField, Tooltip( "キャラの現在HP" )]
	private float m_NowHp;

	[SerializeField, Tooltip( "キャラの最大HP" )]
	private float m_MaxHp;

	#endregion



	#region Getter & Setter


	public E_CHARA_TROOP GetTroop()
	{
		return m_Troop;
	}

	public BulletSetParam GetBulletSetParam()
	{
		return m_BulletSetParam;
	}

	public void SetBulletSetParam( BulletSetParam param )
	{
		m_BulletSetParam = param;
	}

    public BulletParam GetBulletParam(int index = 0)
    {
        return m_BulletSetParam.GetBulletParam(index);
    }

    public int GetBulletPrefabsCount()
    {
        return m_BulletSetParam.GetBulletPrefabsCount();
    }

	#endregion


    /// <summary>
    /// HPを初期化する
    /// </summary>
    /// <param name="hp">最大HP</param>
    public void InitHp(float hp)
    {
        m_MaxHp = m_NowHp = hp;
    }

	/// <summary>
	/// このキャラを回復する。
	/// </summary>
	public void Recover( float recover )
	{
		if( recover <= 0 )
		{
			return;
		}

		m_NowHp = Mathf.Clamp( m_NowHp + recover, 0, m_MaxHp );
	}

	/// <summary>
	/// このキャラにダメージを与える。
	/// HPが0になった場合は死ぬ。
	/// </summary>
	public void Damage(float damage )
	{
		if( damage <= 0 )
		{
			return;
		}

		m_NowHp = Mathf.Clamp( m_NowHp - damage, 0, m_MaxHp );

		if( m_NowHp == 0 )
		{
			Dead();
		}
	}

	/// <summary>
	/// このキャラを死亡させる。
	/// </summary>
	public virtual void Dead()
	{

	}

    /// <summary>
    /// 他の弾から当てられた時の処理。
    /// </summary>
    /// <param name="attackBullet">他の弾</param>
    /// <param name="attackData">他の弾の衝突情報</param>
    /// <param name="targetData">このキャラの衝突情報</param>
    public virtual void SufferBullet(BulletController attackBullet, ColliderData attackData, ColliderData targetData)
    {
        Damage(1);
    }

    /// <summary>
    /// 他のキャラから当てられた時の処理。
    /// </summary>
    /// <param name="attackChara">他のキャラ</param>
    /// <param name="attackData">他のキャラの衝突情報</param>
    /// <param name="targetData">このキャラの衝突情報</param>
    public virtual void SufferChara(CharaController attackChara, ColliderData attackData, ColliderData targetData)
    {
        Damage(1);
    }

    /// <summary>
    /// 他のキャラに当たった時の処理。
    /// </summary>
    /// <param name="targetChara">他のキャラ</param>
    /// <param name="attackData">このキャラの衝突情報</param>
    /// <param name="targetData">他のキャラの衝突情報</param>
    public virtual void HitChara(CharaController targetChara, ColliderData attackData, ColliderData targetData)
    {

    }

    /// <summary>
    /// 他のアイテムに当たった時の処理。
    /// </summary>
    /// <param name="targetItem">他のアイテム</param>
    /// <param name="attackData">このキャラの衝突情報</param>
    /// <param name="targetData">他のアイテムの衝突情報</param>
    public virtual void HitItem(ItemController targetItem, ColliderData attackData, ColliderData targetData)
    {

    }

	/// <summary>
	/// 複数の弾を拡散させたい時の拡散角度のリストを取得する。
	/// </summary>
	/// <param name="bulletNum">弾の個数</param>
	/// <param name="spreadAngle">弾同士の角度間隔</param>
	protected static List<float> GetBulletSpreadAngles( int bulletNum, float spreadAngle )
	{
		List<float> spreadAngles = new List<float>();

		if( bulletNum % 2 == 1 )
		{
			spreadAngles.Add( 0f );

			for( int i = 0; i < ( bulletNum - 1 ) / 2; i++ )
			{
				spreadAngles.Add( spreadAngle * ( i + 1f ) );
				spreadAngles.Add( spreadAngle * -( i + 1f ) );
			}
		}
		else
		{
			for( int i = 0; i < bulletNum / 2; i++ )
			{
				spreadAngles.Add( spreadAngle * ( i + 0.5f ) );
				spreadAngles.Add( spreadAngle * -( i + 0.5f ) );
			}
		}

		return spreadAngles;
	}
}
