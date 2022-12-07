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
    // �����̔v�f�[�^
    HaiData haiData;

    void Start()
    {
        // ���g�̔v�f�[�^��ۑ�
        if(!GameManager.instance.gameStart)
        {
            haiData = GameManager.instance.SetData(int.Parse(gameObject.name));
        }
    }

    void Update()
    {
        // �L�[�{�[�h�̏�Ԃ̎擾
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            //�@�X�y�[�X�L�[�̏�Ԃ̎擾
            if (keyboard.zKey.wasPressedThisFrame)
            {
                SpisnMahjongHai();
            }
        }


    }

    /// <summary>
    /// �v���̂Ƀf�[�^���i�[����
    /// </summary>
    public void InputHaiData()
    {

    }

    /// <summary>
    /// �v����]�����ė��ƕ\���t�ɂ���
    /// </summary>
    public async void SpinMahjongHai(HaiData h,bool isDora)
    {
        if(!isDora)
        {
            // �~�܂����v�̏�Ԃ��擾����
            var scale = transform.localScale;
            var haiRotation = transform.localEulerAngles;


            //�v��傫������
            transform.DOScale(scale * 4, 0.5f);

            // ��x����]���Ă��Ȃ�������P�W�O�x��]������
            if (!h.isObverse)
            {
                transform.DORotate(new Vector3(180f, 180f, -90f), 0.5f);
            }
            else
            {
                transform.DORotate(new Vector3(180f, 180f, -90f), 0.5f);
            }

            // �����邽�߂ɂ�����ƒx��
            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            //���ɖ߂�
            transform.DOScale(scale, 0.5f);

            // ���ɂ߂���Ă����炻�̂܂܂ɂ���
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
            // ��x����]���Ă��Ȃ��������]������
            if (!h.isObverse)
            {
                // �R�}����]������(�v���X�E�}�C�i�X�Ō����ύX�\)
                transform.DORotate(new Vector3(0f, -180f, 0f), 0.5f, RotateMode.LocalAxisAdd);
                h.isObverse = true;
            }
        }




        // �~�܂����v�̊p�x���擾����


       // Debug.Log(haiRotation);



        // �v�𐳖ʌ����ɂ���



    }

    public void SpisnMahjongHai()
    {
            // �R�}����]������(�v���X�E�}�C�i�X�Ō����ύX�\)
            transform.DORotate(new Vector3(0f, -180f, 0f), 0.5f, RotateMode.LocalAxisAdd);
    }
}
