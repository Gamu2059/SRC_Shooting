using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードのプレイヤーコントローラ
/// </summary>
public class BattleRealPlayerController : CharaController
{
	/// <summary>
	/// プレイヤーキャラのライフサイクル
	/// </summary>
	[System.Serializable]
	public enum E_PLAYER_LIFE_CYCLE
	{
		/// <summary>
		/// 戦闘画面には出ていない
		/// </summary>
		AHEAD,

		/// <summary>
		/// 現在戦闘中
		/// </summary>
		SORTIE,

		/// <summary>
		/// 死亡により戦闘画面から退場
		/// </summary>
		DEAD,
	}

    #region Field

    private BattleRealPlayerParamSet m_ParamSet;

    private int m_Level;

    private float m_ShotRemainTime;

    #endregion

    #region Game Cycle

    private void Start()
	{
        // 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
        BattleRealPlayerManager.RegisterPlayer(this);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        m_ShotRemainTime = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_ShotRemainTime -= Time.deltaTime;
    }

    #endregion

    public void SetParamSet(BattleRealPlayerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    /// <summary>
    /// 通常弾を発射する。
    /// </summary>
    public virtual void ShotBullet()
	{
        
	}

    public void ChargeLaser()
    {

    }

    public virtual void ShotLaser()
    {

    }

    public void ChargeBomb()
    {

    }

    public virtual void ShotBomb()
    {

    }

    protected override void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (selfColliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            Damage(1);
        }
    }

    protected override void OnEnterSufferChara(HitSufferData<CharaController> sufferData)
    {
        base.OnEnterSufferChara(sufferData);

        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (selfColliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            Damage(1);
        }
    }

    //public override void HitItem(ItemController targetItem, ColliderData attackData, ColliderData targetData)
    //{
    //    base.HitItem(targetItem, attackData, targetData);

    //    //if (targetData.CollideName != ItemController.GAIN_COLLIDE)
    //    //{
    //    //    return;
    //    //}

    //    switch(targetItem.GetItemType())
    //    {
    //        case E_ITEM_TYPE.SMALL_SCORE:
    //        case E_ITEM_TYPE.BIG_SCORE:
    //            //BattleRealPlayerManager.Instance.AddScore(targetItem.GetPoint());
    //            break;
    //        case E_ITEM_TYPE.SMALL_SCORE_UP:
    //        case E_ITEM_TYPE.BIG_SCORE_UP:
    //            break;
    //        case E_ITEM_TYPE.SMALL_EXP:
    //        case E_ITEM_TYPE.BIG_EXP:
    //            //BattleRealPlayerManager.Instance.AddExp(targetItem.GetPoint());
    //            break;
    //        case E_ITEM_TYPE.SMALL_BOMB:
    //        case E_ITEM_TYPE.BIG_BOMB:
    //            //BattleRealPlayerManager.Instance.AddBombCharge(targetItem.GetPoint());
    //            break;
    //    }
    //}

    public int GetLevel()
	{
        //return BattleRealPlayerManager.Instance.GetCurrentLevel().Value;
        return 0;
	}

	public override void Dead()
	{
		if( BattleManager.Instance.m_PlayerNotDead )
		{
			return;
		}

		base.Dead();

		gameObject.SetActive( false );
		BattleManager.Instance.GameOver();
	}
}
