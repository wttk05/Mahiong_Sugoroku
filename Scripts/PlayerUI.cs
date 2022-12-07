using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;

public class PlayerUI : MonoBehaviour
{
    // �C���X�^���X���쐬
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
        // �C���X�^���X����
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
        // ���i�͔�\��
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
            tmp.text = "�~" + playerManager.nextDoraCount;
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

        // �e�v���C���[�̃X�R�A�ɕω�����������UpdateScore�X�R�A�����s����
        playerManager1.score.Subscribe(score => UpdateScore(score, scoreText1));// 10000��20000�������炷���ω�������B
        playerManager2.score.Subscribe(score => UpdateScore(score, scoreText2));
        playerManager3.score.Subscribe(score => UpdateScore(score, scoreText3));
        playerManager4.score.Subscribe(score => UpdateScore(score, scoreText4));

        // �e�v���C���[�̑����l�X�R�A�ɕω����������ꍇ�AMoveFluctuationScore�����s����
        playerManager1.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText1view));
        playerManager2.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText2view));
        playerManager3.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText3view));
        playerManager4.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText4view));
    }

    public void InputPlayerUIData(PlayerManager pm)
    {
        playerManager1 = pm;
        // �e�v���C���[�̃X�R�A�ɕω�����������UpdateScore�X�R�A�����s����
        playerManager1.score.Subscribe(score => UpdateScore(score, scoreText1));// 10000��20000�������炷���ω�������B
        // �e�v���C���[�̑����l�X�R�A�ɕω����������ꍇ�AMoveFluctuationScore�����s����
        playerManager1.fluctuationScore.Subscribe(fscore => MoveFluctuationScore(fscore, fScoreText1view));
    }

    //void EaseScore(int firstscore, int lastscore, int i, int fluctuationScore, bool code)// 0���}�C�i�X
    //{
    //    // �}���Y�Ƃ��̂Ƃ��ʂ�
    //    if (firstscore is 0)
    //    {
    //        playerManagerDatas[i].fluctuationScore.Value = fluctuationScore;
    //    }
    //    else
    //    {
    //        //�X�R�A �����l�A�j���[�V����
    //        if (code)
    //            playerManagerDatas[i].fluctuationScore.Value = fluctuationScore;
    //        else
    //            playerManagerDatas[i].fluctuationScore.Value = -fluctuationScore;


    //        // �X�R�A�A�j���[�V����
    //        DOTween.To(
    //            () => playerManagerDatas[i].score.Value,
    //            num => playerManagerDatas[i].score.Value = num,
    //            lastscore,
    //            1.4f
    //            );
    //    }

    //    // ���Z�b�g
    //    playerManagerDatas[i].fluctuationScore.Value = 0;
    //}


    // �_���̏���A�j���[�V����
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

    // �_���̍X�V
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

    // ���̃^�[���̊|������\������X�R�A��\��
    void AvtiveNextBoostScoreText()
    {
        NextBoostScoreCheck(playerManager1, nextBoostScoreText1);
        NextBoostScoreCheck(playerManager2, nextBoostScoreText2);
        NextBoostScoreCheck(playerManager3, nextBoostScoreText3);
        NextBoostScoreCheck(playerManager4, nextBoostScoreText4);
    }

    // �^�[��UI��ω�������
    public void UpdateTurnUI(int turnPlayer)
    {
        turnText.text = "P" + turnPlayer + "�̃^�[��";
    }

    public void UpdatePlayRollText(bool b)
    {
        PlayRollText.gameObject.SetActive(b);
    }

}
