using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Menu : MonoBehaviour{
    public GameObject CanvasMenu;
    public GameObject CanvasAvatar;
    
    public void ConectarHost(){
        CanvasMenu.SetActive(false);
        CanvasAvatar.SetActive(true);
    }

    public void ConectarCliente(){

    }

    public void Desconectar(){

    }

    public void Volver(){
        CanvasAvatar.SetActive(false);
        CanvasMenu.SetActive(true);
    }

    public void ElegirAvatar(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
