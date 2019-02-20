using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPool : MonoBehaviour
{
    public GameObject caixasPrefab;

    private float[] colunasPossiveis = new float[] {-1.75f, 0f, 1.75f};
    private Vector2 objectPoolPosition = new Vector2 (0f,-8f); 
    private GameObject[] caixas = new GameObject[20];

    void start() {
        for (int i = 0; i < 20; i++) {
            caixas[i] = (GameObject)Instantiate(caixasPrefab, objectPoolPosition, Quaternion.identity);
        }

        caixas[0].transform.position = new Vector2(0f, -4f);

        for (int i = 0; i < 20; i++) {
            int randomXPosition = Random.Range(0, 2);
            float YPosition = (float)i;
            caixas[i].transform.position = new Vector2(colunasPossiveis[randomXPosition], 4f * YPosition);
        }
    }

    void update() {

    }
}
