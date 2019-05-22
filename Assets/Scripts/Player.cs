using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float constanteLateral = 1.75f;
    public float constanteProfundidade = -4f;

    private bool Morreu = false;
    private bool EmMovimentoPulo = false;
    private bool botaoReiniciarJogo = false;
    private Rigidbody2D rb2d;
    private Animator anim;
    private Collider2D collider;

    Vector2 firstPressPos;

    void Start() {
        rb2d = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
    }

    void FixedUpdate() {
        if (collider && !rb2d.IsTouching(collider) && !EmMovimentoPulo) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                GameControl.instance.Morreu();
                anim.SetTrigger("Morreu");
                Morreu = true;
            }
        }
    }

    void Update() {
        float posicaoX = transform.localPosition.x;

        if (!Morreu) {

            if (!EmMovimentoPulo) {

                //Para esquerda - Teclado
                if (Input.GetKeyDown(KeyCode.LeftArrow)) {

                    if (posicaoX == -constanteLateral) {
                        StartCoroutine(ParabolaPuloEsquerdaLimite(transform.localPosition.x));
                    } else {
                        StartCoroutine(ParabolaPuloEsquerda(transform.localPosition.x));
                    }

                    anim.SetTrigger("Jump");
                }

                //Para direita - Teclado
                if (Input.GetKeyDown(KeyCode.RightArrow)) {

                    if (posicaoX == constanteLateral) {
                        StartCoroutine(ParabolaPuloDireitaLimite(transform.localPosition.x));                    
                    } else {
                        StartCoroutine(ParabolaPuloDireita(transform.localPosition.x));
                    }

                    anim.SetTrigger("Jump");
                }
                
                if (Input.touchCount > 0) {
                    Touch t = Input.GetTouch(0);

                    if (t.phase == TouchPhase.Began) {
                        firstPressPos = t.position;
                    }

                    if (t.phase == TouchPhase.Ended) {
                        Vector2 secondPressPos = t.position;                    

                        //Para esquerda - Swipe
                        if((secondPressPos.x < firstPressPos.x) && (Mathf.Abs(firstPressPos.x) - Mathf.Abs(secondPressPos.x) > 10f)) {
                            if (posicaoX == -constanteLateral) {
                                StartCoroutine(ParabolaPuloEsquerdaLimite(transform.localPosition.x));
                            } else {
                                StartCoroutine(ParabolaPuloEsquerda(transform.localPosition.x));
                            }

                            anim.SetTrigger("Jump");
                        }

                        //Para direita - Swipe
                        if((secondPressPos.x > firstPressPos.x) && (Mathf.Abs(secondPressPos.x) - Mathf.Abs(firstPressPos.x) > 10f)) {
                            if (posicaoX == constanteLateral) {
                                StartCoroutine(ParabolaPuloDireitaLimite(transform.localPosition.x));
                            } else {
                                StartCoroutine(ParabolaPuloDireita(transform.localPosition.x));
                            }

                            anim.SetTrigger("Jump");
                        }

                    }
                }            
            }
        
        } else {
            if (Input.touchCount > 0) {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began) { 
                    if (EventSystem.current.IsPointerOverGameObject(t.fingerId)) {
                        botaoReiniciarJogo = true;
                    }
                }

                if (t.phase == TouchPhase.Ended) { 
                    if (botaoReiniciarJogo) {
                        botaoReiniciarJogo = false;
                        GameControl.instance.NovoJogo();
                    }
                }
            }
        }
    }

    IEnumerator ParabolaPuloEsquerda(float posicaoInicialx) {
        EmMovimentoPulo = true;
        float con = 0.25f;
        float[] valoresY = new float[] {0f, 0.38f, 0.62f, 0.75f, 0.75f, 0.62f, 0.38f, 0f};

        for (int i = 1; i < 8; i++) {
            float x = posicaoInicialx - (con*i);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, valoresY[i] + constanteProfundidade), 100f);
            yield return new WaitForSeconds(0.0325f);
        }

        EmMovimentoPulo = false;
    }

    IEnumerator ParabolaPuloEsquerdaLimite(float posicaoInicialx) {
        EmMovimentoPulo = true;
        float con = 0.25f;
        float[] valoresY = new float[] {0f, 0.38f, 0.62f, 0.75f, 0.75f, 0.62f, 0.38f, 0f};

        for (int i = 1; i < 4; i++) {
            float x = posicaoInicialx - (con*i);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, valoresY[i] + constanteProfundidade), 100f);
            yield return new WaitForSeconds(0.0325f);
        }

        for (int i = 4; i < 8; i++) {
            float x = 2.75f - (con*(i-3));
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, valoresY[i] + constanteProfundidade), 100f);
            yield return new WaitForSeconds(0.0325f);
        }

        EmMovimentoPulo = false;
    }

    IEnumerator ParabolaPuloDireita(float posicaoInicialx) {
        EmMovimentoPulo = true;
        float con = 0.25f;
        float[] valoresY = new float[] {0f, 0.38f, 0.62f, 0.75f, 0.75f, 0.62f, 0.38f, 0f};

        for (int i = 1; i < 8; i++) {
            float x = posicaoInicialx + (con*i);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, valoresY[i] + constanteProfundidade), 100f);
            yield return new WaitForSeconds(0.0325f);
        }

        EmMovimentoPulo = false;
    }

    IEnumerator ParabolaPuloDireitaLimite(float posicaoInicialx) {
        EmMovimentoPulo = true;
        float con = 0.25f;
        float[] valoresY = new float[] {0f, 0.38f, 0.62f, 0.75f, 0.75f, 0.62f, 0.38f, 0f};

        for (int i = 1; i < 4; i++) {
            float x = posicaoInicialx + (con*i);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, valoresY[i] + constanteProfundidade), 100f);
            yield return new WaitForSeconds(0.0325f);
        }

        for (int i = 4; i < 8; i++) {
            float x = -2.75f + (con*(i-3));
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, valoresY[i] + constanteProfundidade), 100f);
            yield return new WaitForSeconds(0.0325f);
        }

        EmMovimentoPulo = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "box") {
            collider = other;
        }
    }
}
