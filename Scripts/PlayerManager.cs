using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class PlayerManager : MonoBehaviour
{
    public GameObject prefab;　// プレイヤーのモデル
    public int posNum; // 何番目にいるか
    public Color color; // 色
    public bool turn; // 自分のターンかどうか
    public int num; // 何番目か（ターン）
    public bool isGoal;//ゴールしているかどうか
    public int nextDoraCount;
    public int ArrivalRank;// 何位か


    // 点数の初期値(外部から取得できるようにする)
    public ReactiveProperty<int> score = new ReactiveProperty<int>();
    // 点数の増減値
    public ReactiveProperty<int> fluctuationScore = new ReactiveProperty<int>();
    // 次回の点数への書け数
    public ReactiveProperty<int> nextScoreBoost = new ReactiveProperty<int>();

    private void Start()
    {
        // 初期値設定
        score.Value = 35000;
        fluctuationScore.Value = 0;
        nextScoreBoost.Value = 0;
    }

    /// <summary>
    /// サイコロの目に応じた数ステップして進む
    /// </summary>
    public void Move(Vector3 endPosition, float time)
    {
        prefab.transform.DOLocalMove(endPosition, time);
    }

}
