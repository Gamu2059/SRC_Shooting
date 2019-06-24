using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム生成パラメータに変換するクラス。
/// </summary>
public class ItemCreateParamTranslator
{
    /// <summary>
    /// 文字列からアイテム生成パラメータに変換する。
    /// </summary>
    public static ItemCreateParam TranslateFromString(string paramData)
    {
        var param = new ItemCreateParam();

        if (paramData == null)
        {
            return param;
        }

        string[] paramDataArray = paramData.Split(';');

        List<ItemCreateParam.ItemSpreadParam> spreadParams = null;

        foreach(var data in paramDataArray)
        {
            string[] pArray = data.Trim().Split(':');

            if (pArray.Length != 3)
            {
                continue;
            }

            string type = pArray[0].Trim();
            string num = pArray[1].Trim();
            string radius = pArray[2].Trim();

            int numValue = 0;
            float radiusValue = 0;

            if (!IsVaridItemType(type) || !int.TryParse(num, out numValue) || !float.TryParse(radius, out radiusValue))
            {
                continue;
            }

            var spreadParam = new ItemCreateParam.ItemSpreadParam();
            spreadParam.ItemType = GetItemType(type);
            spreadParam.ItemNum = numValue;
            spreadParam.SpreadRadius = radiusValue;

            if (spreadParams == null)
            {
                spreadParams = new List<ItemCreateParam.ItemSpreadParam>();
            }

            spreadParams.Add(spreadParam);
        }

        if (spreadParams == null || spreadParams.Count < 1)
        {
            return param;
        }

        param.ItemSpreadParams = spreadParams.ToArray();
        return param;
    }

    private static bool IsVaridItemType(string type)
    {
        switch(type)
        {
            case "SS":
            case "BS":
            case "SU":
            case "BU":
            case "SE":
            case "BE":
            case "SB":
            case "BB":
                return true;
        }

        return false;
    }

    private static E_ITEM_TYPE GetItemType(string type)
    {
        switch (type)
        {
            default:
                return E_ITEM_TYPE.SMALL_SCORE;
            case "BS":
                return E_ITEM_TYPE.BIG_SCORE;
            case "SU":
                return E_ITEM_TYPE.SMALL_SCORE_UP;
            case "BU":
                return E_ITEM_TYPE.BIG_SCORE_UP;
            case "SE":
                return E_ITEM_TYPE.SMALL_EXP;
            case "BE":
                return E_ITEM_TYPE.BIG_EXP;
            case "SB":
                return E_ITEM_TYPE.SMALL_BOMB;
            case "BB":
                return E_ITEM_TYPE.BIG_BOMB;
        }
    }
}
