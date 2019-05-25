using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cromossomo {
    
    public int [] cromossomo = new int [10];
    public void GerarPopulacaoInicial() {
        for(int i = 0; i < 10; i++) {
            cromossomo[i] = Random.Range(0, 3);
        }
    }

    public int FuncaoAvaliacao() {
        int temp = cromossomo[0];
        int count = 0;
        Debug.Log(cromossomo[0]);
        for (int i = 1; i < 10; i++) {
            Debug.Log(cromossomo[i]);
            if (cromossomo[i] != temp) {
                count++;
                temp = cromossomo[i];
            }
        }

        return count;
    }
}