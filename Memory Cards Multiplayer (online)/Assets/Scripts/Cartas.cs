using UnityEngine;

public class Cartas : MonoBehaviour{
    public SpriteRenderer spriteRenderer;
    public Sprite[] cartaTipo;
    public GameObject carta;
    private int[] baraja = new int[7];
    private int eleccion;

    private int numAleatorio;

    //Posición de las cartas
    private float x;
    private float y;
    private float sepX;
    private float sepY;

    private bool cartasSobreLaMesa = false;

    void Start(){
        x = -7.5f;
        y = 2f;
        sepX = 5f;
        sepY = 4f;
    }
//NO, CREAR UNA BARSE PREESTABLECIDA Y DESORDENARLA
    void Update(){
        if (!cartasSobreLaMesa){
            //tipos de cartas
            for (int a = 0; a<=3; a++){
                eleccion = Random.Range(1,cartaTipo.Length-1);
                baraja[a] = eleccion;
            }
            //cartas
            for (int i=0; i<=7; i++){ //Primeras 4 cartas
                Instantiate(carta, new Vector2(x, y), Quaternion.identity);
                //baraja[i] = carta.GetComponent<VoltearCarta>().tipo;
                Debug.Log("detrás");
                x += sepX;
                if (x>8f){ //Últimas 4 cartas
                    y -= sepY;
                    x = x-(sepX*4);
                }
            }
            Debug.Log(baraja[3]);
            cartasSobreLaMesa = true;
        }
    }
}