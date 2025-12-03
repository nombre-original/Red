using UnityEngine;

public class Cartas : MonoBehaviour{
    public SpriteRenderer spriteRenderer;
    public Sprite[] cartaTipo;
    public GameObject carta;
    private int[] baraja = new int[7];

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
            for (int i=0; i<=7; i++){ //Primeras 4 cartas
                numAleatorio = Random.Range(1,cartaTipo.Length-1);//num aleatorio
                //Instantiate(cartas[numAleatorio], new Vector2(x, y), Quaternion.identity/*ignora apartado rotación*/);
                Instantiate(carta, new Vector2(x, y), Quaternion.identity);
                Debug.Log("detrás");
                x += sepX;
                if (x>8f){ //Últimas 4 cartas
                    y -= sepY;
                    x = x-(sepX*4);
                }
            }
            cartasSobreLaMesa = true;
        }
        
        /*
        //ELEGIR CARTAS DE LA BARAJA
        for (int i=0; i<=cartas.Length; i++){
            numAleatorio = Random.Range(0,numCartas);//num aleatorio
            baraja[i] = numAleatorio;
            Debug.Log(baraja[i]);
            //REPETICIONES
            /*for (int a=0; a<=baraja[i-1]; a++){//si se repite 1 vez
                if(numAleatorio == baraja[a]){
                    for (int b=0; b<=baraja[a+1]; b++){//si se repite dos veces
                        if(numAleatorio == baraja[b]){
                            numAleatorio = Random.Range(0,cartas.Length);
                        }
                    }
                //SI NO SE REPITE
                } else {
                    baraja[i] = numAleatorio;
                }
            }
        }
        for (int i=0; i<=4; i++){
            Debug.Log(baraja[i]);
        }
        }*/
    }
}
