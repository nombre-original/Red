using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GestionConectar : MonoBehaviour{
    public static float puntos = 15;

    public void conectarComoHost(){
        NetworkManager.Singleton.StartHost();
    }

    public void conectarComoCliente(){
        NetworkManager.Singleton.StartClient();
    }

    public void desconectar(){
        NetworkManager.Singleton.Shutdown();
    }
}
