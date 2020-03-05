using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Breno.Systema {
	public class ConectionManager : MonoBehaviourPunCallbacks
	{
        public GameObject Player; 
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnected()
        {
            Debug.Log("Connected server...");
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Serve: " + PhotonNetwork.CloudRegion + " Ping: " + PhotonNetwork.GetPing());
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconected because: " + cause);
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined");
            RoomOptions rn = new RoomOptions();
            rn.MaxPlayers = 4;
            PhotonNetwork.JoinOrCreateRoom("MyRoom", rn, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.Instantiate(Player.name, Player.transform.position, Player.transform.rotation, 0);
        }
    }
}
