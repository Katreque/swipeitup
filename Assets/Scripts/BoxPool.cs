using System;   
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoxPool : MonoBehaviour {
    public GameObject caixasPrefab;
    private float[] colunasPossiveis = new float[] {-1.75f, 0f, 1.75f};
    private Vector2 objectPoolPosition = new Vector2 (0f, 0f);
    private GameObject[] caixas = new GameObject[1000];
    private Persistente arquivoGame;
    private List<Cromossomo> cromossomosUsados = new List<Cromossomo>();

    void Start() {
        try {
            arquivoGame = SaveControl.LoadArquivoJogo();
        } catch(Exception) {
            arquivoGame = new Persistente();
        }

        if (arquivoGame.iniciouGame) {
            cromossomosUsados = arquivoGame.cromossomos;
            Evolucao();

            for (int i = 0; i < 100; i++) {
                for (int j = 0; j < 10; j++) {
                    caixas[(j + (i*10))] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[cromossomosUsados[i].cromossomo[j]], 1.28f * (float)(j + (i*10))), Quaternion.identity);
                }
            }

            //Iniciar sempre no centro
            Destroy(caixas[0]);
            caixas[0] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[1], 0), Quaternion.identity);

        } else {
            GerarPopulacaoInicial();

            for (int i = 0; i < 100; i++) {
                for (int j = 0; j < 10; j++) {
                    caixas[(j + (i*10))] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[cromossomosUsados[i].cromossomo[j]], 1.28f * (float)(j + (i*10))), Quaternion.identity);
                }
            }

            //Iniciar sempre no centro
            Destroy(caixas[0]);
            caixas[0] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[1], 0), Quaternion.identity);
        }

        VerificarPontuacao();
    }

    void FixedUpdate() {
        if (GameControl.instance.atualizaVelocidadeMundo) {
            GameControl.instance.atualizaVelocidadeMundo = false;

            for (int i = 0; i < 1000; i++) {
                caixas[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, GameControl.instance.GetVelocidadeScrollAtual());
            }  
        }
    }

    void GerarPopulacaoInicial() {
        for (int i = 0; i < 100; i++) {
            Cromossomo c = new Cromossomo();
            c.Inicializar();
            c.id = i;
            cromossomosUsados.Add(c);
        }

        arquivoGame.cromossomos = cromossomosUsados;
        SaveControl.AtualizarArquivoJogo(arquivoGame);
    }

    void VerificarPontuacao() {
        int total = 0;

        for (int i = 0; i < cromossomosUsados.Count; i++) {
            total += cromossomosUsados[i].FuncaoAvaliacao();
        }

        GameControl.instance.PontuacaoIA(total/100);
    }

    void Evolucao() {
        Selecao();
    }

    void Selecao() {
        //aptidaoParcial += (arquivoGame.cromossomos[i].FuncaoAvaliacao() / aptidaoTotal);
        Cromossomo primeiro = new Cromossomo();
        Cromossomo segundo = new Cromossomo();
        int maior = 0;

        for (int i = 0; i < cromossomosUsados.Count; i++) {
            if (cromossomosUsados[i].FuncaoAvaliacao() > maior) {

                maior = cromossomosUsados[i].FuncaoAvaliacao();

                if (i == 0) {
                    primeiro = cromossomosUsados[i];
                    segundo = cromossomosUsados[i];
                } else {
                    segundo = primeiro;
                    primeiro = cromossomosUsados[i];
                }
            }
        }

        Crossover(primeiro, segundo);
    }

    void Crossover(Cromossomo primeiro, Cromossomo segundo) {
        Cromossomo temp;
        temp = primeiro;

        primeiro.cromossomo[4] = segundo.cromossomo[4];
        primeiro.cromossomo[5] = segundo.cromossomo[5];
        primeiro.cromossomo[6] = segundo.cromossomo[6];
        primeiro.cromossomo[7] = segundo.cromossomo[7];

        segundo.cromossomo[4] = temp.cromossomo[4];
        segundo.cromossomo[5] = temp.cromossomo[5];
        segundo.cromossomo[6] = temp.cromossomo[6];
        segundo.cromossomo[7] = temp.cromossomo[7];

        Mutacao(primeiro, segundo);
    }

    void Mutacao(Cromossomo primeiro, Cromossomo segundo) {
        int chance = UnityEngine.Random.Range(0, 100);

        if (chance <= 5 && chance >= 0) {
            int cromossomo = UnityEngine.Random.Range(0, 1);
            int numCromossomo = UnityEngine.Random.Range(0, 7);
            int valGene = UnityEngine.Random.Range(0, 2);

            if (cromossomo == 0) {
                primeiro.cromossomo[numCromossomo] = valGene;
            } else {
                segundo.cromossomo[numCromossomo] = valGene;
            }
        }

        ProximaGeracao(primeiro, segundo);
    }

    void ProximaGeracao(Cromossomo primeiro, Cromossomo segundo) {
        Cromossomo penultimo = new Cromossomo();
        Cromossomo ultimo = new Cromossomo();
        int menor = 10;

        for (int i = 0; i < cromossomosUsados.Count; i++) {
            if (cromossomosUsados[i].FuncaoAvaliacao() < menor) {

                menor = cromossomosUsados[i].FuncaoAvaliacao();

                if (i == 0) {
                    penultimo = cromossomosUsados[i];
                    ultimo = cromossomosUsados[i];
                } else {
                    ultimo = penultimo;
                    penultimo = cromossomosUsados[i];
                }
            }
        }

        cromossomosUsados[penultimo.id] = primeiro;
        cromossomosUsados[ultimo.id] = segundo;

        Debug.Log(primeiro.FuncaoAvaliacao());
        Debug.Log(segundo.FuncaoAvaliacao());
        Debug.Log(penultimo.FuncaoAvaliacao());
        Debug.Log(ultimo.FuncaoAvaliacao());

        arquivoGame.cromossomos = cromossomosUsados;
        SaveControl.AtualizarArquivoJogo(arquivoGame);
    }
}
