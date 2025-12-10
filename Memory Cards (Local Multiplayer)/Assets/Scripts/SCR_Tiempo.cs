using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class SCR_Tiempo : MonoBehaviour{
    [SerializeField]
    private TMP_Text tiempo_txt;
    private float tiempoLimite;
    private float tiempoMostrar;
    public bool jugando;

    void Start(){
        tiempoLimite = 60f;
        jugando = true;
    }

    void Update(){
        if (!NetworkManager.Singleton.IsServer) jugando = false;
        if (!jugando){
            tiempoLimite = 60f;
            tiempo_txt.text = ("Esperando jugadores...");
        }
        if (jugando){
            Partida_Nueva();
        }
    }

    void Partida_Nueva(){
        if (tiempoLimite>0){
            tiempoMostrar = Mathf.FloorToInt(tiempoLimite);
            tiempo_txt.text = ("Tiempo: " + tiempoMostrar);
            tiempoLimite -= Time.deltaTime;
        } else {
            tiempo_txt.text = ("¡Fin!");
        }
    }
}
