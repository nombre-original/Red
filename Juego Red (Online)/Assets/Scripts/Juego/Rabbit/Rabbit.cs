using UnityEngine;

public class Rabbit : MonoBehaviour{
    private int direction;
    private float distance;
    private float wait;
    public bool pickedUp = false;
    private bool inside = false;
    private int speed = 3;
    void Start(){
        
    }

    void Update(){
        Run();
        Grabbed();
        Inside();
    }

    private void Run(){
        
    }
    
    void Grabbed(){
        if (pickedUp){

        }
    }

    private void Inside(){

    }
}
