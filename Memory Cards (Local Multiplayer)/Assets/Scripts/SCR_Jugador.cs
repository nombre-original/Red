using UnityEngine;
using Unity.Netcode;

public class SCR_Jugador : NetworkBehaviour{
    [SerializeField] int velocidad = 5;

    private SCR_Carta cartaTocada;

    void Update(){
        if (!IsOwner) return;

        Vector3 dir = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"),
            0f
        ).normalized;

        transform.position += dir * velocidad * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && cartaTocada != null){
            RequestFlipServerRpc(cartaTocada.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        var carta = other.GetComponent<SCR_Carta>();
        if (carta != null){
            cartaTocada = carta;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        var carta = other.GetComponent<SCR_Carta>();
        if (carta != null && carta == cartaTocada)
            cartaTocada = null;
    }

    [ServerRpc]
    private void RequestFlipServerRpc(ulong cartaNetworkId){
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(cartaNetworkId, out var netObj)){
            var carta = netObj.GetComponent<SCR_Carta>();
            if (carta != null){
                carta.SetBocaArribaServer(!carta.EstaBocaArriba());
            }
        }
    }
}
