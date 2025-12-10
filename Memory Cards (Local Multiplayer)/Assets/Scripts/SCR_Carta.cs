using UnityEngine;
using Unity.Netcode;

public class SCR_Carta : NetworkBehaviour{
    private SpriteRenderer sr;
    public Sprite reverso;

    private NetworkVariable<int> tipoId = new NetworkVariable<int>(
        -1,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private NetworkVariable<bool> bocaArriba = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    void Awake(){
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = reverso;
    }

    public override void OnNetworkSpawn(){
        if (IsClient){
            tipoId.OnValueChanged += (_, __) => ActualizarSprite();
            bocaArriba.OnValueChanged += (_, __) => ActualizarSprite();
            ActualizarSprite();
        }
    }

    private void ActualizarSprite(){
        if (sr == null) return;
        if (tipoId.Value < 0 || !bocaArriba.Value){
            sr.sprite = reverso;
            return;
        }
        var juego = Object.FindFirstObjectByType<SCR_Juego>();
        if (juego == null || juego.tipos == null) { sr.sprite = reverso; return; }
        int id = tipoId.Value;
        if (id >= 0 && id < juego.tipos.Length) sr.sprite = juego.tipos[id];
        else sr.sprite = reverso;
    }


    public void MarcarId(int id){
        if (!IsServer) return;
        tipoId.Value = id;
    }

    public void SetBocaArribaServer(bool valor){
        if (!IsServer) return;
        bocaArriba.Value = valor;
    }

    [ServerRpc]
    public void FlipCartaServerRpc(bool valor, ServerRpcParams rpcParams = default){
        bocaArriba.Value = valor;
    }

    [ServerRpc]
    public void EmparejadaServerRpc(){
        var netObj = GetComponent<NetworkObject>();

        if (netObj != null && netObj.IsSpawned)
            netObj.Despawn(true);
        else
            Destroy(gameObject);
    }

    public int EnviarId() => tipoId.Value;
    public bool EstaBocaArriba() => bocaArriba.Value;
}