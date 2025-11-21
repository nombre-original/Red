using UnityEngine;
using Unity.Netcode;

public class Conexiones : MonoBehaviour{
    public void ConectarHost(){
        NetworkManager.Singleton.StartHost();
    }
    public void ConectarCliente(){
        NetworkManager.Singleton.StartClient();
    }
    public void Desconectar(){
        NetworkManager.Singleton.Shutdown();
    }
}
