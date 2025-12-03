using UnityEngine;

public class CambioCartas : MonoBehaviour{
    public SpriteRenderer spriteRenderer;
    public Sprite[] carta;
    public bool girada;

    void Start(){
        girada = false;
    }
    
    void Update(){
        if (!girada){
            spriteRenderer.sprite = carta[0];
        } else {
            Cambio();
        }
    }

    public void Cambio(){
        spriteRenderer.sprite = carta[1];
    }
}
