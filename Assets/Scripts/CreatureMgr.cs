using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 生物を管理するクラス
public class CreatureMgr : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Creaturemgrdata creaturedata;    // 生物データ
    [SerializeField] private Image creature_image;    // 生物イメージ
    [SerializeField] private Image creature_frame;    // 生物フレーム
    [SerializeField] private int index;    // 生物管理の要素番号
    [SerializeField] private AudioClip spawn;    // スポーン音
    [SerializeField] private GameObject timer;

    private GameObject creature_prefab;    // 生物のプレハブ
    private GameObject creature_instance;    // 生物のインスタンス
    private int creature_score;    // 生物オブジェクトの単スコア

    private Rigidbody2D creature_rigidbody;    // 生物インスタンスのRigidBody2D
    private Vector3 destination;    // 生物の目的座標
    private bool grabbing = false;    // 掴んでいるかの判定
    private AudioSource audiosource;


    // Start is called before the first frame update
    void Start()
    {
        // 生物データの初期化
        creature_prefab = creaturedata.GetCreature_Prefab(index);
        creature_image.GetComponent<Image>().sprite = creaturedata.GetCreature(index);
        creature_frame.GetComponent<Image>().sprite = creaturedata.GetCreature_Frame(index);
        creature_score = creaturedata.GetScore(index);

        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 生物インスタンスが生成されている場合、一定の距離落下したらインスタンスを削除
        if(creature_instance != null) {
            if(creature_instance.transform.position.y < -6) {
                creature_instance = null;
                grabbing = false;
            }
        }
    }

    // 一定秒で呼び出されるUpdate(物理演算の整合性を保つため)
    void FixedUpdate()
    {
        // 生物インスタンスを掴んでいるなら移動
        if(grabbing && creature_instance != null) {
            Vector2 velocity = (destination - creature_instance.transform.position) / Time.deltaTime;
            creature_rigidbody.linearVelocity = velocity;
        }
    }

    // タップ・ポインタが押されている場合に呼び出される関数
    public void OnPointerDown(PointerEventData eventData)
    {
        SpawnCreature(eventData);
    }

    // タップ・ポインタを離した場合に呼び出される関数
    public void OnPointerUp(PointerEventData eventData)
    {
        ReleaseCreature();
    }

    // ドラッグしている場合に呼び出される関数
    public void OnDrag(PointerEventData eventData)
    {
        if (creature_instance != null && grabbing) {
            if (creature_rigidbody != null) {
                // ドラッグしている位置を取得
                destination = Camera.main.ScreenToWorldPoint(eventData.position);
                destination.z = 0f;

                if(timer.GetComponent<Timer>().EndTimer()) {
                    ReleaseCreature();
                }
            }
        }
    }

    // 生物インスタンスを生成する関数
    void SpawnCreature(PointerEventData eventData)
    {
        // タップ・ポインタが押された位置に生物インスタンスを生成
        Vector3 spawnposition = Camera.main.ScreenToWorldPoint(eventData.position);
        spawnposition.z = 0f; // カメラからの距離を調整
        destination = spawnposition;

        creature_instance = Instantiate(creature_prefab, spawnposition, creature_prefab.transform.rotation);
        audiosource.PlayOneShot(spawn);
        creature_rigidbody = creature_instance.GetComponent<Rigidbody2D>();
        grabbing = true;
        creature_instance.GetComponent<Creature>().SetScore(creature_score);
        creature_instance.tag = "creature_" + index;
        creature_instance.layer = LayerMask.NameToLayer("handed_creature");
    }

    // 生物インスタンスが落下する関数
    void ReleaseCreature()
    {
        if(creature_instance != null) {
            if(creature_rigidbody != null) {
                creature_rigidbody.gravityScale = 1f;    // 重力gを1に設定
                grabbing = false;
                creature_instance.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;    // 生物インスタンスの速度を0に戻す
                creature_instance.layer = LayerMask.NameToLayer("creature");
                creature_instance.GetComponent<Creature>().SetReleasing();
                creature_instance = null;
            }
        }
    }
}