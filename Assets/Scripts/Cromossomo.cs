using UnityEngine;
using System;

[Serializable]

public class Cromossomo {
    
    public int [] cromossomo = new int [10];
    public int id;

    public void Inicializar() {
        for(int i = 0; i < 10; i++) {
            cromossomo[i] = UnityEngine.Random.Range(0, 3);
        }
    }

    public int FuncaoAvaliacao() {
        int temp = cromossomo[0];
        int count = 0;
        for (int i = 1; i < 10; i++) {
            if (cromossomo[i] != temp) {
                count++;
                temp = cromossomo[i];
            }
        }

        return count;
    }
}