using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;

public class PlayerUI : MonoBehaviour
{
    // インスタンスを作成
    public static PlayerUI instance;

    [SerializeField] TextMeshProUGUI scoreText1;
    [SerializeField] TextMeshProUGUI scoreText2;
    [SerializeField] TextMeshProUGUI scoreText3;
    [SerializeField] TextMeshProUGUI scoreText4;

    [SerializeField] TextMeshProUGUI fScoreText1view;
    [SerializeField] TextMeshProUGUI fScoreText2view;
    [SerializeField] TextMeshProUGUI fScoreText3view;
    [SerializeField] TextMeshProUGUI fScoreText4view;

    [SerializeField] TextMeshProUGUI nextBoostScoreText1;
    [SerializeField] TextMeshProUGUI nextBoostScoreText2;
    [SerializeField] TextMeshProUGUI nextBoostScoreText3;
    [SerializeField] TextMeshProUGUI nextBoostScoreText4;

    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] TextMeshProUGUI PlayRollText;


    PlayerManager playerManager1;
    PlayerManager playerManager2;
    PlayerManager playerManager3;
    PlayerManager playerManager4;

    float delayTime = 1.3f;
    float time = 0f;
    float speed = 3.0f;

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
        // 普段は非表示
        fScoreText1view.gameObject.SetActive(false);
        fScoreText2view.gameObject.SetActive(false);
        fScoreText3view.gameObject.SetActive(false);
        fScoreText4view.gameObject.SetActive(false);

        nextBoostScoreText1.gameObject.SetActive(false);
         nextBoostScoreText2.gameObject.SetActive(false);
        nextBoostScoreText3.gameObject.SetActive(false);
        nextBoostScoreText4.gameObject.SetActive(false);

    }

    void Update()
    {
        PlayRollText.color = GetAlphaColor(PlayRollText.color);

        AvtiveNextBoostScoreText();

    }

    void NextBoostScoreCheck(PlayerManager playerManager , TextMeshProUGUI tmp)
    {
        if (playerManager.nextDoraCount == 1)
        {
            tmp.gameObject.SetActive(false);

        }
        else
        {
            tmp.text = "×" + playerManager.nextDoraCount;
            tmp.gameObject.SetActive(true);
        }
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * speed;
        color.a = Mathf.Sin(time);

        return color;
    }

    public void InputPlayerUIData(PlayerManager pM1, PlayerManager pM2, PlayerManager pM3, PlayerManager pM4)
    {
        playerManager1 = pM1;
        playerManager2 = pM2;
        playerManager3 = pM3;
        playerManager4 = pM4;

        // 各プレイヤーのスコアに変化があったらUpdateScoreスコアを実行する
        playerManager1.score.Subscribe(score => UpdateScore(score, scoreText1));// 10000→20000だったらすぐ変化させる。
        playerManager2.score.Subscribe(score => UpdateScore(score, scoreText2));
        playerManager3.score.Subscribe(score => UpdateScore(score, scoreText3));
        playerManager4.score.Subscribe(score => UpdateScore(score, scoreText4));

        // 各プレイヤーの増減値スコアに変化があった場合、MoveFluctuationScoreを実行する
        playerManager1.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText1view));
        playerManager2.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText2view));
        playerManager3.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText3view));
        playerManager4.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText4view));
    }

    public void InputPlayerUIData(PlayerManager pm)
    {
        playerManager1 = pm;
        // 各プレイヤーのスコアに変化があったらUpdateScoreスコアを実行する
        playerManager1.score.Subscribe(score => UpdateScore(score, scoreText1));// 10000→20000だったらすぐ変化させる。
        // 各プレイヤーの増減値スコアに変化があった場合、MoveFluctuationScoreを実行する
        playerManager1.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText1view));
    }

    //void EaseScore(int firstscore, int lastscore, int i, int fluctuationScore, bool code)// 0がマイナス
    //{
    //    // マンズとかのとき通る
    //    if (firstscore is 0)
    //    {
    //        playerManagerDatas[i].fluctuationScore.Value = fluctuationScore;
    //    }
    //    else
    //    {
    //        //スコア 増減値アニメーション
    //        if (code)
    //            playerManagerDatas[i].fluctuationScore.Value = fluctuationScore;
    //        else
    //            playerManagerDatas[i].fluctuationScore.Value = -fluctuationScore;


    //        // スコアアニメーション
    //        DOTween.To(
    //            () => playerManagerDatas[i].score.Value,
    //            num => playerManagerDatas[i].score.Value = num,
    //            lastscore,
    //            1.4f
    //            );
    //    }

    //    // リセット
    //    playerManagerDatas[i].fluctuationScore.Value = 0;
    //}


    // 点数の上限アニメーション
    void MoveFluctuationScore(int fluctuationScore, TextMeshProUGUI tmp)
    {
        if (fluctuationScore != 0)
        {
            tmp.gameObject.SetActive(true);
            tmp.text = fluctuationScore.ToString();

            if (fluctuationScore > 0)
                tmp.color = Color.red;
            else
                tmp.color = Color.blue;


            DOTween.ToAlpha(() => tmp.color,
             color => tmp.color = color,
            1f,
            delayTime
          ).OnComplete(() => DOTween.ToAlpha(() => tmp.color,
           color => tmp.color = color,
          0f,
         delayTime
          ).OnComplete(() => tmp.gameObject.SetActive(false))
            );
        }
    }

    // 点数の更新
    void UpdateScore(int score, TextMeshProUGUI player)
    {
        if (player == scoreText1)
        {
            scoreText1.text = "P1 " + score;
        }
        else if (player == scoreText2)
        {
            scoreText2.text = "P2 " + score;
        }
        else if (player == scoreText3)
        {
            scoreText3.text = "P3 " + score;
        }
        else if (player == scoreText4)
        {
            scoreText4.text = "P4 " + score;
        }
    }

    // 次のターンの掛け率を表示するスコアを表示
    void AvtiveNextBoostScoreText()
    {
        NextBoostScoreCheck(playerManager1, nextBoostScoreText1);
        NextBoostScoreCheck(playerManager2, nextBoostScoreText2);
        NextBoostScoreCheck(playerManager3, nextBoostScoreText3);
        NextBoostScoreCheck(playerManager4, nextBoostScoreText4);
    }

    // ターンUIを変化させる
    public void UpdateTurnUI(int turnPlayer)
    {
        turnText.text = "P" + turnPlayer + "のターン";
    }

    public void UpdatePlayRollText(bool b)
    {
        PlayRollText.gameObject.SetActive(b);
    }

}
