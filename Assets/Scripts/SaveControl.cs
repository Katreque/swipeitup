using System;   
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveControl {
    private static Persistente dadosJogador = new Persistente();
    private static string path = Path.Combine(Application.persistentDataPath, "genes.json");

    public static void AtualizarArquivoJogo(Persistente data) {
        dadosJogador = data;
    }

    public static void SalvarArquivoJogo() {
        string jsonString = JsonUtility.ToJson(dadosJogador);

        using (StreamWriter streamWriter = File.CreateText(path)) {
            streamWriter.Write(jsonString);
        }
    }

    public static Persistente LoadArquivoJogo() {
        Debug.Log(Application.persistentDataPath);
        using (StreamReader streamReader = File.OpenText(path)) {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<Persistente>(jsonString);
        }
    }
}