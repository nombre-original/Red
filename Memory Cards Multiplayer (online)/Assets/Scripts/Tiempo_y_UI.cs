using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tiempo_y_UI : MonoBehaviour{
    private float tiempoLimite;
    private float tiempoMostrar;
    public bool jugando;
    [SerializeField]
    private TMP_Text tiempo_txt;

    void Start(){
        tiempoLimite = 60f;
        jugando = true;
    }

    void Update(){
        if (!jugando){
            tiempoLimite = 60f;
            tiempo_txt.text = ("Esperando jugadores...");
        }
        if (jugando){
            Partida_Nueva();
        }
    }

    private void Partida_Nueva(){
        if (tiempoLimite>0){
            tiempoMostrar = Mathf.FloorToInt(tiempoLimite);
            tiempo_txt.text = ("Tiempo: " + tiempoMostrar);
            tiempoLimite -= Time.deltaTime;
        } else {
            tiempo_txt.text = ("¡Fin!");
        }
    }
}