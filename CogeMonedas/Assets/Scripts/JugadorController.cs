using UnityEngine;
using Unity.Netcode;

public class JugadorController : NetworkBehaviour{
    [SerializeField]
    float velocidad = 5f;

    [SerializeField]
    NetworkVariable<int> vida = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField]
    NetworkVariable<int> monedas = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField]
    GameObject prefabProyectil;

    void Update(){
        //------------ movimiento
        if (!IsOwner) return;
        Vector3 direccion = new Vector3();

        direccion.x = Input.GetAxis("Horizontal");
        direccion.y = Input.GetAxis("Vertical");

        direccion.Normalize();

        transform.position = transform.position + direccion * velocidad * Time.deltaTime;

        //------------ lanzar proyectil
        if (Input.GetKeyDown(KeyCode.Space)){
            LanzarProyectilServerRPC();
        }
    }

    [ServerRpc]
    void LanzarProyectilServerRPC(){
        GameObject nuevoProyectil = Instantiate(prefabProyectil);
        nuevoProyectil.transform.position = transform.position;
        NetworkObject no = nuevoProyectil.GetComponent<NetworkObject>();
        no.Spawn();
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (!IsOwner) return;
        if (collision.collider.CompareTag("moneda")){
            monedas.Value = monedas.Value+1;
            NetworkObject no = collision.gameObject.GetComponent<NetworkObject>();
            EliminarMonedaServerRpc(no.NetworkObjectId);
        }
    }

    [ServerRpc]
    void EliminarMonedaServerRpc(ulong noId){
        NetworkObject no = NetworkManager.Singleton.SpawnManager.SpawnedObjects[noId];
        no.Despawn(); //desconectar y eliminar el problema
    }
}
