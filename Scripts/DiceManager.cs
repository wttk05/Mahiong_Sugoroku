using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
public class DiceManager : MonoBehaviour
{
    // �C���X�^���X���쐬
    public static DiceManager instance;

    [SerializeField] GameObject dicePrefab;

    //2�������邽��
    [SerializeField] Vector3 dicePos1;
    [SerializeField] Vector3 dicePos2;

    // �T�C�R��
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
        //�܂��Q����
        dice1 = Instantiate(dicePrefab, dicePos1, Quaternion.identity);
        dice2 = Instantiate(dicePrefab, dicePos2, Quaternion.identity);

        dice1rb = dice1.GetComponent<Rigidbody>();
        dice2rb = dice2.GetComponent<Rigidbody>();

        dice1.SetActive(false);
        dice2.SetActive(false);
    }

    void Update()
    {
        // ��~������Œ肵�Ēl�����
        if (isRoll)
        {
            if (dice1rb.IsSleeping() && move1)
            {
                if (dice2rb.IsSleeping() && move2)
                {
                    var num1 = CheckDiceNum(dice1rb, dice1);
                    var num2 = CheckDiceNum(dice2rb, dice2);

                    // �T�C�̖ڂ̍��v�l���擾
                    if (!cheat)
                    {
                        plusDiceNum =num1 + num2;
                    }
                    else
                    {
                        plusDiceNum = cheatNum;
                    }

                    GameManager.instance.DiceEnd(plusDiceNum);
                    
                    // ���������Ȃ��悤�Ƀt���O��߂��Ă���
                    //isroll = false;
                    move1 = false;
                    move2 = false;
                }
            }
        }

    }

    /// <summary>
    /// �_�C�X�𐶐�����
    /// </summary>
    void CreateDice()
    {
        isRoll = true;
        PlayerUI.instance.UpdatePlayRollText(false);

        //�T�C�R����
        dice1.SetActive(true);
        dice2.SetActive(true);
    }

    /// <summary>
    /// �_�C�X�̖ڂ𒲂ׂ�
    /// </summary>
    private int CheckDiceNum(Rigidbody rb , GameObject obj)
    {
            // ���������~������
            rb.constraints = RigidbodyConstraints.FreezeAll;

            var children = new Transform[6];

            // �q�i�ʂ��Ƃ̃g�����X�t�H�[�����擾�j
            for (int i = 0;i< children.Length; i++)
            {
                children[i] = obj.transform.GetChild(i);
            }

            // �P��Y�̒l�������q���o��
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
    /// �_�C�X��U�鏈��
    /// </summary>
    public void CallDice()
    {
        if (isRoll)
            return;

        // �܂�����
        CreateDice();

        // �S�[���Ɍ������ē�����
        MoveDice();

    }

    void MoveDice()
    {
        //�T�C�R���Ɉ�x�����͂�������i�K���j
        dice1rb.AddForce(new Vector3(-200.0f, 0, 0.3f));
        dice2rb.AddForce(new Vector3(-200.0f, 0, 0.3f));

        // ������͂��T�o�ɂ킽���Ɠ�������������i�Ǝv���j
        dice1rb.angularVelocity = new Vector3(Random.Range(-400.0f, 400.0f), Random.Range(-400.0f, 400.0f), Random.Range(-400.0f, 400.0f));
        dice2rb.angularVelocity = new Vector3(Random.Range(-400.0f, 400.0f), Random.Range(-400.0f, 400.0f), Random.Range(-400.0f, 400.0f));

        move1 = true;
        move2 = true;
    }

    public void DeleteDice()
    {
        // �_�C�X�������ʒu�ɖ߂�
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
