using UnityEngine;

public class VoltearCarta : MonoBehaviour{
    public SpriteRenderer spriteRenderer;
    public Sprite[] carta;
    public bool girada;
    public int tipo;

    void Start(){
        girada = false;
        tipo = Random.Range(1,carta.Length-1);
    }
    
    void Update(){
        if (!girada){
            spriteRenderer.sprite = carta[0];
        } else {
            spriteRenderer.sprite = carta[tipo];
        }
    }
}
