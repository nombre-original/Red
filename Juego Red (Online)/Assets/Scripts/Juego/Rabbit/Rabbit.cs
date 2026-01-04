using UnityEngine;
using Unity.Netcode;

public class Rabbit : NetworkBehaviour{
    private Rigidbody2D rb;
    private Animator animRabbit;

    private float speed = 2f;
    public bool isWalking;

    private float walkTime;
    private float waitTime;
    private float walkCounter;
    private float waitCounter;

    private int walkDirection;

    private NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> netIsWalking = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Server);

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animRabbit = GetComponent<Animator>();

        ChooseDirection();
        netPosition.Value = transform.position;
        netIsWalking.Value = isWalking;

        /*waitCounter = waitTime;
        walkCounter = walkTime;*/
        netPosition.OnValueChanged += (oldV, newV) =>{
            if (!IsServer) transform.position = newV;
        };
        netIsWalking.OnValueChanged += (oldV, newV) =>{
            if (!IsServer) isWalking = newV;
        };
        
    }

    void Update(){
        if (isWalking){
            animRabbit.Play("penguin_walk");
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

            if (walkCounter < 0){
                isWalking = false;
                waitCounter = waitTime;
            }

        } else {
            animRabbit.Play("penguin_idle");
            waitCounter -= Time.deltaTime;
            rb.linearVelocity = Vector2.zero;

            if (waitCounter < 0) ChooseDirection();
        }
    }

    private void ChooseDirection(){
        walkDirection = Random.Range(0,4);
        walkTime = Random.Range(0.5f,1f);
        waitTime = Random.Range(0.3f,1f);
        isWalking = true;
        walkCounter = walkTime;
        netIsWalking.Value = isWalking;
    }
}
