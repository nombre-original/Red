using UnityEngine;
using Unity.Netcode;

public class SCR_Carta : NetworkBehaviour{
    private SpriteRenderer sr;
    public Sprite reverso;
    private NetworkVariable<int> tipoId = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    void Awake(){
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = reverso;
    }

    public void MarcarId(int id){
        if (!IsServer) return;
        tipoId.Value = id;
    }

    [ServerRpc]
    public void PedirGirarServerRpc(ulong requesterClientId, ServerRpcParams rpcParams = default){
        var clientParams = new ClientRpcParams{
            Send = new ClientRpcSendParams{
                TargetClientIds = new ulong[] {requesterClientId}
            }
        };

        GirarClientRpc(tipoId.Value, clientParams);
        
        if(IsServer){
            var juego = Object.FindFirstObjectByType<SCR_Juego>();
            juego.CartaElegidaServer(requesterClientId, GetComponent<NetworkObject>().NetworkObjectId, tipoId.Value);
        }
    }

    [ClientRpc]
    private void GirarClientRpc(int tipoIndex, ClientRpcParams clientRpcParams = default){
        var juego = Object.FindFirstObjectByType<SCR_Juego>();
        if (juego == null){
            return;
        }
        var tipos = juego.tipos;
        if (tipos == null || tipos.Length == 0){
            return;
        }
        if (tipoIndex >= 0 && tipoIndex < tipos.Length){
            sr.sprite = tipos[tipoIndex];
        }
    }

    [ServerRpc]
    public void EmparejadaServerRpc(ServerRpcParams rpcParams = default){
        var obj = GetComponent<NetworkObject>();
        if (obj != null && obj.IsSpawned){
            obj.Despawn(true);
        }else{
            Destroy(gameObject);
        }
    }

    public int EnviarId(){
        return tipoId.Value;
    }

    public void GirarParaClienteDesdeServidor(int tipoIndex, ClientRpcParams clientRpcParams){
        GirarClientRpc(tipoIndex, clientRpcParams);
    }
}