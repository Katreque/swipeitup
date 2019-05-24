using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cromossomo {
    public List<int> cromossomo = new List<int>();
    
    public void GerarPopulacaoInicial() {
        for(int i = 0; i < 10; i++) {
            cromossomo.Add(Random.Range(0, 3));
        }
    }
}