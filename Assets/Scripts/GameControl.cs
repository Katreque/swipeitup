using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public GameObject gameOverText;
    public Text scoreText;
    public bool gameOver = false;
    public bool botaoClicado = false;
    public bool atualizaVelocidadeMundo = false;
    private int score = 0;
    private int scoreTemp = 0;
    private float velocidadeScrollAtual = -1.5f;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        scoreTemp = score;
    }

    void FixedUpdate() {
        if (gameOver && botaoClicado) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (scoreTemp == score - 1) {
            AtualizaVelocidadeScroll();
            scoreTemp += 1;
        }
    }

    public void Pontuou() {
        if (gameOver) {
            return;
        }

        score++;
        scoreText.text = score.ToString();
    }

    public void Morreu() {
        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void NovoJogo () {
        botaoClicado = true;
    }

    public void AtualizaVelocidadeScroll() {
        atualizaVelocidadeMundo = true;
        velocidadeScrollAtual += -0.05f;
    }

    public float GetVelocidadeScrollAtual() {
        return velocidadeScrollAtual;
    }
}
