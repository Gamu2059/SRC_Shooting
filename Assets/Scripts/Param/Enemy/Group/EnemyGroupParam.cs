using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupParam : ScriptableObject
{
    public class EnemyParam
    {
        public float Appear;

        public int ListIndex;
    }

    public EnemyParam[] EnemyParams;


    public EnemyParam[] GetEnemyGroupParams()
    {
        return EnemyParams;
    }
}
