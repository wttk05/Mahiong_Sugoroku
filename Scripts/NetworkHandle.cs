using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;

public class NetworkHandle : MonoBehaviour
{
    // �C���X�^���X���쐬
    public static NetworkHandle instance;

    // �l�b�g���[�N�p�v���n�u
    [SerializeField] NetworkRunner networkRunnerPrefab;
    private  NetworkRunner networkRunner;

    private void Start()
    {
        CreateNetworkRunner();
    }

    public void CreateNetworkRunner()
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network Runner";
    }


    /// <summary>
    /// �l�b�g���[�N�Q�[�����J�n����
    /// </summary>
    //public async UniTask StartGame()
    //{
    //    runner = Instantiate(networkRunnerPrefab);
    //    runner.ProvideInput = true;

    //    var result = await runner.StartGame(new StartGameArgs
    //    {
    //        GameMode = GameMode.AutoHostOrClient,
    //        SessionName = "TestRoom",
    //        Scene = SceneManager.GetActiveScene().buildIndex,
    //        SceneManager = runner.GetComponent<NetworkSceneManagerDefault>()
    //    });
    //}
}
