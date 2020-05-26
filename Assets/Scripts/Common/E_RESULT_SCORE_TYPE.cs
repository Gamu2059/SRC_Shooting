using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_RESULT_SCORE_TYPE
{
    LEVEL_BONUS = 1 << 0,
    MAX_CHAIN_BONUS = 1 << 1,
    BULLET_REMOVE_BONUS = 1 << 2,
    SECRET_ITEM_BONUS = 1 << 3,
    MAX_HACKING_CHAIN_BONUS = 1 << 4,
    SCORE = 1 << 5,
    TOTAL_SCORE = 1 << 6,
}
