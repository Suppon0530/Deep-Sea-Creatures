using UnityEngine;

// 生物の回転ボタンクラス
public class Rotate : MonoBehaviour
{
    [SerializeField] Creaturemgrdata creaturedata;    // 生物データ
    [SerializeField] private GameObject[] creature_image;    // 生物のImageオブジェクト

    private GameObject[] creature;    // 生物オブジェクト
    private float rotationAngle = 0.0f; // 回転角度


    // Start is called before the first frame update
    void Start()
    {
        // 生物オブジェクトの取得
        creature = new GameObject[5];
        for(int i = 0; i < 5; i++) {
            creature[i] = creaturedata.GetCreature_Prefab(i);;
        }
    }

    // 生物オブジェクトを回転させる関数
    public void RotateCreature()
    {
        float angleIncrement = 60.0f;
        rotationAngle += angleIncrement;
        if (rotationAngle >= 360.0f)
        {
            rotationAngle -= 360.0f;
        }

        for(int i = 0; i < creature.Length; i++) {
            creature[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
            creature_image[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
        }
    }
}
