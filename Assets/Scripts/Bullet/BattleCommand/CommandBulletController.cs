using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// コマンドイベントの全ての弾オブジェクトの基礎クラス。
/// </summary>
public class CommandBulletController : BattleHackingObjectBase
{
    #region Field Inspector

    /// <summary>
    /// この弾のグループ名。
    /// 同じ名前同士の弾は、違うキャラから生成されたものであっても、プールから再利用される。
    /// </summary>
    [SerializeField]
    private string m_BulletGroupId = "Default Bullet";

    #endregion



    #region Field

    /// <summary>
    /// この弾がプレイヤー側と敵側のどちらに属しているか。
    /// 同じ値同士を持つ弾やキャラには被弾しない。
    /// </summary>
    private E_CHARA_TROOP m_Troop;

    /// <summary>
    /// 弾を発射したキャラ。
    /// </summary>
    private CommandCharaController m_BulletOwner;

    /// <summary>
    /// 弾の標的となっているキャラ。
    /// </summary>
    private CommandCharaController m_Target;

    /// <summary>
    /// 発射したキャラの何番目の弾か。
    /// </summary>
    private int m_BulletIndex;

    /// <summary>
    /// この弾のBulletParam。
    /// BulletParamで制御されていない場合、この弾はプログラムで直接制御しなければならない。
    /// </summary>
    private BulletParam m_BulletParam;

    /// <summary>
    /// この弾のBulletParamのインデックス。
    /// </summary>
    private int m_BulletParamIndex;

    /// <summary>
    /// この弾のOrbitalParam。
    /// 何も指定しない場合、この弾はプログラムで直接制御しなければならない。
    /// </summary>
    private BulletOrbitalParam m_OrbitalParam;

    /// <summary>
    /// この弾のOrbitalParamのインデックス。
    /// </summary>
    private int m_OrbitalParamIndex;

    /// <summary>
    /// この弾の状態。
    /// </summary>
    [SerializeField]
    private E_POOLED_OBJECT_CYCLE m_Cycle;

    /// <summary>
    /// この弾が何秒生存しているか。
    /// </summary>
    private float m_NowLifeTime;

    /// <summary>
    /// この弾が1秒間にどれくらい回転するか。
    /// </summary>
    private Vector3 m_NowDeltaRotation;

    /// <summary>
    /// この弾が1秒間にどれくらいスケーリングするか。
    /// </summary>
    private Vector3 m_NowDeltaScale;

    /// <summary>
    /// この弾の攻撃力。
    /// </summary>
    private float m_NowDamage;

    /// <summary>
    /// この弾が1秒間にどれくらい移動するか。
    /// </summary>
    private float m_NowSpeed;

    /// <summary>
    /// この弾が1秒間にどれくらい加速するか。
    /// </summary>
    private float m_NowAccel;

    /// <summary>
    /// この弾が発射された瞬間にターゲットに設定されているキャラの方向を向くかどうか。
    /// </summary>
    private bool m_IsSearch;

    /// <summary>
    /// この弾が1秒間にどれくらいターゲットに設定されているキャラを追従するか。
    /// </summary>
    private float m_NowLerp;

    private HitSufferController<CommandCharaController> m_CharaHit;

    #endregion



    #region Getter & Setter

    /// <summary>
    /// この弾のグループ名を取得する。
    /// 同じ名前同士の弾は、違うキャラから生成されたものであっても、プールから再利用される。
    /// </summary>
    public string GetBulletGroupId()
    {
        return m_BulletGroupId;
    }

    /// <summary>
    /// この弾がプレイヤー側と敵側のどちらに属しているか。
    /// 同じ値同士を持つ弾やキャラには被弾しない。
    /// </summary>
    public E_CHARA_TROOP GetTroop()
    {
        return m_Troop;
    }

    /// <summary>
    /// この弾がプレイヤー側と敵側のどちらに属しているかを設定する。
    /// </summary>
    public void SetTroop(E_CHARA_TROOP troop)
    {
        m_Troop = troop;
    }

    /// <summary>
    /// この弾を発射したキャラを取得する。
    /// </summary>
    public CommandCharaController GetBulletOwner()
    {
        return m_BulletOwner;
    }

    /// <summary>
    /// この弾の標的となっているキャラを取得する。
    /// </summary>
    public CommandCharaController GetTarget()
    {
        return m_Target;
    }

    /// <summary>
    /// この弾の標的となっているキャラを設定する。
    /// </summary>
    public void SetTarget(CommandCharaController target)
    {
        m_Target = target;
    }

    /// <summary>
    /// 発射したキャラの何番目の弾かを取得する。
    /// </summary>
    public int GetBulletIndex()
    {
        return m_BulletIndex;
    }

    /// <summary>
    /// この弾のBulletParam。
    /// BulletParamで制御されていない場合、この弾はプログラムで直接制御しなければならない。
    /// </summary>
    public BulletParam GetBulletParam()
    {
        return m_BulletParam;
    }

    /// <summary>
    /// この弾のBulletParamのインデックス。
    /// </summary>
    public int GetBulletParamIndex()
    {
        return m_BulletParamIndex;
    }

    /// <summary>
    /// この弾のOrbialParam。
    /// 何も指定しない場合、この弾はプログラムで直接制御しなければならない。
    /// </summary>
    public BulletOrbitalParam GetOrbitalParam()
    {
        return m_OrbitalParam;
    }

    /// <summary>
    /// この弾のOrbitalParamのインデックス。
    /// </summary>
    public int GetOrbitalParamIndex()
    {
        return m_OrbitalParamIndex;
    }

    /// <summary>
    /// この弾の状態を取得する。
    /// </summary>
    public E_POOLED_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    /// <summary>
    /// この弾の状態を設定する。
    /// </summary>
    public void SetCycle(E_POOLED_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    /// <summary>
    /// この弾が何秒生存しているかを取得する。
    /// </summary>
    public float GetNowLifeTime()
    {
        return m_NowLifeTime;
    }

    /// <summary>
    /// この弾のライフタイムを設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    protected void SetNowLifeTime(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowLifeTime = GetRelativeValue(relative, GetNowLifeTime(), value);
    }

    /// <summary>
    /// この弾のローカル座標を取得する。
    /// </summary>
    public Vector3 GetPosition()
    {
        return transform.localPosition;
    }

    /// <summary>
    /// この弾のローカル座標を設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetPosition(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        transform.localPosition = GetRelativeValue(relative, GetPosition(), value);
    }

    /// <summary>
    /// この弾のローカルオイラー回転を取得する。
    /// </summary>
    public Vector3 GetRotation()
    {
        return transform.localEulerAngles;
    }

    /// <summary>
    /// この弾のローカルオイラー回転を設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetRotation(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        transform.localEulerAngles = GetRelativeRotateValue(relative, GetRotation(), value);
    }

    /// <summary>
    /// この弾のローカルスケールを取得する。
    /// </summary>
    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    /// <summary>
    /// この弾のローカルスケールを設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetScale(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        transform.localScale = GetRelativeValue(relative, GetScale(), value);
    }

    /// <summary>
    /// この弾が1秒間にどれくらい回転するかを取得する。
    /// </summary>
    public Vector3 GetNowDeltaRotation()
    {
        return m_NowDeltaRotation;
    }

    /// <summary>
    /// この弾が1秒間にどれくらい回転するかを設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetNowDeltaRotation(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowDeltaRotation = GetRelativeValue(relative, GetNowDeltaRotation(), value);
    }

    /// <summary>
    /// この弾が1秒間にどれくらいスケーリングするかを取得する。
    /// </summary>
    public Vector3 GetNowDeltaScale()
    {
        return m_NowDeltaScale;
    }

    /// <summary>
    /// この弾が1秒間にどれくらいスケーリングするかを設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetNowDeltaScale(Vector3 value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowDeltaScale = GetRelativeValue(relative, GetNowDeltaScale(), value);
    }

    /// <summary>
    /// この弾の攻撃力を取得する。
    /// </summary>
    public float GetNowDamage()
    {
        return m_NowDamage;
    }

    /// <summary>
    /// この弾の攻撃力を設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetNowDamage(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowDamage = GetRelativeValue(relative, GetNowDamage(), value);
    }

    /// <summary>
    /// この弾が1秒間にどれくらい移動するかを取得する。
    /// </summary>
    public float GetNowSpeed()
    {
        return m_NowSpeed;
    }

    /// <summary>
    /// この弾が1秒間にどれくらい移動するかを取得する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetNowSpeed(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowSpeed = GetRelativeValue(relative, GetNowSpeed(), value);
    }

    /// <summary>
    /// この弾が1秒間にどれくらい加速するかを取得する。
    /// </summary>
    public float GetNowAccel()
    {
        return m_NowAccel;
    }

    /// <summary>
    /// この弾が1秒間にどれくらい加速するかを設定する。
    /// </summary>
    /// <param name="value">設定する値</param>
    /// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
    public void SetNowAccel(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowAccel = GetRelativeValue(relative, GetNowAccel(), value);
    }

    /// <summary>
    /// この弾が発射された瞬間にターゲットに設定されているキャラの方向を向くかどうかを取得する。
    /// </summary>
    public bool IsSearch()
    {
        return m_IsSearch;
    }

    public void SetSearch(bool value)
    {
        m_IsSearch = value;
    }

    /// <summary>
    /// この弾が1秒間にどれくらいターゲットに設定されているキャラを追従するかを取得する。
    /// </summary>
    public float GetNowLerp()
    {
        return m_NowLerp;
    }

    /// <summary>
    /// この弾が1秒間にどれくらいターゲットに設定されているキャラを追従するかを設定する。
    /// </summary>
    public void SetNowLerp(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE)
    {
        m_NowLerp = GetRelativeValue(relative, GetNowLerp(), value);
    }

    #endregion



    /// <summary>
    /// 弾を生成する。
    /// </summary>
    /// <param name="bulletOwner">弾を発射させるキャラ</param>
    private static CommandBulletController CreateBullet(CommandCharaController bulletOwner)
    {
        if (bulletOwner == null)
        {
            return null;
        }

        // プレハブを取得
        var bulletPrefab = bulletOwner.GetBulletPrefab();

        if (bulletPrefab == null)
        {
            return null;
        }

        // プールから弾を取得
        var bullet = BattleHackingBulletManager.Instance.GetPoolingBullet(bulletPrefab);

        if (bullet == null)
        {
            return null;
        }

        // 座標を設定
        bullet.SetPosition(bulletOwner.transform.localPosition);

        // 回転を設定
        bullet.SetRotation(bulletOwner.transform.localEulerAngles);

        // スケールを設定
        bullet.SetScale(bulletPrefab.transform.localScale);

        bullet.ResetBulletLifeTime();
        bullet.ResetBulletParam();

        bullet.m_BulletOwner = bulletOwner;
        bullet.SetTroop(bulletOwner.GetTroop());
        bullet.m_BulletParam = null;
        bullet.m_BulletIndex = 0;
        bullet.m_BulletParamIndex = 0;
        bullet.m_OrbitalParamIndex = -1;

        return bullet;
    }

    /// <summary>
    /// 指定したキャラの最初に登録されている弾をBulletParamの指定なしで発射する。
    /// 弾は指定したキャラの位置に生成する。
    /// 発射後はプログラムで制御しないと動かないので注意。
    /// </summary>
    /// <param name="bulletOwner">弾を発射させるキャラ</param>
    /// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
    public static CommandBulletController ShotBulletWithoutBulletParam(CommandCharaController bulletOwner, bool isCheck = true)
    {
        var bullet = CreateBullet(bulletOwner);

        if (bullet == null)
        {
            return null;
        }

        if (isCheck)
        {
            BattleHackingBulletManager.Instance.CheckStandbyBullet(bullet);
        }

        return bullet;
    }

    /// <summary>
    /// 指定したキャラの最初に登録されている弾を最初に登録されているBulletParamのデフォルトのOrbitalParamで弾を発射する。
    /// 弾は指定したキャラの位置に生成する。
    /// </summary>
    /// <param name="bulletOwner">弾を発射させるキャラ</param>
    /// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
    public static CommandBulletController ShotBullet(CommandCharaController bulletOwner, bool isCheck = true)
    {
        var bullet = CreateBullet(bulletOwner);

        if (bullet == null)
        {
            return null;
        }

        // BulletParamを取得
        var bulletParam = bulletOwner.GetBulletParam();

        if (bulletParam != null)
        {
            // 軌道を設定
            bullet.ChangeOrbital(bulletParam.GetOrbitalParam());
        }

        bullet.m_BulletParam = bulletParam;

        if (isCheck)
        {
            BattleHackingBulletManager.Instance.CheckStandbyBullet(bullet);
        }

        return bullet;
    }

    /// <summary>
    /// 指定したパラメータを用いて弾を生成する。
    /// </summary>
    /// <param name="shotParam">弾を発射させるパラメータ</param>
    private static CommandBulletController CreateBullet(CommandBulletShotParam shotParam)
    {
        var bulletOwner = shotParam.BulletOwner;

        if (bulletOwner == null)
        {
            return null;
        }

        // プレハブを取得
        var bulletPrefab = bulletOwner.GetBulletPrefab(shotParam.BulletIndex);

        if (bulletPrefab == null)
        {
            return null;
        }

        // プールから弾を取得
        var bullet = BattleHackingBulletManager.Instance.GetPoolingBullet(bulletPrefab);

        if (bullet == null)
        {
            return null;
        }

        // 座標を設定
        bullet.SetPosition(shotParam.Position != null ? (Vector3)shotParam.Position : bulletOwner.transform.localPosition);

        // 回転を設定
        bullet.SetRotation(shotParam.Rotation != null ? (Vector3)shotParam.Rotation : bulletOwner.transform.localEulerAngles);

        // スケールを設定
        bullet.SetScale(shotParam.Scale != null ? (Vector3)shotParam.Scale : bulletPrefab.transform.localScale);

        bullet.ResetBulletLifeTime();
        bullet.ResetBulletParam();

        bullet.m_BulletOwner = bulletOwner;
        bullet.SetTroop(bulletOwner.GetTroop());
        bullet.m_BulletParam = null;
        bullet.m_BulletIndex = 0;
        bullet.m_BulletParamIndex = 0;
        bullet.m_OrbitalParamIndex = -1;

        return bullet;
    }

    /// <summary>
    /// 指定したパラメータを用いて弾をBulletParamの指定なしで発射する。
    /// 発射後はプログラムで制御しないと動かないので注意。
    /// </summary>
    /// <param name="bulletOwner">弾を発射させるキャラ</param>
    /// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
    public static CommandBulletController ShotBulletWithoutBulletParam(CommandBulletShotParam shotParam, bool isCheck = true)
    {
        var bullet = CreateBullet(shotParam);

        if (bullet == null)
        {
            return null;
        }

        if (isCheck)
        {
            BattleHackingBulletManager.Instance.CheckStandbyBullet(bullet);
        }

        return bullet;
    }

    /// <summary>
    /// 指定したパラメータを用いて弾を発射する。
    /// </summary>
    /// <param name="shotParam">発射時のパラメータ</param>
    /// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
    public static CommandBulletController ShotBullet(CommandBulletShotParam shotParam, bool isCheck = true)
    {
        var bullet = CreateBullet(shotParam);

        if (bullet == null)
        {
            return null;
        }

        // BulletParamを取得
        var bulletParam = shotParam.BulletOwner.GetBulletParam(shotParam.BulletParamIndex);
        bullet.m_BulletParamIndex = shotParam.BulletParamIndex;

        if (bulletParam != null)
        {
            // 軌道を設定
            bullet.ChangeOrbital(bulletParam.GetOrbitalParam(shotParam.OrbitalIndex));
            bullet.m_OrbitalParamIndex = shotParam.OrbitalIndex;
        }

        bullet.m_BulletParam = bulletParam;

        if (isCheck)
        {
            BattleHackingBulletManager.Instance.CheckStandbyBullet(bullet);
        }

        return bullet;
    }

    /// <summary>
    /// この弾の軌道情報を上書きする。
    /// </summary>
    public virtual void ChangeOrbital(BulletOrbitalParam orbitalParam)
    {
        m_OrbitalParam = orbitalParam;

        // ターゲットの上書き
        if (m_OrbitalParam.Target == E_ATTACK_TARGET.ENEMY)
        {
            m_Target = GetNearestEnemy();
        }
        else if (m_OrbitalParam.Target == E_ATTACK_TARGET.OWNER)
        {
            m_Target = m_BulletOwner;
        }


        // 各種パラメータの上書き
        SetPosition(m_OrbitalParam.Position, m_OrbitalParam.PositionRelative);
        SetRotation(m_OrbitalParam.Rotation, m_OrbitalParam.RotationRelative);
        SetScale(m_OrbitalParam.Scale, m_OrbitalParam.ScaleRelative);
        SetNowDeltaRotation(m_OrbitalParam.DeltaRotation, m_OrbitalParam.DeltaRotationRelative);
        SetNowDeltaScale(m_OrbitalParam.DeltaScale, m_OrbitalParam.DeltaScaleRelative);
        SetNowDamage(m_OrbitalParam.Damage, m_OrbitalParam.DamageRelative);
        SetNowSpeed(m_OrbitalParam.Speed, m_OrbitalParam.SpeedRelative);
        SetNowAccel(m_OrbitalParam.Accel, m_OrbitalParam.AccelRelative);
        SetSearch(m_OrbitalParam.IsSearch);
        SetNowLerp(m_OrbitalParam.Lerp, m_OrbitalParam.LerpRelative);

        // オプションパラメータは後で実装

        if (m_IsSearch)
        {
            LookAtTarget();
        }
    }

    /// <summary>
    /// この弾を破棄する。
    /// </summary>
    public virtual void DestroyBullet()
    {
        DestroyAllTimer();

        if (m_Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            BattleHackingBulletManager.Instance.CheckPoolBullet(this);
        }
    }

    /// <summary>
    /// Searchがtrueかつターゲットキャラが設定されている時、ターゲットキャラの方向を向く。
    /// </summary>
    public void LookAtTarget()
    {
        if (m_Target != null)
        {
            transform.LookAt(m_Target.transform);
        }
    }

    protected Vector3 GetRelativeValue(E_RELATIVE relative, Vector3 baseValue, Vector3 relativeValue)
    {
        if (relative == E_RELATIVE.RELATIVE)
        {
            return baseValue + relativeValue;
        }
        else
        {
            return relativeValue;
        }
    }

    protected Vector3 GetRelativeRotateValue(E_RELATIVE relative, Vector3 baseValue, Vector3 relativeValue)
    {
        if (relative == E_RELATIVE.RELATIVE)
        {
            var value = baseValue + relativeValue;

            if (relativeValue.x != 0)
                value.x = value.x % 360 + 360;

            if (relativeValue.y != 0)
                value.y = value.y % 360 + 360;

            if (relativeValue.z != 0)
                value.z = value.z % 360 + 360;

            return value;
        }
        else
        {
            return relativeValue;
        }
    }

    protected float GetRelativeValue(E_RELATIVE relative, float baseValue, float relativeValue)
    {
        if (relative == E_RELATIVE.RELATIVE)
        {
            return baseValue + relativeValue;
        }
        else
        {
            return relativeValue;
        }
    }

    /// <summary>
    /// この弾の最も近くにいる敵を探し出す。
    /// </summary>
    public CommandCharaController GetNearestEnemy()
    {
        if (m_Troop == E_CHARA_TROOP.ENEMY)
        {
            return BattleHackingPlayerManager.Instance.Player;
        }
        else
        {
            List<BattleHackingEnemyController> enemies = BattleHackingEnemyManager.Instance.Enemies;
            CommandCharaController nearestEnemy = null;
            float minSqrDist = float.MaxValue;

            foreach (var enemy in enemies)
            {
                float sqrDist = (transform.position - enemy.transform.position).sqrMagnitude;

                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy;
        }
    }

    /// <summary>
    /// 弾の残存時間をリセットする。
    /// </summary>
    public void ResetBulletLifeTime()
    {
        m_NowLifeTime = 0;
    }

    /// <summary>
    /// 全ての軌道用のパラメータをリセットする。
    /// </summary>
    public void ResetBulletParam()
    {
        m_NowDeltaRotation = Vector3.zero;
        m_NowDeltaScale = Vector3.zero;

        m_NowDamage = 0;

        m_NowSpeed = 0;
        m_NowAccel = 0;

        m_IsSearch = false;
        m_NowLerp = 0;
    }


    #region Game Cycle

    protected override void OnAwake()
    {
        base.OnAwake();
        m_CharaHit = new HitSufferController<CommandCharaController>();
    }

    protected override void OnDestroyed()
    {
        m_CharaHit.OnFinalize();
        m_CharaHit = null;
        base.OnDestroyed();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CharaHit.OnEnter = OnEnterHitChara;
        m_CharaHit.OnStay = OnStayHitChara;
        m_CharaHit.OnExit = OnExitHitChara;
    }

    public override void OnFinalize()
    {
        m_CharaHit.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        if (m_BulletParam == null)
        {
            return;
        }

        SetRotation(GetNowDeltaRotation() * Time.deltaTime, E_RELATIVE.RELATIVE);
        SetScale(GetNowDeltaScale() * Time.deltaTime, E_RELATIVE.RELATIVE);

        SetNowSpeed(GetNowAccel() * Time.deltaTime, E_RELATIVE.RELATIVE);

        if (m_Target != null)
        {
            Vector3 targetDeltaPos = m_Target.transform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, targetDeltaPos.normalized, m_NowLerp);
        }

        var speed = GetNowSpeed() * Time.deltaTime;
        SetPosition(transform.forward * speed, E_RELATIVE.RELATIVE);

        SetNowLifeTime(Time.deltaTime, E_RELATIVE.RELATIVE);

        if (GetNowLifeTime() > GetBulletParam().LifeTime)
        {
            DestroyBullet();
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (BattleHackingBulletManager.Instance.IsOutOfBulletField(this))
        {
            DestroyBullet();
        }
    }

    #endregion

    #region Impl IColliderProcess

    public override void ClearColliderFlag()
    {
        m_CharaHit.ClearUpdateFlag();
    }

    public override void ProcessCollision()
    {
        m_CharaHit.ProcessCollision();
    }

    #endregion

    #region Hit Chara

    /// <summary>
    /// 他のキャラに当たった時の処理。
    /// </summary>
    /// <param name="targetChara">他のキャラ</param>
    /// <param name="attackData">この弾の衝突情報</param>
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
}
