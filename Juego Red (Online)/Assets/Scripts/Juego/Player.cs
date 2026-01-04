using UnityEngine;
using TMPro;
using Unity.Netcode;

public class Player : NetworkBehaviour{
    public enum Estados {Idle, Walk, Hurt};
    public Estados statePlayer;
    private int speed;
    private Animator animPlayer;

    private NetworkVariable<int> puntos = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private bool painOn = false;
    private float painTime = 1.5f;
    private float painCounter;
    public TMP_Text textoPuntos;
    
    void Start(){
        animPlayer = GetComponent<Animator>();
        statePlayer = Estados.Idle;
        speed = 6;
        painCounter = painTime;

        if (IsOwner) textoPuntos.text = ("Puntos: " + puntos);

        puntos.OnValueChanged += (oldV, newV) =>{
            if (IsOwner && textoPuntos != null){
                textoPuntos.text = "Puntos: " + newV;
            }
        };
    }

    void Update(){
        if (!IsOwner) return;

        if (painOn){
            animPlayer.Play("hurt");
            painCounter -= Time.deltaTime;
            if (painCounter < 0){
                painOn = false;
                painCounter = painTime;
            }
        } else {
            switch (statePlayer){
                case Estados.Idle:
                    Idle();
                    break;
                case Estados.Walk:
                    Walk();
                    break;
            }
        }
    }

    public void Idle(){
        if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("walk_down")) animPlayer.Play("idle_down");
        if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("walk_up")) animPlayer.Play("idle_up");
        if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("walk_side")) animPlayer.Play("idle_side");

        //WALK
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
            statePlayer = Estados.Walk;
        }
    }

    public void Walk(){
        //límites
        if(transform.position.y > 5f) transform.position = new Vector3 (transform.position.x, 5f, 0);
        if(transform.position.x > 8.45f) transform.position = new Vector3 (8.45f, transform.position.y, 0);
        if(transform.position.y < -3.5f) transform.position = new Vector3 (transform.position.x, -3.5f, 0);
        if(transform.position.x < -8.45f) transform.position = new Vector3 (-8.45f, transform.position.y, 0);

        if (Input.GetKey(KeyCode.DownArrow)){
            animPlayer.Play("walk_down");
            transform.eulerAngles = new Vector3(0,0,0); /*izq*/
            transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        } else if (Input.GetKey(KeyCode.UpArrow)){
            animPlayer.Play("walk_up");
            transform.eulerAngles = new Vector3(0,0,0); /*izq*/
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        } else if (Input.GetKey(KeyCode.LeftArrow)){
            animPlayer.Play("walk_side");
            transform.eulerAngles = new Vector3(0,0,0); /*izq*/
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        } else if (Input.GetKey(KeyCode.RightArrow)){
            animPlayer.Play("walk_side");
            transform.eulerAngles = new Vector3(0,180,0); /*der*/
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }

        //idle
        if (!Input.anyKey){
            statePlayer = Estados.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (!IsOwner) return;
        if (other.CompareTag("1p")){
            RecojeConejosServerRpc(other.gameObject.GetComponent<NetworkObject>() != null ? other.gameObject.GetComponent<NetworkObject>().NetworkObjectId : 0, 1, other.gameObject.name);
        }else if (other.CompareTag("5p")){
            RecojeConejosServerRpc(other.gameObject.GetComponent<NetworkObject>() != null ? other.gameObject.GetComponent<NetworkObject>().NetworkObjectId : 0, 5, other.gameObject.name);
        }else if (other.CompareTag("-2p")){
            RecojeConejosServerRpc(other.gameObject.GetComponent<NetworkObject>() != null ? other.gameObject.GetComponent<NetworkObject>().NetworkObjectId : 0, -2, other.gameObject.name);
        }
    }

    [ServerRpc]
    private void RecojeConejosServerRpc(ulong networkObjectId, int deltaPoints, string objectName, ServerRpcParams rpcParams = default){
        puntos.Value += deltaPoints; //actualiza puntos

        if (deltaPoints == -2){ //el nonejo
            DueleClientRpc(OwnerClientId);
        }

        if (networkObjectId != 0){ //destruir recogidos
            if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject netObj)){
                netObj.Despawn(true);
            }
        }
    }

    [ClientRpc]
    private void DueleClientRpc(ulong targetClientId, ClientRpcParams clientRpcParams = default){
        if (!IsOwner) return;
        painOn = true;
    }
}
