using UnityEngine;

public class Player : MonoBehaviour{
    public enum Estados {Idle, Walk, Attack, Hurt};
    public Estados statePlayer;
    private int speed;
    private Animator animPlayer;
    
    void Start(){
        animPlayer = GetComponent<Animator>();
        statePlayer = Estados.Idle;
        speed = 6;
    }

    void Update(){
        switch (statePlayer){
            case Estados.Idle:
                Idle();
                break;
            case Estados.Walk:
                Walk();
                break;
            case Estados.Attack:
                Attack();
                break;
            case Estados.Hurt:
                Hurt();
                break;
        }
    }

    public void Idle(){
        animPlayer.Play("idle_down");

        //WALK
        /*if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
            statePlayer = Estados.Walk;
        }
        //ATTACK
        if (Input.GetKey(KeyCode.X)){
            statePlayer.Estados.Attack;
        }*/
    }

    public void Walk(){
        animPlayer.Play("walk_down");
    }

    public void Attack(){
        animPlayer.Play("hammer_attack_down");
        animPlayer.Play("hammer_attack_down_overlay");
    }

    public void Hurt(){

    }
}
