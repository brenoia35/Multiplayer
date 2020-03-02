using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// acesso a UI
using UnityEngine.UI;

// === Abilitando o photon no script ===
using Photon.Pun;
using Photon.Realtime;
// =====================================


namespace FenyxBrasil.Photon
{
    public class NetworkController : MonoBehaviourPunCallbacks
    {

        [Header("Login")]
        public GameObject login;
        public GameObject partidas;

        [Space]
        [Header("Player")]
        public InputField playerNameInput;
        string playerNameTemp;

        [Space]
        [Header("Room")]
        public InputField roomName;


        //bool isConnected = false;



        void Start()
        {
            // chama a conexãocom o servidor da photon usando as configurações
            // PhotonNetwork.ConnectUsingSettings();


            // criando nome temporario do jogador e passando para o imput
            playerNameTemp = "Jogador" + Random.Range(1000, 10000);
            
            // so repassar se a pessoa não digitar nada
            playerNameInput.text = playerNameTemp;

            // criando nome temporario da sala e passando para o imput
            roomName.text = "sala" + Random.Range(1000, 10000);


            // reconfigurando paineis de login
            login.gameObject.SetActive(true);
            partidas.gameObject.SetActive(false);

        }




        void Update()
        {
            // Se estiver desconectado, tente conectar novamente.
            //if(isConnected == false)
            //{
            //    // chama a conexãocom o servidor da photon usando as configurações
            //    PhotonNetwork.ConnectUsingSettings();
            //}

        }




        public void Login()
        {

            if (playerNameInput.text != "")
            {
                //PhotonNetwork.NickName = playerNameTemp;
                PhotonNetwork.NickName = playerNameInput.text;
            }

            else
            {
                //PhotonNetwork.NickName = playerNameInput.text;
                PhotonNetwork.NickName = playerNameTemp;
            }


            // conectar na sala do photon usando as configurações
            PhotonNetwork.ConnectUsingSettings();


            // reconfigurando paineis de login
            login.gameObject.SetActive(false);



        }



        // buscar as partidas existentes
        public void BotaoBuscaPartidaRapida()
        {
            // entrar em um loby
            PhotonNetwork.JoinLobby();
        }



        // criando uma sala
        public void BotaoCriarSala()
        {
            // variavel para receber o nome da sala do imput
            string roomNameTemp = roomName.text;
            
            // definir a quantidade de players
            RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 4 };
            
            //   (nome da sala, quantidade de jogadores, tipo de loby)
            PhotonNetwork.JoinOrCreateRoom(roomNameTemp, roomOptions, TypedLobby.Default);
        }


        


        // ##############################    PunCallbacks  ###############################

        // quando a conexão com o photon é realizada...
        // conexão
        public override void OnConnected()
        {
            
            Debug.Log("OnConnected");


            // quando houver conexão, passa a variavel para verdadeiro
            //isConnected = true;


        }




        // chamada quando a conexão é realizada e validada
        // validação
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            Debug.Log("Server: " + PhotonNetwork.CloudRegion + " Ping: " + PhotonNetwork.GetPing());

            // entrar em um loby
            // ao entrar em um lobby, chama OnJoinedLobby()
            // desativado... entrar pelo botão
            // PhotonNetwork.JoinLobby();


            // reconfigurando paineis de login
            // habilitar a tela somente depois de autenticar
            partidas.gameObject.SetActive(true);

        }


        

        // assim que entrar em um loby, chamar uma sala
        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");

            // tentar entrar em uma sala existente
            // caso dê erro, ele chama OnJoinRandomFailed(short returnCode, string message)
            PhotonNetwork.JoinRandomRoom();
        }




        // caso dê erro ao entrar em uma sala porque ela não existe, ai criamos uma sala
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // variavel temporaria para criarmos um nome randomico para evitar conflito
            string rootemp = "Sala" + Random.Range(1000, 10000);

            // criar a sala com o nome randomico gerado
            // quando entrar, chama o OnJoinedRoom()
            PhotonNetwork.CreateRoom(rootemp);

        }




        // chamado quando entramos em uma sala
        public override void OnJoinedRoom()
        {
            // ao entrar na sala
            Debug.Log("OnJoinedRoom");

            // informando o nome da sala que entrou
            Debug.Log("Nome da Sala: " + PhotonNetwork.CurrentRoom.Name);

            // quantidade de jogadores na sala
            Debug.Log("Quantidade de Players : " + PhotonNetwork.CurrentRoom.PlayerCount);
        }




        // chamada quando é desconectada do servidor da photon
        public override void OnDisconnected(DisconnectCause cause)
        {
            // retorna a ccausa do erro
            Debug.Log("OnDisconnected: " + cause);


            // No site da photon tem a sigla para cada servidor
            // faz a mudança para outro servidor... exemplo "eu" que é do Estados Unidos
            //PhotonNetwork.ConnectToRegion("eu");


            // quando perder a conexão, passar a variavel para falso
            // deposi de mudar de servidor com o PhotonNetwork.ConnectToRegion("eu")
            //isConnected = false;


        }

        // ##############################    PunCallbacks  ###############################





    }
}
