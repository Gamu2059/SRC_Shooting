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

    protected override void OnEnterHitItem(HitSufferData<BattleRealItemController> hitData)
    {
        base.OnEnterHitItem(hitData);

        var itemColliderType = hitData.SufferCollider.Transform.ColliderType;
        switch (itemColliderType)
        {
            case E_COLLIDER_TYPE.ITEM_ATTRACT:
                hitData.OpponentObject.AttractPlayer();
                break;
            case E_COLLIDER_TYPE.ITEM_GAIN:
                GetItem(hitData.OpponentObject);
                break;
            default:
                break;
        }
    }

    private void GetItem(BattleRealItemController item)
    {
        if (item == null)
        {
            return;
        }

        /* デバッグ用 */
        Debug.Log("You got " + item.ItemType + "! : " + item.ItemPoint + "pts!");

        switch (item.ItemType)
        {
            case E_ITEM_TYPE.SMALL_SCORE:
                BattleRealPlayerManager.Instance.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_SCORE:
                BattleRealPlayerManager.Instance.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_SCORE_UP:
                BattleRealPlayerManager.Instance.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_SCORE_UP:
                BattleRealPlayerManager.Instance.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_EXP:
                BattleRealPlayerManager.Instance.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_EXP:
                BattleRealPlayerManager.Instance.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.SMALL_BOMB:
                BattleRealPlayerManager.Instance.AddScore(item.ItemPoint);
                break;
            case E_ITEM_TYPE.BIG_BOMB:
                BattleRealPlayerManager.Instance.AddScore(item.ItemPoint);
                break;
        }
    }

    public int GetLevel()
    {
        //return BattleRealPlayerManager.Instance.GetCurrentLevel().Value;
        return 0;
    }

    public override void Dead()
    {
        if (BattleManager.Instance.m_PlayerNotDead)
        {
            return;
        }

        base.Dead();

        gameObject.SetActive(false);
        BattleManager.Instance.GameOver();
    }
}
