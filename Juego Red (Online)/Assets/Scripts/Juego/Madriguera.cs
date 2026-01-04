using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class Madriguera : NetworkBehaviour{
    //conejos
    public GameObject pingu;
    public GameObject goldenPingu;
    public GameObject nongu;
    private GameObject conejo;

    //porcentajes
    private float pinguPercent = 65f; //65
    private float goldenPinguPercent = 80f; //15

    //tiempos
    private float tiempo;
    private float tiempoInicio = 2f;
    private float aceleracion;
    private float tiempoPasado = 0f;
    private float cadaCuantoAcelerar = 2f;
    private float cuantoReducir = 0.06f;

    private Coroutine spawnerCoroutine;

    void Start(){
        tiempo = tiempoInicio;
        aceleracion = cadaCuantoAcelerar;
        spawnerCoroutine = StartCoroutine(SpawnerCoroutine());
    }

    void Update(){
        Temporizador();
    }

    private void Temporizador(){
        tiempoPasado += Time.deltaTime;

        if (tiempoPasado >= aceleracion){
            Acelerador();
            tiempoPasado = 0f;
        }
    }

    private void Acelerador(){
        float nuevoTiempo = tiempo - cuantoReducir;
        tiempo = nuevoTiempo;
    }

    IEnumerator SpawnerCoroutine(){
        while (true){
            yield return new WaitForSeconds(tiempo);
            Chistera();
        }
    }

    private void Chistera(){
        float a = Random.value * 100f;
        if (a < pinguPercent) conejo = pingu;
        if (a >= pinguPercent && a < goldenPinguPercent) conejo = goldenPingu;
        if (a >= goldenPinguPercent) conejo = nongu;
        //Instantiate (conejo, new Vector2(0,0), Quaternion.identity);
        GameObject go = Instantiate(conejo, transform.position, Quaternion.identity);
        var netObj = go.GetComponent<NetworkObject>();
        netObj.Spawn();
    }
}
