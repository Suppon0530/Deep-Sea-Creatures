using UnityEngine;

// アスペクト比を一定に設定するクラス
[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;
    private Vector2 targetVector2 = new Vector2(9, 16);

    void Update()
    {
        float screenAspect = Screen.width / (float)Screen.height;
        float targetAspect = targetVector2.x / targetVector2.y;
        float screenMultiplier = targetAspect / screenAspect;

        Rect viewportRect = new Rect(0, 0, 1, 1); //Viewport初期値でRectを作成

        if (screenMultiplier < 1)
        {
            viewportRect.width = screenMultiplier; //使用する横幅を変更
            viewportRect.x = 0.5f - viewportRect.width * 0.5f; //中央寄せ
        }
        else
        {
            viewportRect.height = 1 / screenMultiplier; //使用する縦幅を変更
            viewportRect.y = 0.5f - viewportRect.height * 0.5f; //中央余生
        }

        //カメラのViewportに設定
        targetCamera.rect = viewportRect;
    }
}