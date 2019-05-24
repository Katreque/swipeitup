using System;   
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoxPool : MonoBehaviour
{
    public GameObject caixasPrefab;
    private float[] colunasPossiveis = new float[] {-1.75f, 0f, 1.75f};
    private Vector2 objectPoolPosition = new Vector2 (0f, 0f);
    private GameObject[] caixas = new GameObject[10];
    private Persistente arquivoGame;
    private List<Cromossomo> cromossomosCriados = new List<Cromossomo>();
    private string path;

    void Start() {
        path = Path.Combine(Application.persistentDataPath, "genes.txt");

        try {
            arquivoGame = LoadArquivoJogo();
        } catch(Exception) {
            arquivoGame = new Persistente();
            SalvarArquivoJogo(arquivoGame);
        }

        if (arquivoGame.iniciouGame) {

        } else {
            Cromossomo c = new Cromossomo();
            c.GerarPopulacaoInicial();
            cromossomosCriados.Add(c);

            caixas[0] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[1], 0), Quaternion.identity);

            for (int i = 1; i < 10; i++) {
                caixas[i] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[c.cromossomo[i]], 1.27f * (float)i), Quaternion.identity);
            }
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

                int randomXPosition = UnityEngine.Random.Range(0, 3);
                caixas[i].transform.position = new Vector2(colunasPossiveis[randomXPosition], 10.56f);
            }
        }
    }

    void SalvarArquivoJogo(Persistente data) {
        string jsonString = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(path)) {
            streamWriter.Write(jsonString);
        }
    }

    Persistente LoadArquivoJogo() {
        using (StreamReader streamReader = File.OpenText(path)) {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<Persistente>(jsonString);
        }
    }

    void FuncaoAvaliacao() {

    }

    void Selecao() {

    }

    void Crossover() {

    }

    void Mutacao() {

    }
}
