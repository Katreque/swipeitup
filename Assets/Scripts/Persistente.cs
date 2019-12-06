using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]

public class Persistente {
    public bool iniciouGame;
    public List<Cromossomo> cromossomos = new List<Cromossomo>();
    public int highScore;
    public int highScoreTotal;
    public int lastScore;
    public bool evolutionState;
    public int[] lastScores = new int[4];
}