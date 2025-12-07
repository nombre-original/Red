using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class SCR_Juego : NetworkBehaviour{
    public GameObject carta;
    public Sprite[] tipos;
    private List<Sprite> spritesElegidos;
    private List<int> listaId;//como string pero adaptable
    private int baraja;
    private int numAleatorio;

    //Posición de las cartas
    private float x;
    private float y;
    private float sepX;
    private float sepY;

    void Start(){
        //si no se ha conectado el host
        if (!NetworkManager.Singleton.IsServer) return;
        baraja = 8;
        x = -7.5f;
        y = 2f;
        sepX = 5f;
        sepY = 4f;

        listaId = new List<int>(baraja);
        spritesElegidos = new List<Sprite>();

        NuevoTablero();
    }

    private void NuevoTablero(){
        Elegir();
        Mezclar();
        Repartir();
    }

    private void Elegir(){
        spritesElegidos.Clear();
        listaId.Clear();
        int i = 0;
        while (i < baraja){
            numAleatorio = Random.Range(0, tipos.Length);
            if (!spritesElegidos.Contains(tipos[numAleatorio])){
                spritesElegidos.Add(tipos[numAleatorio]);
                spritesElegidos.Add(tipos[numAleatorio]);
                listaId.Add(i);
                listaId.Add(i + 1);
                i += 2;
            }
        }
    }

    private void Mezclar(){
        for (int i=baraja-1; i>0; i--){
            numAleatorio = Random.Range(0,i+1);
            //desorden ID
            int elegidoId = listaId[i];
            listaId[i] = listaId[numAleatorio];
            listaId[numAleatorio] = elegidoId;
            //desorden Sprites
            Sprite elegidoSprite = spritesElegidos[i];
            spritesElegidos[i] = spritesElegidos[numAleatorio];
            spritesElegidos[numAleatorio] = elegidoSprite;
        }
    }

    private void Repartir(){
        float xOriginal = x;
        for (int i=0; i<baraja; i++){
            GameObject nuevaCarta = Instantiate(carta, new Vector2(x, y), Quaternion.identity);
            var novaCarta = nuevaCarta.GetComponent<SCR_Carta>();
            var netObj = nuevaCarta.GetComponent<NetworkObject>();
            novaCarta.MarcarId(listaId[i]);
            netObj.Spawn();
            x += sepX;
            if (x>8f){ //Últimas 4 cartas
                y -= sepY;
                x = xOriginal;
            }
        }
    }

    public void CartaElegidaServer(ulong requesterClientId, ulong cartaNetworkId, int tipoId){
        Debug.Log("carta es elegida");
    }
}
