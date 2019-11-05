using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントのキャラの制御コンポーネント。
/// </summary>
public class CommandCharaController : BattleHackingObjectBase
{
    [SerializeField]
    private CommandBulletSetParam m_BulletSetParam;

    #region Field

    public E_CHARA_TROOP Troop { get; protected set; }
    public float NowHp { get; private set; }
    public float MaxHp { get; private set; }

    private HitSufferController<CommandBulletController> m_BulletSuffer;
    private HitSufferController<CommandCharaController> m_CharaSuffer;
    private HitSufferController<CommandCharaController> m_CharaHit;

    #endregion

    #region Getter & Setter

    public E_CHARA_TROOP GetTroop()
    {
        return Troop;
    }

    public CommandBulletSetParam GetBulletSetParam()
    {
        return m_BulletSetParam;
    }

    public void SetBulletSetParam(CommandBulletSetParam param)
    {
        m_BulletSetParam = param;
    }

    /// <summary>
    /// キャラが保持するBulletParamの個数を取得する。
    /// </summary>
    public int GetBulletParamsCount()
    {
        if (GetBulletSetParam().GetBulletParams() == null)
        {
            return -1;
        }

        return GetBulletSetParam().GetBulletParams().Length;
    }

    /// <summary>
    /// 指定したインデックスのBulletParamを取得する。
    /// </summary>
    public BulletParam GetBulletParam(int bulletParamIndex = 0)
    {
        int paramCount = GetBulletParamsCount();

        if (GetBulletSetParam().GetBulletParams() == null || paramCount < 1)
        {
            return null;
        }

        if (bulletParamIndex < 0 || bulletParamIndex >= paramCount)
        {
            bulletParamIndex = 0;
        }

        return GetBulletSetParam().GetBulletParams()[bulletParamIndex];
    }

    /// <summary>
    /// キャラが保持する弾プレハブの個数を取得する。
    /// </summary>
    public int GetBulletPrefabsCount()
    {
        if (GetBulletSetParam().GetBulletPrefabs() == null)
        {
            return -1;
        }

        return GetBulletSetParam().GetBulletPrefabs().Length;
    }

    /// <summary>
    /// 指定したインデックスの弾のプレハブを取得する。
    /// </summary>
    public CommandBulletController GetBulletPrefab(int bulletIndex = 0)
    {
        int prefabCount = GetBulletPrefabsCount();

        if (GetBulletSetParam().GetBulletPrefabs() == null || prefabCount < 1)
        {
            return null;
        }

        if (bulletIndex < 0 || bulletIndex >= prefabCount)
        {
            bulletIndex = 0;
        }

        return GetBulletSetParam().GetBulletPrefabs()[bulletIndex];
    }

    #endregion

    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();

        m_BulletSuffer = new HitSufferController<CommandBulletController>();
        m_CharaSuffer = new HitSufferController<CommandCharaController>();
        m_CharaHit = new HitSufferController<CommandCharaController>();
    }

    protected override void OnDestroyed()
    {
        m_CharaHit.OnFinalize();
        m_CharaSuffer.OnFinalize();
        m_BulletSuffer.OnFinalize();
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
    }

    public override void OnFinalize()
    {
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
        MaxHp = NowHp = hp;
    }

    /// <summary>
    /// このキャラを回復する。
    /// </summary>
    public void Recover(int recover)
    {
        if (recover <= 0)
        {
            return;
        }

        NowHp = Mathf.Clamp(NowHp + recover, 0, MaxHp);

        OnRecover();
    }

    protected virtual void OnRecover()
    {

    }

    /// <summary>
    /// このキャラにダメージを与える。
    /// HPが0になった場合は死ぬ。
    /// </summary>
    public void Damage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        NowHp = Mathf.Clamp(NowHp - damage, 0, MaxHp);

        OnDamage();

        if (NowHp == 0)
        {
            Dead();
        }
    }

    protected virtual void OnDamage()
    {

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
    }

    public override void ProcessCollision()
    {
        m_BulletSuffer.ProcessCollision();
        m_CharaSuffer.ProcessCollision();
        m_CharaHit.ProcessCollision();
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
    public void SufferBullet(CommandBulletController attackBullet, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_BulletSuffer.Put(attackBullet, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterSufferBullet(HitSufferData<CommandBulletController> sufferData)
    {

    }

    protected virtual void OnStaySufferBullet(HitSufferData<CommandBulletController> sufferData)
    {

    }

    protected virtual void OnExitSufferBullet(HitSufferData<CommandBulletController> sufferData)
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
    public void SufferChara(CommandCharaController attackChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaSuffer.Put(attackChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterSufferChara(HitSufferData<CommandCharaController> sufferData)
    {

    }

    protected virtual void OnStaySufferChara(HitSufferData<CommandCharaController> sufferData)
    {

    }

    protected virtual void OnExitSufferChara(HitSufferData<CommandCharaController> sufferData)
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
    public void HitChara(CommandCharaController targetChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaHit.Put(targetChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterHitChara(HitSufferData<CommandCharaController> hitData)
    {

    }

    protected virtual void OnStayHitChara(HitSufferData<CommandCharaController> hitData)
    {

    }

    protected virtual void OnExitHitChara(HitSufferData<CommandCharaController> hitData)
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
}
