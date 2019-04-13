using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンドイベントのキャラの制御コンポーネント。
/// </summary>
public class CommandCharaController : BattleCommandObjectBase
{
    #region Field Inspector

    [Header("キャラの基礎パラメータ")]

    [SerializeField, Tooltip("キャラの所属")]
    private E_CHARA_TROOP m_Troop;

    [SerializeField, Tooltip("キャラが用いる弾の組み合わせ")]
    private CommandBulletSetParam m_BulletSetParam;

    [SerializeField, Tooltip("キャラの衝突情報")]
    private BattleObjectCollider m_Collider;

    [Header("キャラの基礎ステータス")]

    [SerializeField, Tooltip("キャラの現在HP")]
    private int m_NowHp;

    [SerializeField, Tooltip("キャラの最大HP")]
    private int m_MaxHp;

    #endregion



    #region Getter & Setter

    public E_CHARA_TROOP GetTroop()
    {
        return m_Troop;
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



    /// <summary>
    /// このキャラを回復する。
    /// </summary>
    public virtual void Recover(int recover)
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
    public virtual void Damage(int damage)
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

    /// <summary>
    /// このキャラが他のキャラに当たった場合のコールバック。
    /// </summary>
    public virtual void OnHitCharacter(CharaController chara)
    {

    }

    /// <summary>
    /// このキャラが他の弾に当たった場合のコールバック。
    /// </summary>
    public virtual void OnHitBullet(BulletController bullet)
    {

    }

    /// <summary>
    /// 他の弾がこのキャラに当たった場合のコールバック。
    /// </summary>
    public virtual void OnSuffer(BulletController bullet, ColliderData colliderData)
    {
        Damage(1);
    }

    /// <summary>
    /// 他のキャラがこのキャラに当たった場合のコールバック。
    /// </summary>
    /// <param name="hitChara">当った他のキャラ</param>
    /// <param name="hitData">他のキャラの当たったデータ</param>
    /// <param name="sufferData">このキャラの当たったデータ</param>
    public virtual void OnSufferChara(CharaController hitChara, ColliderData hitData, ColliderData sufferData)
    {

    }

    /// <summary>
    /// このキャラが他のキャラに当たった場合のコールバック。
    /// </summary>
    /// <param name="sufferChara">当った他のキャラ</param>
    /// <param name="hitData">このキャラの当たったデータ</param>
    /// <param name="sufferData">他のキャラの当たったデータ</param>
    public virtual void OnHitChara(CharaController sufferChara, ColliderData hitData, ColliderData sufferData)
    {

    }

    /// <summary>
    /// このキャラがアイテムに当たった場合のコールバック。
    /// </summary>
    /// <param name="sufferItem">当ったアイテム</param>
    /// <param name="hitData">このキャラの当たったデータ</param>
    /// <param name="sufferData">アイテムの当たったデータ</param>
    public virtual void OnHitItem(ItemController sufferItem, ColliderData hitData, ColliderData sufferData)
    {

    }

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
