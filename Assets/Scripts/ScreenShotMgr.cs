using System.Collections;
using UnityEngine;

// スクリーンショットクラス
public class ScreenShotMgr : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private RenderTexture rendertexture;
    private Texture2D screenshotTexture;
    private Sprite textureSprite;
    private ScoreMgr playerdata;

    // Start is called before the first frame update
    void Start()
    {
        GameObject dontDestroyObject = GameObject.Find("ScoreMgr");
        if (dontDestroyObject != null)
        {
            playerdata = dontDestroyObject.GetComponent<ScoreMgr>();
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        rendertexture = new RenderTexture((int)canvasRect.rect.width, (int)canvasRect.rect.height, 24);

        screenshotTexture = new Texture2D(rendertexture.width, rendertexture.height, TextureFormat.RGB24, false);
    }

    // スクリーンショットを撮影する関数
    public void ScreenShot()
    {
        StartCoroutine(CaptureScreenshot());
    }

    IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        // CanvasをRenderTextureに描画
        RenderTexture.active = rendertexture;
        Camera.main.targetTexture = rendertexture;
        Camera.main.Render();

        // RenderTextureをTexture2Dに読み込む
        screenshotTexture.ReadPixels(new Rect(0, 0, rendertexture.width, rendertexture.height), 0, 0);
        screenshotTexture.Apply();

        // 白い枠を追加
        AddWhiteBorder(screenshotTexture, 8);

        // Texture2DをSpriteに変換
        textureSprite = Sprite.Create(screenshotTexture, new Rect(0, 0, screenshotTexture.width, screenshotTexture.height), Vector2.zero);

        // 後始末
        RenderTexture.active = null;
        Camera.main.targetTexture = null;
        
        playerdata.SetSprite(textureSprite);
    }

    // 白い枠を追加するメソッド
    void AddWhiteBorder(Texture2D texture, int borderWidth)
    {
        Color[] pixels = texture.GetPixels();

        int width = texture.width;
        int height = texture.height;

        // 上側の枠
        for (int y = 0; y < borderWidth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[y * width + x] = Color.white;
            }
        }

        // 下側の枠
        for (int y = height - borderWidth; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[y * width + x] = Color.white;
            }
        }

        // 左側の枠
        for (int y = borderWidth; y < height - borderWidth; y++)
        {
            for (int x = 0; x < borderWidth; x++)
            {
                pixels[y * width + x] = Color.white;
            }
        }

        // 右側の枠
        for (int y = borderWidth; y < height - borderWidth; y++)
        {
            for (int x = width - borderWidth; x < width; x++)
            {
                pixels[y * width + x] = Color.white;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }
}