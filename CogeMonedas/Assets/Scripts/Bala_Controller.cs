using UnityEngine;
using Unity.Netcode;

public class Bala_Controller : NetworkBehaviour{
    void Start(){
        
    }

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.up;
    }

    void Update(){
        
    }
}
