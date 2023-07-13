using Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager
    {
        [SerializeField] private string _playerName;
        [SerializeField] private InputNameField _inputNameField;
        private Dictionary<int, ShipController> _players = new Dictionary<int, ShipController>();

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnTransform = GetStartPosition();
            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            _players.Add(conn.connectionId, player.GetComponent<ShipController>());
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler(100, ReciveName);
            base.OnStartServer();
        }


        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            MessageLogin message = new MessageLogin();
            message.Login = _inputNameField.InputField.text != "" ? _inputNameField.InputField.text : _playerName;

            conn.Send(100, message);
            _inputNameField.gameObject.SetActive(false);
        }

        private void ReciveName(NetworkMessage netMsg)
        {
            _players[netMsg.conn.connectionId].PlayerName = netMsg.reader.ReadString();
            _players[netMsg.conn.connectionId].gameObject.name = _players[netMsg.conn.connectionId].PlayerName;
            
            Debug.Log($"Name {_players[netMsg.conn.connectionId].PlayerName} recived");
        }

        public class MessageLogin : MessageBase
        {
            public string Login;

            public override void Deserialize(NetworkReader reader)
            {
                Login = reader.ReadString();
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(Login);
            }
        }
    }
}