using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Breno.Systema {
	public class ConectionManager : MonoBehaviourPunCallbacks
	{
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
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconected because: " + cause);
        }
    }
}
