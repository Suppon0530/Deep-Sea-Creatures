using UnityEngine;

// BGMクラス
public class SoundMgr : MonoBehaviour
{
    private static bool Dontdestroy = false;    // 静的変数

    void Start()
    {
        // DontDestroyがfalseであれば、DontDestroyOnLoad()
        if(!Dontdestroy) {
            DontDestroyOnLoad(this);
            Dontdestroy = true;
        }
        else {
            Destroy(this.gameObject);
            return;
        }
    }
}
