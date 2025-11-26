using UnityEngine;

public class Cartas : MonoBehaviour{
    public GameObject[] cartas;
    private int[] baraja = new int[7];

    private int numAleatorio;
    private int numCartas;

    void Start(){
        numCartas = cartas.Length;
    }
//NO, CREAR UNA BARSE PREESTABLECIDA Y DESORDENARLA
    void Update(){
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
        }*/
        }
    }
}
