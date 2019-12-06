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

            this.AjustaArrayScore();
            this.Evolucao();

            for (int i = 0; i < 100; i++) {
                for (int j = 0; j < 10; j++) {
                    caixas[(j + (i*10))] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[cromossomosUsados[i].cromossomo[j]], 1.28f * (float)(j + (i*10))), Quaternion.identity);
                }
            }

            //Iniciar sempre no centro
            Destroy(caixas[0]);
            caixas[0] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[1], 0), Quaternion.identity);

        } else {
            this.GerarPopulacaoInicial();

            for (int i = 0; i < 100; i++) {
                for (int j = 0; j < 10; j++) {
                    caixas[(j + (i*10))] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[cromossomosUsados[i].cromossomo[j]], 1.28f * (float)(j + (i*10))), Quaternion.identity);
                }
            }

            //Iniciar sempre no centro
            Destroy(caixas[0]);
            caixas[0] = (GameObject)Instantiate(caixasPrefab, new Vector2(colunasPossiveis[1], 0), Quaternion.identity);
        }

        this.VerificarPontuacao();
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
        arquivoGame.evolutionState = true;

        for (int i = 0; i < arquivoGame.lastScores.Length; i++) {
            arquivoGame.lastScores[i] = -1;
        }

        SaveControl.AtualizarArquivoJogo(arquivoGame);
    }

    void VerificarPontuacao() {
        int total = 0;

        for (int i = 0; i < cromossomosUsados.Count; i++) {
            total += cromossomosUsados[i].FuncaoAvaliacao();
        }

        GameControl.instance.PontuacaoIA(total/100);
    }

    void AjustaArrayScore() {
        bool todasPartidasConcluidas = true;

        for (int i = 0; i < arquivoGame.lastScores.Length; i++) {
            if (arquivoGame.lastScores[i] == -1) {
                arquivoGame.lastScores[i] = SaveControl.RetornarLastScore();
                todasPartidasConcluidas = false;
                break;
            }
        }

        if (todasPartidasConcluidas) {
            this.SetaDirecaoEvolucao();
        }
    }

    void SetaDirecaoEvolucao() {
        int mediaUltimosScores = 0;
        int maiorElemento = 0;

        for (int i = 0; i < arquivoGame.lastScores.Length; i++) {
            mediaUltimosScores += arquivoGame.lastScores[i];

            if (maiorElemento < arquivoGame.lastScores[i]) {
                maiorElemento = arquivoGame.lastScores[i];
            }
        }

        mediaUltimosScores = (mediaUltimosScores - maiorElemento) / (arquivoGame.lastScores.Length - 1);
        Debug.Log(SaveControl.RetornarEvolutionState());

        if (mediaUltimosScores < (maiorElemento / 2)) {
            arquivoGame.evolutionState = false;
        } else {
            arquivoGame.evolutionState = true;
        }

        for (int i = 0; i < arquivoGame.lastScores.Length; i++) {
            arquivoGame.lastScores[i] = -1;
        }

        SaveControl.AtualizarArquivoJogo(arquivoGame);
    }

    void Evolucao() {
        Selecao();
    }

    void Selecao() {
        Cromossomo primeiro = new Cromossomo();
        Cromossomo segundo = new Cromossomo();

        if (SaveControl.RetornarEvolutionState()) {
            int maior = 0;

            for (int i = 0; i < cromossomosUsados.Count; i++) {
                if (cromossomosUsados[i].FuncaoAvaliacao() >= maior) {

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
        } else {
            int menor = 10;

            for (int i = 0; i < cromossomosUsados.Count; i++) {
                if (cromossomosUsados[i].FuncaoAvaliacao() <= menor) {

                    menor = cromossomosUsados[i].FuncaoAvaliacao();

                    if (i == 0) {
                        primeiro = cromossomosUsados[i];
                        segundo = cromossomosUsados[i];
                    } else {
                        segundo = primeiro;
                        primeiro = cromossomosUsados[i];
                    }
                }
            }
        }
        

        Crossover(primeiro, segundo);
    }

    void Crossover(Cromossomo primeiro, Cromossomo segundo) {
        Cromossomo temp;
        temp = primeiro;

        int pontoCorte = UnityEngine.Random.Range(1, 9);

        for (int i = 0; i < pontoCorte; i++) {
            primeiro.cromossomo[i] = segundo.cromossomo[i];
            segundo.cromossomo[i] = temp.cromossomo[i];
        }

        Mutacao(primeiro, segundo);
    }

    void Mutacao(Cromossomo primeiro, Cromossomo segundo) {
        int chance = UnityEngine.Random.Range(0, 100);

        if (chance <= 10 && chance >= 0) {
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

        if (SaveControl.RetornarEvolutionState()) {
            int menor = 10;

            for (int i = 0; i < cromossomosUsados.Count; i++) {
                if (cromossomosUsados[i].FuncaoAvaliacao() <= menor) {

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
        } else {
            int maior = 0;

            for (int i = 0; i < cromossomosUsados.Count; i++) {
                if (cromossomosUsados[i].FuncaoAvaliacao() >= maior) {

                    maior = cromossomosUsados[i].FuncaoAvaliacao();

                    if (i == 0) {
                        penultimo = cromossomosUsados[i];
                        ultimo = cromossomosUsados[i];
                    } else {
                        ultimo = penultimo;
                        penultimo = cromossomosUsados[i];
                    }
                }
            }
        }

        cromossomosUsados[penultimo.id].cromossomo = primeiro.cromossomo;
        cromossomosUsados[ultimo.id].cromossomo = segundo.cromossomo;

        arquivoGame.cromossomos = cromossomosUsados;
        SaveControl.AtualizarArquivoJogo(arquivoGame);
    }
}
