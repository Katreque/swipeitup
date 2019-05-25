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
    private GameObject[] caixas = new GameObject[20];
    private Persistente arquivoGame;
    private List<Cromossomo> cromossomosUsados = new List<Cromossomo>();
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
            Cromossomo c1 = new Cromossomo();
            c1.GerarPopulacaoInicial();
            cromossomosUsados.Add(c1);

            caixas[0] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[1], 0), Quaternion.identity);

            for (int i = 1; i < 10; i++) {
                caixas[i] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[c1.cromossomo[i]], 1.27f * (float)i), Quaternion.identity);
            }

            Cromossomo c2 = new Cromossomo();
            c2.GerarPopulacaoInicial();
            cromossomosUsados.Add(c2);

            for (int i = 0; i < 10; i++) {
                caixas[i+10] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[c2.cromossomo[i]], 1.27f * (float)i+10), Quaternion.identity);
            }
        }
    }

    void FixedUpdate() {
        if (GameControl.instance.atualizaVelocidadeMundo) {
            GameControl.instance.atualizaVelocidadeMundo = false;

            for (int i = 0; i < 20; i++) {
                //caixas[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, GameControl.instance.GetVelocidadeScrollAtual());
            }  
        }

        if (arquivoGame.iniciouGame) {

        } else {
            if (caixas[9].transform.position.y < -2f) {
                Cromossomo c = new Cromossomo();
                c.GerarPopulacaoInicial();
                cromossomosUsados.Add(c);

                for (int i = 0; i < 10; i++) {
                    Destroy(caixas[i]);
                    caixas[i] = null;
                    caixas[i] = (GameObject)Instantiate(caixasPrefab, objectPoolPosition, Quaternion.identity);
                    caixas[i].transform.position = new Vector2(colunasPossiveis[c.cromossomo[i]], 10.56f + (1.27f * (float)i));
                }
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
