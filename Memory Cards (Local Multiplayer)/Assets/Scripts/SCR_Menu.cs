using UnityEngine;
using Unity.Netcode;

public class SCR_Menu : MonoBehaviour{
    public GameObject panelMenu;
    public GameObject Button_Desconectar;
    public GameObject Tiempo;
    public GameObject Juego;

    public void ConectarHost(){
        NetworkManager.Singleton.StartHost();
        Tiempo.SetActive(true);
        Button_Desconectar.SetActive(true);
        panelMenu.SetActive(false);
        Juego.SetActive(true);
    }
    public void ConectarCliente(){
        NetworkManager.Singleton.StartClient();
        Tiempo.SetActive(true);
        Button_Desconectar.SetActive(true);
        panelMenu.SetActive(false);
        Juego.SetActive(true);
    }
    public void Desconectar(){
        NetworkManager.Singleton.Shutdown();
        Tiempo.SetActive(false);
        Juego.SetActive(false);
        Button_Desconectar.SetActive(false);
        panelMenu.SetActive(true);
    }
}