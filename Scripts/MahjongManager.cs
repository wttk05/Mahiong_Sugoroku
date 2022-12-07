using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UniRx;
using Cysharp.Threading.Tasks;
using System;

public class MahjongManager : MonoBehaviour
{
    // 自分の牌データ
    HaiData haiData;

    void Start()
    {
        // 自身の牌データを保存
        if(!GameManager.instance.gameStart)
        {
            haiData = GameManager.instance.SetData(int.Parse(gameObject.name));
        }
    }

    void Update()
    {
        // キーボードの状態の取得
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            //　スペースキーの状態の取得
            if (keyboard.zKey.wasPressedThisFrame)
            {
                SpisnMahjongHai();
            }
        }


    }

    /// <summary>
    /// 牌自体にデータを格納する
    /// </summary>
    public void InputHaiData()
    {

    }

    /// <summary>
    /// 牌を回転させて裏と表を逆にする
    /// </summary>
    public async void SpinMahjongHai(HaiData h,bool isDora)
    {
        if(!isDora)
        {
            // 止まった牌の状態を取得する
            var scale = transform.localScale;
            var haiRotation = transform.localEulerAngles;


            //牌を大きくする
            transform.DOScale(scale * 4, 0.5f);

            // 一度も回転していなかったら１８０度回転させる
            if (!h.isObverse)
            {
                transform.DORotate(new Vector3(180f, 180f, -90f), 0.5f);
            }
            else
            {
                transform.DORotate(new Vector3(180f, 180f, -90f), 0.5f);
            }

            // 見せるためにちょっと遅延
            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            //元に戻す
            transform.DOScale(scale, 0.5f);

            // 既にめくれていたらそのままにする
            if(!h.isObverse)
            {
                transform.DORotate(new Vector3(haiRotation.x-180f, haiRotation.y, haiRotation.z), 0.5f);
                h.isObverse = true;
            }
            else
            {
                transform.DORotate(new Vector3(haiRotation.x, haiRotation.y, haiRotation.z), 0.5f);
            }

        }
        else
        {
            // 一度も回転していなかったら回転させる
            if (!h.isObverse)
            {
                // コマを回転させる(プラス・マイナスで向き変更可能)
                transform.DORotate(new Vector3(0f, -180f, 0f), 0.5f, RotateMode.LocalAxisAdd);
                h.isObverse = true;
            }
        }




        // 止まった牌の角度を取得する


       // Debug.Log(haiRotation);



        // 牌を正面向きにする



    }

    public void SpisnMahjongHai()
    {
            // コマを回転させる(プラス・マイナスで向き変更可能)
            transform.DORotate(new Vector3(0f, -180f, 0f), 0.5f, RotateMode.LocalAxisAdd);
    }
}
