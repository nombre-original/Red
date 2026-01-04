using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class Tiempo : NetworkBehaviour {
    public TMP_Text tiempo_txt;
    private float tiempoLimite = 60f;
    private float tiempoMostrar;
    public GameObject botonReinicio;
    //private bool jugando = false;
    public GameObject agujero;

    private NetworkVariable<float> tiempoRestante = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> jugando = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    void Start(){
        UpdateLocalButtonVisibility();
        //botonReinicio.SetActive(true);
    }

    public override void OnNetworkSpawn(){
        if (IsServer){
            tiempoRestante.Value = tiempoLimite;
            jugando.Value = false;
        }
        tiempoRestante.OnValueChanged += (oldVal, newVal) => UpdateLocalTimeText(newVal);
        jugando.OnValueChanged += (oldVal, newVal) => UpdateLocalButtonVisibility();
        UpdateLocalTimeText(tiempoRestante.Value);
        UpdateLocalButtonVisibility();
    }

    void Update(){
        if (!IsServer) return;

        if (jugando.Value){
            if (tiempoRestante.Value > 0f){
                tiempoRestante.Value -= Time.deltaTime;
            } else {
                tiempoRestante.Value = 0f;
                jugando.Value = false;
                OnGameEndServer();
            }
        }
    }

    private void UpdateLocalTimeText(float tiempo){
        tiempoMostrar = Mathf.FloorToInt(tiempo);
        if (tiempo > 0f) tiempo_txt.text = "Tiempo: " + tiempoMostrar;
        else tiempo_txt.text = "¡Fin!";
    }

    void ElDestructorDeTags(string tag){
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        for (int i = objs.Length - 1; i >= 0; i--){
            if (objs[i] != null) Destroy(objs[i]);
        }
    }

    private void UpdateLocalButtonVisibility(){
        if (botonReinicio != null) botonReinicio.SetActive(!jugando.Value);
        if (agujero != null) agujero.SetActive(jugando.Value);
    }

    public void Lobby(){
        if (IsServer) {
            StartGameServerRpc();
        } else {
            RequestStartGameServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestStartGameServerRpc(ServerRpcParams rpcParams = default){
        StartGameServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc(ServerRpcParams rpcParams = default){
        tiempoRestante.Value = tiempoLimite;
        jugando.Value = true;
    }

    private void OnGameEndServer(){
        ElDestructorDeTagsServer("1p");
        ElDestructorDeTagsServer("5p");
        ElDestructorDeTagsServer("-2p");
    }

    private void ElDestructorDeTagsServer(string tag){
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        for (int i = objs.Length - 1; i >= 0; i--){
            if (objs[i] == null) continue;
            var no = objs[i].GetComponent<Unity.Netcode.NetworkObject>();
            if (no != null && no.IsSpawned){
                no.Despawn(true);
            } else {
                Destroy(objs[i]);
            }
        }
    }
}
