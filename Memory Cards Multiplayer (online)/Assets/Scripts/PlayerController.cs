using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour{
    [SerializeField]
    int velocidad = 5;
    [SerializeField]
    NetworkVariable<int> puntos = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Update(){
        if (!IsOwner) return;

        //MOVIMIENTO
        Vector3 direccion = new Vector3();
        direccion.x = Input.GetAxis("Horizontal");
        direccion.y = Input.GetAxis("Vertical");
        direccion.Normalize();
        transform.position = transform.position + direccion * velocidad * Time.deltaTime;

        //ABRIR CARTAS
        if (Input.GetKeyDown(KeyCode.Space)){
            AbrirServerRPC();
        }
    }

    [ServerRpc]
    void AbrirServerRPC(){
        Debug.Log("Volteando carta");
    }
}
