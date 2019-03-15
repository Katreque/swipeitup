using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPool : MonoBehaviour
{
    public GameObject caixasPrefab;

    private float[] colunasPossiveis = new float[] {-1.75f, 0f, 1.75f};
    private Vector2 objectPoolPosition = new Vector2 (0f, 0f); 
    private GameObject[] caixas = new GameObject[10];

    void Start() {
        for (int i = 0; i < 10; i++) {
            caixas[i] = (GameObject)Instantiate(caixasPrefab, objectPoolPosition, Quaternion.identity);
        }

        caixas[0].transform.position = new Vector2(0f, 0f);
        caixas[1].transform.position = new Vector2(0f, 1.27f);

        for (int i = 2; i < 10; i++) {
            int randomXPosition = Random.Range(0, 3);
            caixas[i].transform.position = new Vector2(colunasPossiveis[randomXPosition], 1.27f * (float)i);
        }
    }

    void FixedUpdate() {
        if (GameControl.instance.atualizaVelocidadeMundo) {
            GameControl.instance.atualizaVelocidadeMundo = false;

            for (int i = 0; i < 10; i++) {
                caixas[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, GameControl.instance.GetVelocidadeScrollAtual());
            }  
        }

        for (int i = 0; i < 10; i++) {
            if (caixas[i].transform.position.y < -2f) {
                
                Destroy(caixas[i]);
                caixas[i] = null;
                caixas[i] = (GameObject)Instantiate(caixasPrefab, objectPoolPosition, Quaternion.identity);

                int randomXPosition = Random.Range(0, 3);
                caixas[i].transform.position = new Vector2(colunasPossiveis[randomXPosition], 10.56f);
            }
        }
    }
}
