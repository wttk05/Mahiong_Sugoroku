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
    // viewにあたるらしい
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
        // 生成する
        //gameObject.AddComponent<NetworkRunner>();
        var vtr = gameObject.GetComponent<NetworkRunner>();
        vtr.ProvideInput = true;

        // ホストorクライアント
        vtr.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });

        //Debug.Log("せいせいするよ");
    }
    //-------------------------------------------------------------------------------------------------------------------------------
    private void CreatePlayer(NetworkRunner runner, PlayerRef player)// 入ってきたプレイヤーの番号
    {
        if (player == 9)// Host
        {             
            // 自分の機体生成
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
            networkKoma.name = "Player003";// 名前変更

            pmKoma = networkKoma.GetComponent<PlayerManager>();
            pmKoma.nextDoraCount = 1;
            pmKoma.color = Color.yellow;

            spawnedCharacters.Add(player, networkKoma);

        }
        else if (player == 2)
        {
            networkKoma = runner.Spawn(whiteKoma, new Vector3(4.5f, 0f, -1.6f), Quaternion.identity, player);
            networkKoma.name = "Player004";// 名前変更

            pmKoma = networkKoma.GetComponent<PlayerManager>();
            pmKoma.nextDoraCount = 1;
            pmKoma.color = Color.white;

            spawnedCharacters.Add(player, networkKoma);
        }
        else
        {
            Debug.Log("お前はただそこにいるだけの空虚な存在");
        }

        // 自身のUIをセット
        //PlayerUI.instance.InputPlayerUIData(pmKoma);
    }
//-------------------------------------------------------------------------------------------------------------------------------


    // 接続された時
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        Debug.Log(player+"が接続しました");
        Debug.Log(runner + "が接続しました");
        CreatePlayer(runner,player);
    }

    // 切断された時
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        runner.Despawn(networkKoma);
        spawnedCharacters.Remove(player);

        Debug.Log(player + "が切断されました");

        if(player == 9)
        {
            Debug.Log("ホストが切断しましたGG\nゲームを終了します。");
            SceneManager.LoadScene("title");
        }
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("接続完了"); }
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
        // ここ通ってたらしく、例外エラーで先に進めなかった
        //throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }
}
