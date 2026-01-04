using UnityEngine;
using Unity.Netcode;

public class GoldenRabbit : NetworkBehaviour{
    private Rigidbody2D rb;
    private Animator animRabbit;

    private float speed = 5f;

    private float walkTime;
    private float walkCounter;
    private int walkDirection;

    private NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Server);

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animRabbit = GetComponent<Animator>();
        walkCounter = walkTime;

        animRabbit.Play("penguin_walk");

        ChooseDirection();
        netPosition.Value = transform.position;

        netPosition.OnValueChanged += (oldV, newV) =>{
            if (!IsServer) transform.position = newV;
        };
    }

    void Update(){
        walkCounter -= Time.deltaTime;

        switch(walkDirection){
            case 0:
                rb.linearVelocity = new Vector2 (0, speed);
                if(transform.position.y > 4.75) walkDirection = 2;
                break;

            case 1:
                transform.eulerAngles = new Vector3(0,0,0); /*der*/
                rb.linearVelocity = new Vector2 (speed, 0);
                if(transform.position.x > 8.45) walkDirection = 3;
                break;

            case 2:
                rb.linearVelocity = new Vector2 (0, -speed);
                if (transform.position.y < -3.7) walkDirection = 0;
                break;

            case 3:
                transform.eulerAngles = new Vector3(0,180,0); /*izq*/
                rb.linearVelocity = new Vector2 (-speed, 0);
                if(transform.position.x < -8.45) walkDirection = 1;
                break;
        }
        if (walkCounter < 0) ChooseDirection();

        netPosition.Value = transform.position;
    }

    private void ChooseDirection(){
        walkDirection = Random.Range(0,4);
        walkTime = Random.Range(0.3f,0.7f);
        walkCounter = walkTime;
    }
}
