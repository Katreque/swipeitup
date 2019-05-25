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
    private LayerMask layerCaixas;
    private Rigidbody2D rb2d;
    private Animator anim;
    private Collider2D colliderPlayer;
    private Collider2D collider;

    Vector2 firstPressPos;

    void Start() {
        rb2d = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
        colliderPlayer = GetComponent<BoxCollider2D> ();
        layerCaixas = LayerMask.GetMask("Caixas");
    }

    void Update() {
        if (collider && !colliderPlayer.IsTouchingLayers(layerCaixas) && !EmMovimentoPulo) {
            GameControl.instance.Morreu();
            anim.SetTrigger("Morreu");
            Morreu = true;
        }

        float posicaoX = transform.localPosition.x;

        if (!Morreu) {

            if (!EmMovimentoPulo) {

                //Para esquerda - Teclado
                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    anim.SetTrigger("Jump");

                    if (posicaoX == -constanteLateral) {
                        StartCoroutine(ParabolaPuloEsquerdaLimite(transform.localPosition.x));
                    } else {
                        StartCoroutine(ParabolaPuloEsquerda(transform.localPosition.x));
                    }
                }

                //Para direita - Teclado
                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    anim.SetTrigger("Jump");

                    if (posicaoX == constanteLateral) {
                        StartCoroutine(ParabolaPuloDireitaLimite(transform.localPosition.x));                    
                    } else {
                        StartCoroutine(ParabolaPuloDireita(transform.localPosition.x));
                    }
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
                            anim.SetTrigger("Jump");

                            if (posicaoX == -constanteLateral) {
                                StartCoroutine(ParabolaPuloEsquerdaLimite(transform.localPosition.x));
                            } else {
                                StartCoroutine(ParabolaPuloEsquerda(transform.localPosition.x));
                            }
                        }

                        //Para direita - Swipe
                        if((secondPressPos.x > firstPressPos.x) && (Mathf.Abs(secondPressPos.x) - Mathf.Abs(firstPressPos.x) > 10f)) {
                            anim.SetTrigger("Jump");

                            if (posicaoX == constanteLateral) {
                                StartCoroutine(ParabolaPuloDireitaLimite(transform.localPosition.x));
                            } else {
                                StartCoroutine(ParabolaPuloDireita(transform.localPosition.x));
                            }
                        }

                    }
                }            
            }
        
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) { 
                GameControl.instance.NovoJogo();
            }
            
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

        anim.SetTrigger("Idle");
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

        anim.SetTrigger("Idle");
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

        anim.SetTrigger("Idle");
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

        anim.SetTrigger("Idle");
        EmMovimentoPulo = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        collider = other;
    }
}
