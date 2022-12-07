using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;


public class NetWorkManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    // view�ɂ�����炵��
    [SerializeField] private NetworkPrefabRef redKoma;
    [SerializeField] private NetworkPrefabRef blueKoma;
    [SerializeField] private NetworkPrefabRef yellowKoma;
    [SerializeField] private NetworkPrefabRef whiteKoma;

    private NetworkObject networkKoma;
    private PlayerManager pmKoma;

    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();


    private void Start()
    {
       StartGame(GameMode.AutoHostOrClient);
    }

    public  void StartGame(GameMode mode)
    {
        // ��������
        //gameObject.AddComponent<NetworkRunner>();
        var vtr = gameObject.GetComponent<NetworkRunner>();
        vtr.ProvideInput = true;

        // �z�X�gor�N���C�A���g
        vtr.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        //Debug.Log("�������������");
    }
    //-------------------------------------------------------------------------------------------------------------------------------
    private void CreatePlayer(NetworkRunner runner, PlayerRef player)// �����Ă����v���C���[�̔ԍ�
    {
        if (player == 9)// Host
        {             
            // �����̋@�̐���
            networkKoma = runner.Spawn(redKoma, new Vector3(4.2f, 0f, -1.9f), Quaternion.identity, player);

            Debug.Log(player.PlayerId);
            Debug.Log(networkKoma);

            //networkKoma.networkTransform.TeleportToPositionRotation(position, rotation);

            //pmKoma = networkKoma.GetComponent<PlayerManager>();
            //pmKoma.nextDoraCount = 1;
            //pmKoma.color = Color.red;

            spawnedCharacters.Add(player, networkKoma);
        }
        else if (player == 0)
        {
            networkKoma = runner.Spawn(blueKoma, new Vector3(4.5f, 0f, -1.9f), Quaternion.identity, player);

            Debug.Log(player.PlayerId);
            Debug.Log(networkKoma);

            spawnedCharacters.Add(player, networkKoma);
        }
        else if (player == 1)
        {
            networkKoma = runner.Spawn(yellowKoma, new Vector3(4.2f, 0f, -1.6f), Quaternion.identity, player);
            networkKoma.name = "Player003";// ���O�ύX

            pmKoma = networkKoma.GetComponent<PlayerManager>();
            pmKoma.nextDoraCount = 1;
            pmKoma.color = Color.yellow;

            spawnedCharacters.Add(player, networkKoma);

        }
        else if (player == 2)
        {
            networkKoma = runner.Spawn(whiteKoma, new Vector3(4.5f, 0f, -1.6f), Quaternion.identity, player);
            networkKoma.name = "Player004";// ���O�ύX

            pmKoma = networkKoma.GetComponent<PlayerManager>();
            pmKoma.nextDoraCount = 1;
            pmKoma.color = Color.white;

            spawnedCharacters.Add(player, networkKoma);
        }
        else
        {
            Debug.Log("���O�͂��������ɂ��邾���̋󋕂ȑ���");
        }

        // ���g��UI���Z�b�g
        //PlayerUI.instance.InputPlayerUIData(pmKoma);
    }
//-------------------------------------------------------------------------------------------------------------------------------


    // �ڑ����ꂽ��
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        Debug.Log(player+"���ڑ����܂���");
        Debug.Log(runner + "���ڑ����܂���");
        CreatePlayer(runner,player);
    }

    // �ؒf���ꂽ��
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        runner.Despawn(networkKoma);
        spawnedCharacters.Remove(player);

        Debug.Log(player + "���ؒf����܂���");

        if(player == 9)
        {
            Debug.Log("�z�X�g���ؒf���܂���GG\n�Q�[�����I�����܂��B");
            SceneManager.LoadScene("title");
        }
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("�ڑ�����"); }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // �����ʂ��Ă��炵���A��O�G���[�Ő�ɐi�߂Ȃ�����
        //throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }
}
