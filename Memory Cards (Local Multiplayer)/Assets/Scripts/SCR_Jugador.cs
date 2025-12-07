using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour{
    [SerializeField]
    int velocidad = 5;

    private SCR_Carta cartaTocada;//si funciona...he borrado demasiadas cosas ya, porfa funciona

    void Update(){
        if (!IsOwner) return;

        //MOVIMIENTO
        Vector3 direccion = new Vector3();
        direccion.x = Input.GetAxis("Horizontal");
        direccion.y = Input.GetAxis("Vertical");
        direccion.Normalize();
        transform.position = transform.position + direccion * velocidad * Time.deltaTime;

        //ABRIR CARTAS
        if (Input.GetKeyDown(KeyCode.Space) && cartaTocada != null){
            VoltearServerRPC(cartaTocada.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }

    //si sobre carta
    void OnTriggerEnter2D(Collider2D other){
        var carta = other.GetComponent<SCR_Carta>();
        if (carta != null){
            cartaTocada = carta;
        }
    }
    //si no sobre carta
    void OnTriggerExit2D(Collider2D other){
        var carta = other.GetComponent<SCR_Carta>();
        if (carta != null && carta == cartaTocada){
            cartaTocada = null;
        }
    }

    [ServerRpc]
    void VoltearServerRPC(ulong cartaNetworkId, ServerRpcParams rpcParams = default){
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(cartaNetworkId, out var netObj)){
            var carta = netObj.GetComponent<SCR_Carta>();
            if (carta != null){
                var clientParams = new ClientRpcParams{
                    Send = new ClientRpcSendParams{
                        TargetClientIds = new ulong[]{rpcParams.Receive.SenderClientId}
                    }
                };
                carta.GirarParaClienteDesdeServidor(carta.EnviarId(), clientParams);
            }
        }
    }
}
