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
    private int score = 0;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        
    }

    void Update() {
        if (gameOver && botaoClicado) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}
