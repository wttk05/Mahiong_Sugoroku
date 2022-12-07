using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
public class DiceManager : MonoBehaviour
{
    // インスタンスを作成
    public static DiceManager instance;

    [SerializeField] GameObject dicePrefab;

    //2つ生成するため
    [SerializeField] Vector3 dicePos1;
    [SerializeField] Vector3 dicePos2;

    // サイコロ
    private GameObject dice1;
    private GameObject dice2;
    private Rigidbody dice1rb;
    private Rigidbody dice2rb;

    public bool isRoll;

    bool move1;
    bool move2;

   public  int plusDiceNum { get; private set; }

    public bool cheat = false;
    public int cheatNum = 0;


    private void Awake()
    {
        // インスタンス生成
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //まず２つ生成
        dice1 = Instantiate(dicePrefab, dicePos1, Quaternion.identity);
        dice2 = Instantiate(dicePrefab, dicePos2, Quaternion.identity);

        dice1rb = dice1.GetComponent<Rigidbody>();
        dice2rb = dice2.GetComponent<Rigidbody>();

        dice1.SetActive(false);
        dice2.SetActive(false);
    }

    void Update()
    {
        // 停止したら固定して値を取る
        if (isRoll)
        {
            if (dice1rb.IsSleeping() && move1)
            {
                if (dice2rb.IsSleeping() && move2)
                {
                    var num1 = CheckDiceNum(dice1rb, dice1);
                    var num2 = CheckDiceNum(dice2rb, dice2);

                    // サイの目の合計値を取得
                    if (!cheat)
                    {
                        plusDiceNum =num1 + num2;
                    }
                    else
                    {
                        plusDiceNum = cheatNum;
                    }

                    GameManager.instance.DiceEnd(plusDiceNum);
                    
                    // 何回も入らないようにフラグを戻しておく
                    //isroll = false;
                    move1 = false;
                    move2 = false;
                }
            }
        }

    }

    /// <summary>
    /// ダイスを生成する
    /// </summary>
    void CreateDice()
    {
        isRoll = true;
        PlayerUI.instance.UpdatePlayRollText(false);

        //サイコロ回す
        dice1.SetActive(true);
        dice2.SetActive(true);
    }

    /// <summary>
    /// ダイスの目を調べる
    /// </summary>
    private int CheckDiceNum(Rigidbody rb , GameObject obj)
    {
            // しっかり停止させる
            rb.constraints = RigidbodyConstraints.FreezeAll;

            var children = new Transform[6];

            // 子（面ごとのトランスフォームを取得）
            for (int i = 0;i< children.Length; i++)
            {
                children[i] = obj.transform.GetChild(i);
            }

            // １番Yの値が高い子を出す
            var topValue = children[0].transform.position.y;
            var topIndex = 0;

            for (var i = 1; i < children.Length; ++i)
            {
                if (children[i].transform.position.y < topValue)
                    continue;
                topValue = children[i].transform.position.y;
                topIndex = i;
            }

            return topIndex + 1;
    }

    /// <summary>
    /// ダイスを振る処理
    /// </summary>
    public void CallDice()
    {
        if (isRoll)
            return;

        // まず生成
        CreateDice();

        // ゴールに向かって投げる
        MoveDice();

    }

    void MoveDice()
    {
        //サイコロに一度だけ力を加える（適当）
        dice1rb.AddForce(new Vector3(-200.0f, 0, 0.3f));
        dice2rb.AddForce(new Vector3(-200.0f, 0, 0.3f));

        // 投げる力をサバにわたすと同じ挙動をする（と思う）
        dice1rb.angularVelocity = new Vector3(Random.Range(-400.0f, 400.0f), Random.Range(-400.0f, 400.0f), Random.Range(-400.0f, 400.0f));
        dice2rb.angularVelocity = new Vector3(Random.Range(-400.0f, 400.0f), Random.Range(-400.0f, 400.0f), Random.Range(-400.0f, 400.0f));

        move1 = true;
        move2 = true;
    }

    public void DeleteDice()
    {
        // ダイスを初期位置に戻す
        dice1.transform.position = dicePos1;
        dice2.transform.position = dicePos2;
        dice1rb.constraints = RigidbodyConstraints.None;
        dice2rb.constraints = RigidbodyConstraints.None;
        dice1.SetActive(false);
        dice2.SetActive(false);

        isRoll = false;
        PlayerUI.instance.UpdatePlayRollText(true);



    }
}
