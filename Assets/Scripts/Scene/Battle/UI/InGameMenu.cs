using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public void OnSubmitRetry()
    {
        var scene = BaseSceneManager.Instance.CurrentScene;
        BaseSceneManager.Instance.LoadScene(scene.Scene);
    }

    public void OnSubmitRetire()
    {
        // ここに配置するのは良くないが、とりあえずここに処理を置く
        DataManager.Instance.OnShootingEnd();
        BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
    }
}
