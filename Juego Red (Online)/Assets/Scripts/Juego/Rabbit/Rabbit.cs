using UnityEngine;

public class Rabbit : MonoBehaviour{
    //locations
    private float x = 0;
    private float y = 0;

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
        direction = Random.Range(0,4);
        distance = Random.Range(0.5,3);

        if (direction == 0) transform.Translate(Vector3.left * Time.deltaTime, Space.World);
        if (direction == 1) transform.Translate(Vector3.right * Time.deltaTime, Space.World);
        if (direction == 2) transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        if (direction == 3) transform.Translate(Vector3.down * Time.deltaTime, Space.World);
    }
    
    void Grabbed(){
        if (pickedUp){

        }
    }

    private void Inside(){

    }
}
