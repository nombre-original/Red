using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class SCR_Juego : NetworkBehaviour{
    public GameObject carta;
    public Sprite[] tipos;

    private List<Sprite> spritesElegidos;
    private List<int> listaId;
    private int baraja;
    private int numAleatorio;

    // Posición de las cartas
    private float x;
    private float y;
    private float sepX;
    private float sepY;

    void Start(){
        if (!NetworkManager.Singleton.IsServer) return;

        baraja = 8;
        x = -7.5f;
        y = 2f;
        sepX = 5f;
        sepY = 4f;

        listaId = new List<int>(baraja);
        spritesElegidos = new List<Sprite>(baraja);

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
                listaId.Add(numAleatorio);
                listaId.Add(numAleatorio);
                i += 2;
            }
        }
    }

    private void Mezclar(){
        for (int i = baraja - 1; i > 0; i--){
            numAleatorio = Random.Range(0, i + 1);

            int elegidoId = listaId[i];
            listaId[i] = listaId[numAleatorio];
            listaId[numAleatorio] = elegidoId;

            Sprite elegidoSprite = spritesElegidos[i];
            spritesElegidos[i] = spritesElegidos[numAleatorio];
            spritesElegidos[numAleatorio] = elegidoSprite;
        }
    }

    private void Repartir(){
        float xOriginal = x;
        for (int i = 0; i < baraja; i++){
            GameObject nueva = Instantiate(carta, new Vector2(x, y), Quaternion.identity);
            var sc = nueva.GetComponent<SCR_Carta>();
            var net = nueva.GetComponent<NetworkObject>();
            sc.MarcarId(listaId[i]);
            net.Spawn();
            x += sepX;
            if (x > 8f){
                y -= sepY;
                x = xOriginal;
            }
        }
    }

    public void CartaElegidaServer(ulong requesterClientId, ulong cartaNetworkId, int tipoId){
        Debug.Log("carta es elegida");
    }
}
