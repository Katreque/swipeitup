using System.IO;
using UnityEngine;

public static class SaveControl {
    private static Persistente dadosJogador = new Persistente();
    private static string path = Path.Combine(Application.persistentDataPath, "genes.json");

    public static void AtualizarArquivoJogo(Persistente data) {
        dadosJogador.iniciouGame = true;
        dadosJogador.evolutionState = data.evolutionState;
        dadosJogador.cromossomos = data.cromossomos;
        dadosJogador.highScore = data.highScore;
        dadosJogador.lastScore = data.lastScore;
        dadosJogador.lastScores = data.lastScores;
    }

    public static void ResetaIAJogo() {
        dadosJogador.iniciouGame = false;
        dadosJogador.evolutionState = true;
        dadosJogador.cromossomos = null;
        dadosJogador.highScore = 0;
        dadosJogador.lastScore = 0;

        SalvarArquivoJogo();
    }

    public static void SetarHighScoreTotal(int score) {
        dadosJogador.highScoreTotal = score;
    }

    public static int RetornarHighScoreTotal() {
        return dadosJogador.highScoreTotal;
    }

    public static void SetarHighScore(int score) {
        dadosJogador.highScore = score;
    }

    public static int RetornarHighScore() {
        return dadosJogador.highScore;
    }

    public static void SetarLastScore(int score) {
        dadosJogador.lastScore = score;
    }

    public static int RetornarLastScore() {
        return dadosJogador.lastScore;
    }

    public static void SetarEvolutionState(bool state) {
        dadosJogador.evolutionState = state;
    }

    public static bool RetornarEvolutionState() {
        return dadosJogador.evolutionState;
    }

    public static void SalvarArquivoJogo() {
        string jsonString = JsonUtility.ToJson(dadosJogador);

        using (StreamWriter streamWriter = File.CreateText(path)) {
            streamWriter.Write(jsonString);
        }
    }

    public static Persistente LoadArquivoJogo() {
        using (StreamReader streamReader = File.OpenText(path)) {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<Persistente>(jsonString);
        }
    }
}