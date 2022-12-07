using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
//using UnityEngine.InputSystem.EnhancedTouch;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Threading;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // インスタンスを作成
    public static GameManager instance;



    // 牌の初期ポジション決め変数
    [SerializeField] Vector3 leftHaiPos;
    [SerializeField] Vector3 topHaiPos;
    [SerializeField] Vector3 rightHaiPos;
    [SerializeField] Vector3 downHaiPos;
    [SerializeField] Vector3 roadHaiPos;
    [SerializeField] Vector3 plusHaiPos;
    [SerializeField] Vector3 doraHaiPos;

    // プレイヤーの位置四箇所
    List<Vector3> playerPos = new List<Vector3>();

    // 列の牌の数を指定する変数
    [SerializeField] int LeftHaiCount;
    [SerializeField] int RightHaiCount;
    [SerializeField] int topHaiCount;
    [SerializeField] int DownHaiCount;
    [SerializeField] int roadHaiCount;
    [SerializeField] int plusHaiCount;
    [SerializeField] int doraHaiCount;

    // 列ごとの間隔を指定する関数
    [SerializeField] float sideHaiDistance;
    [SerializeField] float topHaiDistance;
    [SerializeField] float doraHaiDistance;

    // プレイヤー人数を保管する
    [SerializeField] int playerNum;

    // モデル
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<GameObject> haiPrefabs;
    [SerializeField] GameObject goal;

    // 牌の情報
    List<HaiData> haiDatas = new List<HaiData>();

    // プレイヤーの情報
    List<PlayerManager> playerManagerDatas = new List<PlayerManager>();

    // プレイヤーUI情報
    [SerializeField] PlayerUI playerUI;

    //ダイス情報
    [SerializeField] DiceManager diceManager;

    // アクション情報
    [SerializeField] HaiInfomation haiInfomation;

    // タッチ格納用
    //float touchTimer = 0f;
    //Vector2 firstTouchPos = new Vector2();
    //Vector2 endTouchPos = new Vector2();



    // ターン管理
    public TypeEnum.TurnState turnState { private set; get; }

    // ステータス
    [SerializeField] float moveTime;
    bool isBack = false;
    bool isNextDoraUse = false;
    public bool gameStart = false;

    //List<GameObject> viewHai = new List<GameObject>();
    //int viewHaiCount = 0;

    bool pinzoro = false;
    bool pinroku = false;
    bool appearManzu = false;

    // 接続方法
    public TypeEnum.Connection connection;


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

    //void OnEnable()
    //{
    //    EnhancedTouchSupport.Enable();
    //}

    //void OnDisable()
    //{
    //    EnhancedTouchSupport.Disable();
    //}

    void Start()
    {
        // オフラインか否かで接続方法を帰る
        connection = TypeEnum.Connection.Online;

        // プレイヤー生成
        CreatePlayer();

        // 牌データの格納
        InputHaiData();

        // 牌を生成
        CreateMahjongTiles();

        // ドラを生成する
        CreateDora();

        turnState = TypeEnum.TurnState.Player1;
        playerUI.UpdateTurnUI((int)turnState + 1);
    }

    private void Update()
    {
        // キーボードの状態の取得
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            //　スペースキーの状態の取得
            if (keyboard.dKey.wasPressedThisFrame)
            {
                DiceManager.instance.CallDice();
            }
        }

        //TouchPanel();

    }




    /// <summary>
    /// スマホでの画面タッチ操作（フリック対応）
    /// </summary>
    //void TouchPanel()
    //{
    //    foreach (var touch in Touch.activeTouches)
    //    {

    //        // タッチした時
    //        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
    //        {
    //            //タッチした位置を保存
    //            firstTouchPos = touch.screenPosition;
    //            Debug.Log($"fTouch: Id {touch.touchId} Position {touch.screenPosition} Phase {touch.phase}\n");
    //        }


    //        // 離した時
    //        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
    //        {
    //            // 最後に離した位置を保存
    //            endTouchPos = touch.screenPosition;
    //            Debug.Log($"eTouch: Id {touch.touchId} Position {touch.screenPosition} Phase {touch.phase}\n");


    //            var t = endTouchPos - firstTouchPos;

    //            Debug.Log("t = " + t);


    //            if (Mathf.Abs(t.x) > Mathf.Abs(t.y))
    //            {
    //                if (firstTouchPos.x > endTouchPos.x)
    //                {
    //                    Debug.Log("Left Frick");

    //                }
    //                else if (firstTouchPos.x < endTouchPos.x)
    //                {
    //                    Debug.Log("Right Frick");

    //                }
    //            }
    //            else if (Mathf.Abs(t.x) < Mathf.Abs(t.y))
    //            {
    //                if (firstTouchPos.y > endTouchPos.y)
    //                {
    //                    Debug.Log("Down Frick");

    //                }
    //                else if (firstTouchPos.y < endTouchPos.y)
    //                {
    //                    Debug.Log("Up Frick");
    //                    DiceManager.instance.CallDice();
    //                }
    //            }

    //        }
    //    }

    //Phase Began → タッチし始める位置
    //Phase Stationary → タッチし続けている
    //Phase Moved → 移動した
    //Phase Ended → タッチを辞めた
    //}

    /// <summary>
    /// サイコロの終了時に呼ばれる処理
    /// </summary>
    public void DiceEnd(int num)// NUM＝ダイスの目の値
    {
        // ピンゾロとロクゾロの
        if (num == 2 && !appearManzu)
        {
            // ピンゾロの処理
            pinzoro = true;
        }

        if (num == 12 && !appearManzu)
        {
            // ピンロクの処理
            pinroku = true;
        }

        // ダイスの値が決まり、送信されたらキャラを移動させる
        PlayerMove(num);

        //ゴールした場合
        if (playerManagerDatas[(int)turnState].posNum == 101)
        {
            playerManagerDatas[(int)turnState].isGoal = true;
        }
    }

    /// <summary>
    /// キャラクターを移動させる
    /// </summary>
    void PlayerMove(int aom)
    {
        // 通過したいオブジェクトを入れるリスト
        List<GameObject> targets = new List<GameObject>();
        var pos = 0;
        var ahure = 0;

        // 移動した先の数
        if (!isBack)
        {
            pos = playerManagerDatas[(int)turnState].posNum + aom;

            // ダイスの目から溢れた分
            ahure = pos - 100;

            // １００を超えた場合
            if (pos >= 100)
            {
                // AOMを調整
                aom = aom - ahure;
            }

            //移動先の座標を取得
            for (int i = 0; i < aom; i++)
            {
                targets.Add(haiDatas[i + playerManagerDatas[(int)turnState].posNum].haiPrefab);
            }

            // ゴールの位置以上にいったらゴールの場所をターゲットに追加する
            if (pos >= 101)
            {
                targets.Add(goal);
            }

            // 溢れていたら逆走
            if (ahure > 0)
            {
                // 溢れた場合逆走する
                for (int i = 1; i < ahure; i++)
                {
                    targets.Add(haiDatas[100 - i].haiPrefab);
                }
                isBack = true;
            }

            // 位置情報更新
            playerManagerDatas[(int)turnState].posNum = pos;

            // 溢れていたら調整する
            if (playerManagerDatas[(int)turnState].posNum >= 101)
            {
                // 折り返したので位置の調整
                var x = playerManagerDatas[(int)turnState].posNum - 101;
                var y = 101 - x;
                playerManagerDatas[(int)turnState].posNum = y;

                Debug.Log("調整後のプレイやーの位置" + (playerManagerDatas[(int)turnState].posNum - 1));
            }
        }
        else
        {
            // 逆走なので引く
            pos = playerManagerDatas[(int)turnState].posNum - aom;

            // マイナスよりいかないように設定
            if (pos <= 0)
            {
                pos = 1;
                aom = playerManagerDatas[(int)turnState].posNum - 1;
            }

            // 逆走なので値を引いていく
            for (int i = 0; i < aom; i++)
            {
                targets.Add(haiDatas[playerManagerDatas[(int)turnState].posNum - i - 1 - 1].haiPrefab);
            }

            // 位置情報更新
            playerManagerDatas[(int)turnState].posNum = pos;
        }

        //移動
        playerManagerDatas[(int)turnState].prefab.transform.DOPath
            (
            targets.Select(target => target.transform.position + new Vector3(0, 0.2f, 0)).ToArray(),
            moveTime,
            PathType.Linear
            ).OnComplete(() => SpinHai());
    }


    /// <summary>
    /// 牌を回転させる
    /// </summary>
    async void SpinHai()
    {
        Debug.Log("めくった牌は" + haiDatas[playerManagerDatas[(int)turnState].posNum - 1].haiType);

        if (playerManagerDatas[(int)turnState].posNum - 1 == 100)
        {
            //ゴールしている時
            Thread.Sleep(2);
            PlayerAction("ゴール", haiDatas[playerManagerDatas[(int)turnState].posNum - 1]);
        }
        else
        {
           // 止まる場所の牌を回転させる
            haiDatas[playerManagerDatas[(int)turnState].posNum - 1]
            .haiPrefab.GetComponent<MahjongManager>()
            .SpinMahjongHai(haiDatas[playerManagerDatas[(int)turnState].posNum - 1],false);

            // 見せるためにちょっと遅延
            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            // 牌のタイプに応じて点数計算を行う
            PlayerAction(haiDatas[playerManagerDatas[(int)turnState].posNum - 1].haiType.ToString(), haiDatas[playerManagerDatas[(int)turnState].posNum - 1]);
        }
    }

    /// <summary>
    /// ターン終了処理 async
    /// </summary>
    async UniTask EndTurn(float sec)
    {
        // ちょっと遅延をつける
        await UniTask.Delay(TimeSpan.FromSeconds(sec));

        // ネクストドラを使用していた場合、倍率１に戻す
        if(isNextDoraUse)
        {
            isNextDoraUse = false;
            playerManagerDatas[(int)turnState].nextDoraCount = 1;
        }

        // ゴールしていたら終了処理
        if (playerManagerDatas[(int)turnState].isGoal)
        {
            Debug.Log(playerManagerDatas[(int)turnState].name + "がゴールしました");
            GameSet();
        }
        else
        {
            // 逆走モードを戻す
            if (pinzoro)
            {
                pinzoro = false;
                isBack = true;// 次逆走モード
            }
            else if (pinroku)
            {
                pinroku = false;
                // ターンチェンジせずもう一回
            }
            else
            {
                isBack = false;
                TurnChange();
            }


            // サイコロを消す
            DiceManager.instance.DeleteDice();
        }
    }

    /// <summary>
    /// 麻雀牌にデータを格納するための変数
    /// </summary>
    public HaiData SetData(int i)
    {
        return haiDatas[i];
    }

    /// <summary>
    /// 麻雀牌のデータを格納
    /// </summary>
    void InputHaiData()
    {
        int count = 0;

        // 牌データにプレハブの情報を格納
        for (int i = 0; i < haiPrefabs.Count; i++)
        {
            HaiData haiData = new HaiData();
            // 牌データにプレハブ情報を渡す
            haiData.haiPrefab = haiPrefabs[i];

            //////字牌（固定ID）
            if (count is 0 or 1 or 2 or 3) { haiData.haiType = TypeEnum.HaiType.中; haiData.type = TypeEnum.Type.その他; }
            if (count is 4 or 5 or 6 or 7) { haiData.haiType = TypeEnum.HaiType.發; haiData.type = TypeEnum.Type.その他; }
            if (count is 8 or 9 or 10 or 11) { haiData.haiType = TypeEnum.HaiType.白; haiData.type = TypeEnum.Type.その他; }
            //////筒子
            if (count is 12 or 13 or 14 or 15) { haiData.haiType = TypeEnum.HaiType.一筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 16 or 17 or 18 or 19) { haiData.haiType = TypeEnum.HaiType.二筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 20 or 21 or 22 or 23) { haiData.haiType = TypeEnum.HaiType.三筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 24 or 25 or 26 or 27) { haiData.haiType = TypeEnum.HaiType.四筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 28 or 29) { haiData.haiType = TypeEnum.HaiType.五筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 30 or 31) { haiData.haiType = TypeEnum.HaiType.赤五筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 32 or 33 or 34 or 35) { haiData.haiType = TypeEnum.HaiType.六筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 36 or 37 or 38 or 39) { haiData.haiType = TypeEnum.HaiType.七筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 40 or 41 or 42 or 43) { haiData.haiType = TypeEnum.HaiType.八筒; haiData.type = TypeEnum.Type.筒子; }
            if (count is 44 or 45 or 46 or 47) { haiData.haiType = TypeEnum.HaiType.九筒; haiData.type = TypeEnum.Type.筒子; }
            ///////索子
            if (count is 48 or 49 or 50 or 51) { haiData.haiType = TypeEnum.HaiType.一索; haiData.type = TypeEnum.Type.索子; }
            if (count is 52 or 53 or 54 or 55) { haiData.haiType = TypeEnum.HaiType.二索; haiData.type = TypeEnum.Type.索子; }
            if (count is 56 or 57 or 58 or 59) { haiData.haiType = TypeEnum.HaiType.三索; haiData.type = TypeEnum.Type.索子; }
            if (count is 60 or 61 or 62 or 63) { haiData.haiType = TypeEnum.HaiType.四索; haiData.type = TypeEnum.Type.索子; }
            if (count is 64 or 65 or 66 or 67) { haiData.haiType = TypeEnum.HaiType.五索; haiData.type = TypeEnum.Type.索子; }
            if (count is 64 or 65 or 66) { haiData.haiType = TypeEnum.HaiType.五索; haiData.type = TypeEnum.Type.索子; }
            if (count is 67) { haiData.haiType = TypeEnum.HaiType.赤五索; haiData.type = TypeEnum.Type.索子; }
            if (count is 68 or 69 or 70 or 71) { haiData.haiType = TypeEnum.HaiType.六索; haiData.type = TypeEnum.Type.索子; }
            if (count is 72 or 73 or 74 or 75) { haiData.haiType = TypeEnum.HaiType.七索; haiData.type = TypeEnum.Type.索子; }
            if (count is 76 or 77 or 78 or 79) { haiData.haiType = TypeEnum.HaiType.八索; haiData.type = TypeEnum.Type.索子; }
            if (count is 80 or 81 or 82 or 83) { haiData.haiType = TypeEnum.HaiType.九索; haiData.type = TypeEnum.Type.索子; }
            /////////萬子
            if (count is 84 or 85 or 86 or 87) { haiData.haiType = TypeEnum.HaiType.一萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 88 or 89 or 90 or 91) { haiData.haiType = TypeEnum.HaiType.二萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 92 or 93 or 94 or 95) { haiData.haiType = TypeEnum.HaiType.三萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 96 or 97 or 98 or 99) { haiData.haiType = TypeEnum.HaiType.四萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 100 or 101 or 102) { haiData.haiType = TypeEnum.HaiType.五萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 103) { haiData.haiType = TypeEnum.HaiType.赤五萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 104 or 105 or 106 or 107) { haiData.haiType = TypeEnum.HaiType.六萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 108 or 109 or 110 or 111) { haiData.haiType = TypeEnum.HaiType.七萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 112 or 113 or 114 or 115) { haiData.haiType = TypeEnum.HaiType.八萬; haiData.type = TypeEnum.Type.萬子; }
            if (count is 116 or 117 or 118 or 119) { haiData.haiType = TypeEnum.HaiType.九萬; haiData.type = TypeEnum.Type.萬子; }

            // プレハブと牌固有IDを保存
            haiDatas.Add(haiData);

            count++;
        }

        // 
        gameStart = true;
    }

    /// <summary>
    /// 牌の生成とシャッフル
    /// </summary>
    void CreateMahjongTiles()
    {
        // 格納されている麻雀牌をシャッフル
        haiDatas = haiDatas.OrderBy(haiPrefabs => Guid.NewGuid()).ToList();

        // 牌の並び連番
        int haiNum = 0;

        ////// 麻雀牌生成 //////
        // 左下
        for (int i = 0; i < plusHaiCount; i++)
        {
            var pos = plusHaiPos + new Vector3(0, 0, -topHaiDistance * i);
            haiDatas[haiNum].haiPrefab = Instantiate(haiDatas[haiNum].haiPrefab, pos, Quaternion.Euler(180f, 0f, 90f));
            haiDatas[haiNum].haiPrefab.name = haiNum.ToString();
            haiDatas[haiNum].haiNum = haiNum;
            haiNum++;
        }

        // 左横一列
        for (int i = 0; i < LeftHaiCount; i++)
        {
            var pos = leftHaiPos + new Vector3(-sideHaiDistance * i, 0, 0);
            haiDatas[haiNum].haiPrefab = Instantiate(haiDatas[haiNum].haiPrefab, pos, Quaternion.Euler(180f, 270f, 90f));
            haiDatas[haiNum].haiPrefab.name = haiNum.ToString();
            haiDatas[haiNum].haiNum = haiNum;
            haiNum++;
        }

        // 上一列
        for (int i = 0; i < topHaiCount; i++)
        {
            var pos = topHaiPos + new Vector3(0, 0, topHaiDistance * i);
            haiDatas[haiNum].haiPrefab = Instantiate(haiDatas[haiNum].haiPrefab, pos, Quaternion.Euler(180f, 0f, 90f));
            haiDatas[haiNum].haiPrefab.name = haiNum.ToString();
            haiDatas[haiNum].haiNum = haiNum;
            haiNum++;
        }

        // 右一列
        for (int i = 0; i < RightHaiCount; i++)
        {
            var pos = rightHaiPos + new Vector3(sideHaiDistance * i, 0, 0);
            haiDatas[haiNum].haiPrefab = Instantiate(haiDatas[haiNum].haiPrefab, pos, Quaternion.Euler(180f, 90f, 90f));
            haiDatas[haiNum].haiPrefab.name = haiNum.ToString();
            haiDatas[haiNum].haiNum = haiNum;
            haiNum++;
        }

        // 下一列
        for (int i = 0; i < DownHaiCount; i++)
        {
            var pos = downHaiPos + new Vector3(0, 0, -topHaiDistance * i);
            haiDatas[haiNum].haiPrefab = Instantiate(haiDatas[haiNum].haiPrefab, pos, Quaternion.Euler(180f, 0f, 90f));
            haiDatas[haiNum].haiPrefab.name = haiNum.ToString();
            haiDatas[haiNum].haiNum = haiNum;
            haiNum++;
        }

        // 最後の道
        for (int i = 0; i < roadHaiCount; i++)
        {
            var pos = roadHaiPos + new Vector3(-sideHaiDistance * i, 0, 0);
            haiDatas[haiNum].haiPrefab = Instantiate(haiDatas[haiNum].haiPrefab, pos, Quaternion.Euler(180f, 270f, 90f));
            haiDatas[haiNum].haiPrefab.name = haiNum.ToString();
            haiDatas[haiNum].haiNum = haiNum;
            haiNum++;
        }

        // ドラ表示
        int j = 0;
        for (int i = 0; i < doraHaiCount; i++)
        {
            // ドラ表示牌をONにする
            haiDatas[haiNum].doraDisplay = true;

            // 五回目で改行
            if (i % 5 == 0 && i != 0)
            {
                j++;
            }

            var pos = doraHaiPos + new Vector3(doraHaiDistance * j, 0, (topHaiDistance * i) - (j * 1.5f));
            haiDatas[haiNum].haiPrefab = Instantiate(haiDatas[haiNum].haiPrefab, pos, Quaternion.Euler(180f, 0f, 90f));
            haiDatas[haiNum].haiPrefab.name = haiNum.ToString();
            haiDatas[haiNum].haiNum = haiNum;

            // ドラ表示牌は表にしておく
            haiDatas[haiNum].haiPrefab.GetComponent<MahjongManager>().SpinMahjongHai(haiDatas[haiNum],true);

            haiNum++;
        }

        // ドラソート（企画者のこだわり）
        for (int i = 0; i < 19; i++)
        {
            for (int l = 0; l < 19 - i; l++)
            {
                if ((int)haiDatas[l + 100].haiType > (int)haiDatas[l + 100 + 1].haiType)
                {
                    HaiData h = haiDatas[l + 100];
                    Vector3 temp = haiDatas[l + 100].haiPrefab.transform.position;

                    haiDatas[l + 100].haiPrefab.transform.position = haiDatas[l + 100 + 1].haiPrefab.transform.position;
                    haiDatas[l + 100 + 1].haiPrefab.transform.position = temp;

                    haiDatas[l + 100] = haiDatas[l + 100 + 1];
                    haiDatas[l + 100 + 1] = h;
                }
            }
        }
    }

    /// <summary>
    /// ドラを設定する
    /// </summary>
    void CreateDora()
    {
        for (int i = 0; i < haiDatas.Count; i++)
        {
            // 赤は確定でドラにする
            if (haiDatas[i].haiType is TypeEnum.HaiType.赤五筒 or TypeEnum.HaiType.赤五索 or TypeEnum.HaiType.赤五萬)
            {
                haiDatas[i].redDora = true;
            }

            // 数字の折返し部分は前に戻ってドラを決める
            RefrainDoraHai(i, TypeEnum.HaiType.九萬, TypeEnum.HaiType.一萬);
            RefrainDoraHai(i, TypeEnum.HaiType.九筒, TypeEnum.HaiType.一筒);
            RefrainDoraHai(i, TypeEnum.HaiType.九索, TypeEnum.HaiType.一索);
            RefrainDoraHai(i, TypeEnum.HaiType.赤五筒, TypeEnum.HaiType.六筒);
            RefrainDoraHai(i, TypeEnum.HaiType.赤五索, TypeEnum.HaiType.六索);
            RefrainDoraHai(i, TypeEnum.HaiType.赤五萬, TypeEnum.HaiType.六萬);
            RefrainDoraHai(i, TypeEnum.HaiType.中, TypeEnum.HaiType.白);

            // それ以外はドラ表示牌の次の牌
            if (haiDatas[i].doraDisplay == true)
            {
                // ドラ表示牌の牌を保存
                TypeEnum.HaiType haiTypeDisplay = haiDatas[i].haiType;

                //ドラ表示牌の次の値を探してドラにする
                for (int j = 0; j < haiDatas.Count; j++)
                {
                    //牌タイプの次の牌はドラになる
                    if ((int)haiDatas[j].haiType == (int)haiTypeDisplay + 1)
                    {
                        // 折返しの部分はスルーする
                        if (haiDatas[j].haiType != TypeEnum.HaiType.九萬 ||
                             haiDatas[j].haiType != TypeEnum.HaiType.九筒 ||
                             haiDatas[j].haiType != TypeEnum.HaiType.九索 ||
                             haiDatas[j].haiType != TypeEnum.HaiType.赤五萬 ||
                             haiDatas[j].haiType != TypeEnum.HaiType.赤五筒 ||
                             haiDatas[j].haiType != TypeEnum.HaiType.赤五索 ||
                             haiDatas[j].haiType != TypeEnum.HaiType.中)
                        {
                            haiDatas[j].dora = true;
                        }
                    }
                }
            }
        }

        // ドラ確認
        for (int i = 0; i < haiDatas.Count; i++)
        {
            // 順番
            //Debug.Log(haiDatas[i].haiNum + "牌の種類 : " + haiDatas[i].haiType);

            //haiDatas[i].haiPrefab.GetComponent<MahjongManager>().InputHaiData();

            // ドラ牌の確認
            if (haiDatas[i].dora == true)
            {
                // ドラの牌の色を変える
                //haiDatas[i].haiPrefab.GetComponent<Renderer>().sharedMaterial.color = Color.red;

                //Debug.Log("牌の種類 : " + haiDatas[i].haiType + "はドラです");
            }

            if (haiDatas[i].redDora == true)
            {
                // ドラの牌の色を変える
                //haiDatas[i].haiPrefab.GetComponent<Renderer>().sharedMaterial.color = Color.red;

                //Debug.Log("牌の種類 : " + haiDatas[i].haiType + "は赤ドラです");
            }
        }
    }

    /// <summary>
    /// ドラを折り返す仕組み
    /// </summary>
    void RefrainDoraHai(int l, TypeEnum.HaiType kyu, TypeEnum.HaiType iti)
    {
        if (haiDatas[l].haiType == kyu && haiDatas[l].doraDisplay == true)
        {
            // 牌の中から一を探す
            for (int j = 0; j < haiDatas.Count; j++)
            {
                // ドラにする
                if (haiDatas[j].haiType == iti)
                {
                    haiDatas[j].dora = true;
                }
            }
        }
    }

    /// <summary>
    /// プレイヤーの生成
    /// </summary>
    void CreatePlayer()
    {
        // 固定値を代入
        playerPos.Add(new Vector3(14.2f, 0, -1.9f));
        playerPos.Add(new Vector3(14.5f, 0, -1.9f));
        playerPos.Add(new Vector3(14.2f, 0, -1.6f));
        playerPos.Add(new Vector3(14.5f, 0, -1.6f));


        for (int i = 0; i < playerPos.Count; i++)
        {
            GameObject pre = Instantiate(playerPrefab, playerPos[i], Quaternion.identity);

            // 
            PlayerManager pM = pre.GetComponent<PlayerManager>();
            pM.prefab = pre;
            pM.nextDoraCount = 1;

            if (i == 0)
            {
                pM.prefab.name = "Player1";
                pM.prefab.GetComponent<Renderer>().material.color = Color.red;
                pM.color = Color.red;
            }
            else if (i == 1)
            {
                pM.prefab.name = "Player2";
                pM.prefab.GetComponent<Renderer>().material.color = Color.blue;
                pM.color = Color.blue;
            }
            else if (i == 2)
            {
                pM.prefab.name = "Player3";
                pM.prefab.GetComponent<Renderer>().material.color = Color.yellow;
                pM.color = Color.yellow;
            }
            else if (i == 3)
            {
                pM.prefab.name = "Player4";
                pM.prefab.GetComponent<Renderer>().material.color = Color.white;
                pM.color = Color.white;
            }
            playerManagerDatas.Add(pM);
        }

        playerUI.InputPlayerUIData(playerManagerDatas[0], playerManagerDatas[1], playerManagerDatas[2], playerManagerDatas[3]);

    }

    /// <summary>
    /// ターンを変更する
    /// </summary>
    void TurnChange()
    {
        if (turnState == TypeEnum.TurnState.Player1)
        {
            turnState = TypeEnum.TurnState.Player2;
        }
        else if (turnState == TypeEnum.TurnState.Player2)
        {
            turnState = TypeEnum.TurnState.Player3;
        }
        else if (turnState == TypeEnum.TurnState.Player3)
        {
            turnState = TypeEnum.TurnState.Player4;
        }
        else if (turnState == TypeEnum.TurnState.Player4)
        {
            turnState = TypeEnum.TurnState.Player1;
        }
        // TurnSkip((int)turnState);

        playerUI.UpdateTurnUI((int)turnState + 1);
    }

    /// <summary>
    /// ターンを進める処理
    /// </summary>
    void TurnSkip(int t)
    {
        //追って実装
    }

    /// <summary>
    /// 駒停止後に起きるアクションの処理
    /// </summary>
    async void PlayerAction(String haiName, HaiData haidata)
    {
        // ゴールしていない場合のみアクションを行う
        if (!haiName.Equals("ゴール"))
        {
            // アクションを判別
            HaiAction action = new();
            action = haiInfomation.HaiJudge(haiName);

            //  ドラ数格納
            int doraNum = 1;
            doraNum = DoraCount(haidata, action);


            // 見つけたドラの数×倍
            Debug.Log("全部で"+doraNum * playerManagerDatas[(int)turnState].nextDoraCount + "倍です");

            if (action.type == TypeEnum.Type.筒子)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == (int)turnState)
                    {
                        // 自分の加点
                        var fluctuationScore = action.num * 100 * 3 * doraNum * playerManagerDatas[(int)turnState].nextDoraCount;
                        var lastScore = playerManagerDatas[i].score.Value + fluctuationScore;
                        EaseScore(playerManagerDatas[i].score.Value, lastScore, i, fluctuationScore, true);

                        Debug.Log("`ドラ数 : " + doraNum);
                        Debug.Log("前回から引き継ぎドラネクスト : " + playerManagerDatas[(int)turnState].nextDoraCount);
                        Debug.Log("加算されるスコア : " + action.num * 100 * 3 * doraNum * playerManagerDatas[(int)turnState].nextDoraCount);
                    }
                    else
                    {
                        // それ以外の減点
                        var fluctuationScore = action.num * 100 * doraNum * playerManagerDatas[(int)turnState].nextDoraCount;
                        var lastScore = playerManagerDatas[i].score.Value - fluctuationScore;
                        EaseScore(playerManagerDatas[i].score.Value, lastScore, i, fluctuationScore, false);                 
                    }
                }
                // ネクストドラを使用していた場合、使用フラグを立てる
                if (playerManagerDatas[(int)turnState].nextDoraCount != 1)
                {
                    isNextDoraUse = true;
                }
            }
            else if (action.type == TypeEnum.Type.索子)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == (int)turnState)
                    {
                        // 自分の減点
                        var fluctuationScore = action.num * 100 * 3 * doraNum * playerManagerDatas[(int)turnState].nextDoraCount;
                        var lastScore = playerManagerDatas[i].score.Value - fluctuationScore;
                        EaseScore(playerManagerDatas[i].score.Value, lastScore, i, fluctuationScore, false);

                        Debug.Log("`ドラ数 : " + doraNum);
                        Debug.Log("前回から引き継ぎドラネクスト : " + playerManagerDatas[(int)turnState].nextDoraCount);
                        Debug.Log("減算されるスコア : " + -action.num * 100 * 3 * doraNum * playerManagerDatas[(int)turnState].nextDoraCount);
                    }
                    else
                    {
                        // それ以外の加点
                        var fluctuationScore = action.num * 100 * doraNum * playerManagerDatas[(int)turnState].nextDoraCount;
                        var lastScore = playerManagerDatas[i].score.Value + fluctuationScore;
                        EaseScore(playerManagerDatas[i].score.Value, lastScore, i, fluctuationScore, true);
                    }
                }
                // ネクストドラを使用していた場合、使用フラグを立てる
                if (playerManagerDatas[(int)turnState].nextDoraCount != 1)
                {
                    isNextDoraUse = true;
                }
            }
            else if (action.type == TypeEnum.Type.萬子)
            {
                // サイコロが終わった判定と同じなのでPlayerMoveではない
                appearManzu = true;
                Debug.Log(action.num * doraNum * playerManagerDatas[(int)turnState].nextDoraCount + "歩　前進します");

                if (playerManagerDatas[(int)turnState].posNum != 1)
                {
                    // ネクストドラを使用していた場合、使用フラグを立てる
                    if (playerManagerDatas[(int)turnState].nextDoraCount != 1)
                    {
                        isNextDoraUse = true;
                    }

                    // もう一度サイコロがあるので使用していたら倍率１に戻す
                    if(isNextDoraUse)
                    {
                        playerManagerDatas[(int)turnState].nextDoraCount = 1;
                    }

                    // 萬子の出目分進む
                    DiceEnd(action.num * doraNum * playerManagerDatas[(int)turnState].nextDoraCount);
                }
                else
                {
                    // 萬子が出たフラグを戻して次のターンへ
                    appearManzu = false;
                    await EndTurn(1f);
                }
            }
            else
            {
                // 三元牌
                playerManagerDatas[(int)turnState].nextDoraCount = playerManagerDatas[(int)turnState].nextDoraCount * 2*doraNum;
                Debug.Log(playerManagerDatas[(int)turnState] + "番は次回" + playerManagerDatas[(int)turnState].nextDoraCount + "倍から");
            }

            //終了
            if (action.type != TypeEnum.Type.萬子)
            {
                // 萬子以外が出現しているので
                appearManzu = false;
                await EndTurn(1f);
            }
        }
        else
        {
            await EndTurn(1f);
        }
    }

    /// <summary>
    /// スコアに演出をつけている
    /// </summary>
    void EaseScore(int firstscore, int lastscore, int i, int fluctuationScore, bool code)// 0がマイナス
    {
        // マンズとかのとき通る
        if (firstscore is 0)
        {
            playerManagerDatas[i].fluctuationScore.Value = fluctuationScore;
        }
        else
        {
            //スコア 増減値色
            if (code)
                playerManagerDatas[i].fluctuationScore.Value = fluctuationScore;
            else
                playerManagerDatas[i].fluctuationScore.Value = -fluctuationScore;


            // スコアアニメーション
            DOTween.To(
                () => playerManagerDatas[i].score.Value,
                num => playerManagerDatas[i].score.Value = num,
                lastscore,
                1.4f
                );
        }

        // 加減算したので増減値をリセット
        playerManagerDatas[i].fluctuationScore.Value = 0;
    }

    /// <summary>
    /// ドラがいくつあるか計算
    /// </summary>
    int DoraCount(HaiData haidata, HaiAction action)
    {
        int doraCount = 1;

        // 赤ドラかどうか確認
        if (haidata.redDora)
        {
            Debug.Log("あかどらです");
            doraCount = doraCount * 2;
        }

        // ドラ表示牌からドラを探す
        for (int i = 100; i < 119; i++)
        {
            // 牌のタイプが同じ
            if (haidata.type == haiDatas[i].type)
            {
                Debug.Log("牌のタイプが同じです");

                // 引いた牌がドラ表示牌の次の数字
                if ((int)haidata.haiType == (int)haiDatas[i].haiType + 1)// 引いた牌 == ドラ表示牌＋１の数字
                {
                    // 9と黒5以外は確実に通る
                    Debug.Log(haiDatas[i].haiType + "があるので" + haidata.haiType + "がドラになります");
                    doraCount = doraCount * 2;
                }

                // 赤の5のみ通る処理
                if (action.num == 5 && action.isRed)
                {
                    if ((int)haidata.haiType == (int)haiDatas[i].haiType + 2)
                    {
                        Debug.Log(haiDatas[i].haiType + "があるので2つ飛んで" + haidata.haiType + "がドラになります");
                        doraCount = doraCount * 2;
                    }
                }

                // 9のみ通る処理
                if (action.num == 1)
                {
                    // 引いた牌がドラ表示牌の次の数字
                    if ((int)haidata.haiType == (int)haiDatas[i].haiType - 9)// 引いた牌 == ドラ表示牌＋１の数字
                    {
                        // 9と黒5以外は確実に通る
                        Debug.Log(haiDatas[i].haiType + "があるので９つ飛んで" + haidata.haiType + "がドラになります");
                        doraCount = doraCount * 2;
                    }
                }

                // 中のみ通る処理
                if(haiDatas[i].haiType == TypeEnum.HaiType.白)
                {
                    if ((int)haidata.haiType == (int)haiDatas[i].haiType + 2)
                    {
                        Debug.Log(haiDatas[i].haiType + "があるので2つ飛んで" + haidata.haiType + "がドラになります");
                        doraCount = doraCount * 2;
                    }
                }
            }
        }
        return doraCount;
    }

    /// <summary>
    /// ゴール時に誰か何番目かを取得する
    /// </summary>
    void PlayerDistanceToGoal()
    {
        for (int i = 0; i < 4; i++)
        {
            if (playerManagerDatas[0].posNum < playerManagerDatas[i].posNum)
            {
                playerManagerDatas[0].ArrivalRank += 1;
            }

            if (playerManagerDatas[1].posNum < playerManagerDatas[i].posNum)
            {
                playerManagerDatas[1].ArrivalRank += 1;
            }

            if (playerManagerDatas[2].posNum < playerManagerDatas[i].posNum)
            {
                playerManagerDatas[2].ArrivalRank += 1;
            }

            if (playerManagerDatas[3].posNum < playerManagerDatas[i].posNum)
            {
                playerManagerDatas[3].ArrivalRank += 1;
            }
        }
    }

    /// <summary>
    /// ゴールした時の点数加算処理
    /// </summary>
    async UniTask  Uma(float sec)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(sec));

        var plusScore = 0;
        var lastScore = 0;

        for (int i = 0; i < 4; i++)
        {
            switch (playerManagerDatas[i].ArrivalRank)
            {
                case 0:
                    plusScore = 10000;
                    lastScore =playerManagerDatas[i].score.Value + 10000;
                    EaseScore(playerManagerDatas[i].score.Value, lastScore, i, plusScore, true);
                    break;
                case 1:
                    plusScore = 5000;
                    lastScore = playerManagerDatas[i].score.Value + 5000;
                    EaseScore(playerManagerDatas[i].score.Value, lastScore, i, plusScore, true);
                    break;
                case 2:
                    plusScore = 3000;
                    lastScore = playerManagerDatas[i].score.Value + 3000;
                    EaseScore(playerManagerDatas[i].score.Value, lastScore, i, plusScore, true);
                    break;
                case 3:
                    plusScore = 1000;
                    lastScore = playerManagerDatas[i].score.Value + 1000;
                    EaseScore(playerManagerDatas[i].score.Value, lastScore, i, plusScore, true);
                    break;
            }
        }
    }

    /// <summary>
    /// ゴールした時の終了処理
    /// </summary>
    async void  GameSet()
    {
        Debug.Log("ゲームが終了しました");
        PlayerDistanceToGoal();
        await Uma(1f);

        // ゲーム終了、結果発表へ
    }
}

