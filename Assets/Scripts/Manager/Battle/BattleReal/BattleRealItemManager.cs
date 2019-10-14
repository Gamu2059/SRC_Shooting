using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// アイテムを管理するマネージャ。
/// </summary>
public class BattleRealItemManager : ControllableObject
{
    public const string HOLDER_NAME = "[ItemHolder]";

    #region Field Inspector

    /// <summary>
    /// アイテムのプレハブ群。
    /// </summary>
    [SerializeField]
    private ItemController[] m_ItemPrefabs;

    /// <summary>
    /// アイテムに適用する軌道パラメータ。
    /// </summary>
    [SerializeField]
    private BulletOrbitalParam m_ItemOrbitalParam;

    /// <summary>
    /// アイテムの吸引強度。
    /// </summary>
    [SerializeField]
    private float m_ItemAttractRate;

    #endregion

    #region Field

    /// <summary>
    /// アイテムオブジェクトを保持する。
    /// </summary>
    private Transform m_ItemHolder;

    /// <summary>
    /// STANDBY状態のアイテムを保持するリスト。
    /// </summary>
    private List<ItemController> m_StandbyItems;

    /// <summary>
    /// UPDATE状態のアイテムを保持するリスト。
    /// </summary>
    private List<ItemController> m_UpdateItems;

    /// <summary>
    /// POOL状態のアイテムを保持するリスト。
    /// </summary>
    private List<ItemController> m_PoolItems;

    /// <summary>
    /// POOL状態に遷移するアイテムのリスト。
    /// </summary>
    private List<ItemController> m_GotoPoolItems;

    #endregion

    private Dictionary<E_ITEM_TYPE, ItemController> m_ItemPrefabCache;


    #region Get

    /// <summary>
    /// アイテムの吸引強度を取得する。
    /// </summary>
    public float GetItemAttractRate()
    {
        return m_ItemAttractRate;
    }

    /// <summary>
    /// UPDATE状態のアイテムを保持するリストを取得する。
    /// </summary>
    public List<ItemController> GetUpdateItems()
    {
        return m_UpdateItems;
    }

    #endregion

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StandbyItems = new List<ItemController>();
        m_UpdateItems = new List<ItemController>();
        m_PoolItems = new List<ItemController>();
        m_GotoPoolItems = new List<ItemController>();

        m_ItemPrefabCache = new Dictionary<E_ITEM_TYPE, ItemController>(); ;
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        m_StandbyItems.Clear();
        m_UpdateItems.Clear();
        m_PoolItems.Clear();
    }

    public override void OnStart()
    {
        base.OnStart();

        //if (BattleRealStageManager.Instance != null && BattleRealStageManager.Instance.GetItemHolder() != null)
        //{
        //    m_ItemHolder = BattleRealStageManager.Instance.GetItemHolder().transform;
        //}
        //else if (m_ItemHolder == null)
        //{
        //    var obj = new GameObject(HOLDER_NAME);
        //    obj.transform.position = Vector3.zero;
        //    m_ItemHolder = obj.transform;
        //}
    }

    public override void OnUpdate()
    {
        // Start処理
        foreach (var item in m_UpdateItems)
        {
            if (item == null)
            {
                continue;
            }

            item.OnStart();
        }

        GotoUpdateFromStandby();

        // Update処理
        foreach (var item in m_UpdateItems)
        {
            if (item == null)
            {
                continue;
            }

            item.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var item in m_UpdateItems)
        {
            if (item == null)
            {
                continue;
            }

            item.OnLateUpdate();
        }

        GotoPoolFromUpdate();
    }



    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateFromStandby()
    {
        foreach (var item in m_StandbyItems)
        {
            if (item == null)
            {
                continue;
            }
            else if (item.GetCycle() != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckPoolItem(item);
            }

            item.SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);
            m_UpdateItems.Add(item);
        }

        m_StandbyItems.Clear();
    }

    /// <summary>
    /// POOL状態にする。
    /// </summary>
    private void GotoPoolFromUpdate()
    {
        int count = m_GotoPoolItems.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var item = m_GotoPoolItems[idx];
            item.OnFinalize();
            item.SetCycle(E_POOLED_OBJECT_CYCLE.POOLED);
            item.gameObject.SetActive(false);
            m_GotoPoolItems.RemoveAt(idx);
            m_UpdateItems.Remove(item);
            m_PoolItems.Add(item);
        }

        m_GotoPoolItems.Clear();
    }

    /// <summary>
    /// アイテムをSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyItem(ItemController item)
    {
        if (item == null || !m_PoolItems.Contains(item))
        {
            Debug.LogError("指定されたアイテムを追加できませんでした。");
            return;
        }

        m_PoolItems.Remove(item);
        m_StandbyItems.Add(item);
        item.gameObject.SetActive(true);
        item.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE);
        item.OnInitialize();
    }

    /// <summary>
    /// 指定したアイテムを制御から外すためにチェックする。
    /// </summary>
    public void CheckPoolItem(ItemController item)
    {
        if (item == null || m_GotoPoolItems.Contains(item))
        {
            Debug.LogError("指定したアイテムを削除できませんでした。");
            return;
        }

        item.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        m_GotoPoolItems.Add(item);
    }

    /// <summary>
    /// プールからアイテムを取得する。
    /// 足りなければ生成する。
    /// </summary>
    /// <param name="itemPrefab">取得や生成の情報源となるアイテムのプレハブ</param>
    public ItemController GetPoolingItem(ItemController itemPrefab)
    {
        if (itemPrefab == null)
        {
            return null;
        }

        E_ITEM_TYPE itemType = itemPrefab.GetItemType();
        ItemController item = null;

        foreach (var i in m_PoolItems)
        {
            if (i != null && i.GetItemType() == itemType)
            {
                item = i;
                break;
            }
        }

        if (item == null)
        {
            item = GameObject.Instantiate(itemPrefab);
            item.transform.SetParent(m_ItemHolder);
            m_PoolItems.Add(item);
        }

        return item;
    }

    /// <summary>
    /// アイテムの種類を指定してアイテムプレハブを取得する。
    /// なければnullを返す。
    /// </summary>
    public ItemController GetItemPrefabFromItemType(E_ITEM_TYPE itemType)
    {
        // キャッシュされていれば、それを返す
        if (m_ItemPrefabCache.ContainsKey(itemType))
        {
            var item = m_ItemPrefabCache[itemType];
            if (item != null)
            {
                return item;
            }
        }

        foreach (var item in m_ItemPrefabs)
        {
            if (item == null)
            {
                continue;
            }

            if (item.GetItemType() == itemType)
            {
                m_ItemPrefabCache.Add(itemType, item);
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// 指定した座標から指定した情報でアイテムを生成する。
    /// </summary>
    /// 
    /// <param name="position">生成座標</param>
    /// <param name="param">アイテムの生成情報</param>
    public void CreateItem(Vector3 position, ItemCreateParam param)
    {
        if (param.ItemSpreadParams == null || param.ItemSpreadParams.Length < 1)
        {
            return;
        }

        foreach (var spreadParam in param.ItemSpreadParams)
        {
            var prefab = GetItemPrefabFromItemType(spreadParam.ItemType);
            if (prefab == null)
            {
                continue;
            }

            var item = GetPoolingItem(prefab);
            item.transform.SetParent(m_ItemHolder);

            if (spreadParam.ItemType == param.CenterCreateItemType)
            {
                item.SetPosition(position);
            }
            else
            {
                float r = spreadParam.SpreadRadius;
                float x = Random.Range(-r, r);
                float zR = Mathf.Sqrt(r * r - x * x);
                float z = Random.Range(-zR, zR);
                item.SetPosition(position + new Vector3(x, 0, z));
            }

            item.ChangeOrbital(m_ItemOrbitalParam);
            CheckStandbyItem(item);
        }
    }

    /// <summary>
    /// 全てのアイテムを吸引状態にする。
    /// </summary>
    public void AttractAllItem()
    {
        foreach (var item in m_UpdateItems)
        {
            item.AttractPlayer();
        }
    }

    public void OnAttractAction(E_INPUT_STATE state)
    {
        AttractAllItem();
    }
}
