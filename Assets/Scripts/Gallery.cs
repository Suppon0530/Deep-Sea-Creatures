using UnityEngine;
using TMPro;

// ギャラリークラス
public class Gallery : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] score_text;    // ギャラリーの生物使用回数の配列

    private const int creature_num = 20;    // 生物の数

    private GalleryMgr gallerydata;    // ギャラリーデータ

    // Start is called before the first frame update
    void Start()
    {
        // ギャラリーデータを取得
        GameObject dontDestroyObject = GameObject.Find("GalleryMgr");
        if (dontDestroyObject != null)
        {
            gallerydata = dontDestroyObject.GetComponent<GalleryMgr>();

            for(int i = 0; i < creature_num; i++) {
                score_text[i].text = "・number of uses : " + gallerydata.GetCreatureData(i);
            }
        }
    }
}
