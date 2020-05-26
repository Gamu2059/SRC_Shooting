using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRecordManager : SingletonMonoBehavior<SystemRecordManager>
{
    #region Field Inspector

    [SerializeField]
    private SystemRecordManagerParamSet m_ParamSet;

    #endregion
}
