using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラの制御を行うコンポーネント。
/// </summary>
public class CharaController : BattleRealObjectBase
{
    #region Field Inspector


    [SerializeField, Tooltip("キャラが用いる弾の組み合わせ")]
    private BulletSetParam m_BulletSetParam;

    [Header("キャラの基礎ステータス")]

    [SerializeField, Tooltip("キャラの現在HP")]
    private float m_NowHp;

    [SerializeField, Tooltip("キャラの最大HP")]
    private float m_MaxHp;

    #endregion

    #region Field
    public E_CHARA_TROOP Troop { get; protected set; }

    private HitSufferController<BulletController> m_BulletSuffer;
    private HitSufferController<CharaController> m_CharaSuffer;
    private HitSufferController<CharaController> m_CharaHit;
    private HitSufferController<BattleRealItemController> m_ItemHit;

    #endregion

    #region Get Set

    public BulletSetParam GetBulletSetParam()
    {
        return m_BulletSetParam;
    }

    /// <summary>
    /// いずれ消す
    /// </summary>
    /// <param name="param"></param>
    public void SetBulletSetParam(BulletSetParam param)
    {
        m_BulletSetParam = param;
    }

    #endregion

    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();

        m_BulletSuffer = new HitSufferController<BulletController>();
        m_CharaSuffer = new HitSufferController<CharaController>();
        m_CharaHit = new HitSufferController<CharaController>();
        m_ItemHit = new HitSufferController<BattleRealItemController>();
    }

    protected override void OnDestroyed()
    {
        m_ItemHit.OnFinalize();
        m_CharaHit.OnFinalize();
        m_CharaSuffer.OnFinalize();
        m_BulletSuffer.OnFinalize();
        m_ItemHit = null;
        m_CharaHit = null;
        m_CharaSuffer = null;
        m_BulletSuffer = null;
        base.OnDestroyed();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_BulletSuffer.OnEnter = OnEnterSufferBullet;
        m_BulletSuffer.OnStay = OnStaySufferBullet;
        m_BulletSuffer.OnExit = OnExitSufferBullet;

        m_CharaSuffer.OnEnter = OnEnterSufferChara;
        m_CharaSuffer.OnStay = OnStaySufferChara;
        m_CharaSuffer.OnExit = OnExitSufferChara;

        m_CharaHit.OnEnter = OnEnterHitChara;
        m_CharaHit.OnStay = OnStayHitChara;
        m_CharaHit.OnExit = OnExitHitChara;

        m_ItemHit.OnEnter = OnEnterHitItem;
        m_ItemHit.OnStay = OnStayHitItem;
        m_ItemHit.OnExit = OnExitHitItem;
    }

    public override void OnFinalize()
    {
        m_ItemHit.OnFinalize();
        m_CharaHit.OnFinalize();
        m_CharaSuffer.OnFinalize();
        m_BulletSuffer.OnFinalize();
        base.OnFinalize();
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
    public void Recover(float recover)
    {
        if (recover <= 0)
        {
            return;
        }

        m_NowHp = Mathf.Clamp(m_NowHp + recover, 0, m_MaxHp);
    }

    /// <summary>
    /// このキャラにダメージを与える。
    /// HPが0になった場合は死ぬ。
    /// </summary>
    public void Damage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        m_NowHp = Mathf.Clamp(m_NowHp - damage, 0, m_MaxHp);

        if (m_NowHp == 0)
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

    #region Impl IColliderProcess

    public override void ClearColliderFlag()
    {
        m_BulletSuffer.ClearUpdateFlag();
        m_CharaSuffer.ClearUpdateFlag();
        m_CharaHit.ClearUpdateFlag();
        m_ItemHit.ClearUpdateFlag();
    }

    public override void ProcessCollision()
    {
        m_BulletSuffer.ProcessCollision();
        m_CharaSuffer.ProcessCollision();
        m_CharaHit.ProcessCollision();
        m_ItemHit.ProcessCollision();
    }

    #endregion

    #region Suffer Bullet

    /// <summary>
    /// 他の弾から当てられた時の処理。
    /// </summary>
    /// <param name="attackBullet">他の弾</param>
    /// <param name="attackData">他の弾の衝突情報</param>
    /// <param name="targetData">このキャラの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void SufferBullet(BulletController attackBullet, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_BulletSuffer.Put(attackBullet, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {

    }

    protected virtual void OnStaySufferBullet(HitSufferData<BulletController> sufferData)
    {

    }

    protected virtual void OnExitSufferBullet(HitSufferData<BulletController> sufferData)
    {

    }

    #endregion

    #region Suffer Chara

    /// <summary>
    /// 他のキャラから当てられた時の処理。
    /// </summary>
    /// <param name="attackChara">他のキャラ</param>
    /// <param name="attackData">他のキャラの衝突情報</param>
    /// <param name="targetData">このキャラの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void SufferChara(CharaController attackChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaSuffer.Put(attackChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterSufferChara(HitSufferData<CharaController> sufferData)
    {
    }

    protected virtual void OnStaySufferChara(HitSufferData<CharaController> sufferData)
    {
    }

    protected virtual void OnExitSufferChara(HitSufferData<CharaController> sufferData)
    {
    }

    #endregion

    #region Hit Chara

    /// <summary>
    /// 他のキャラに当たった時の処理。
    /// </summary>
    /// <param name="targetChara">他のキャラ</param>
    /// <param name="attackData">このキャラの衝突情報</param>
    /// <param name="targetData">他のキャラの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void HitChara(CharaController targetChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaHit.Put(targetChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterHitChara(HitSufferData<CharaController> hitData)
    {

    }

    protected virtual void OnStayHitChara(HitSufferData<CharaController> hitData)
    {

    }

    protected virtual void OnExitHitChara(HitSufferData<CharaController> hitData)
    {

    }

    #endregion

    #region Hit Item

    /// <summary>
    /// 他のアイテムに当たった時の処理。
    /// </summary>
    /// <param name="targetItem">他のアイテム</param>
    /// <param name="attackData">このキャラの衝突情報</param>
    /// <param name="targetData">他のアイテムの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void HitItem(BattleRealItemController targetItem, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_ItemHit.Put(targetItem, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterHitItem(HitSufferData<BattleRealItemController> hitData)
    {

    }

    protected virtual void OnStayHitItem(HitSufferData<BattleRealItemController> hitData)
    {

    }

    protected virtual void OnExitHitItem(HitSufferData<BattleRealItemController> hitData)
    {

    }

    #endregion

    /// <summary>
    /// 複数の弾を拡散させたい時の拡散角度のリストを取得する。
    /// </summary>
    /// <param name="bulletNum">弾の個数</param>
    /// <param name="spreadAngle">弾同士の角度間隔</param>
    protected static List<float> GetBulletSpreadAngles(int bulletNum, float spreadAngle)
    {
        List<float> spreadAngles = new List<float>();

        if (bulletNum % 2 == 1)
        {
            spreadAngles.Add(0f);

            for (int i = 0; i < (bulletNum - 1) / 2; i++)
            {
                spreadAngles.Add(spreadAngle * (i + 1f));
                spreadAngles.Add(spreadAngle * -(i + 1f));
            }
        }
        else
        {
            for (int i = 0; i < bulletNum / 2; i++)
            {
                spreadAngles.Add(spreadAngle * (i + 0.5f));
                spreadAngles.Add(spreadAngle * -(i + 0.5f));
            }
        }

        return spreadAngles;
    }

    public float GetNowHP(){
        return m_NowHp;
    }

    public float GetMaxHP(){
        return m_MaxHp;
    }
}
