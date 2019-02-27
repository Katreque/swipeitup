using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float constanteLateral = 1.75f;
    public float constanteProfundidade = -4f;

    private bool Morreu = false;
    private Rigidbody2D rb2d;
    private Animator anim;
    private Collider2D collider;

    Vector2 firstPressPos;

    void Start() {
        rb2d = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
    }

    void FixedUpdate() {
        if (collider && !rb2d.IsTouching(collider)) {
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
        
            //Para esquerda - Teclado
            if (Input.GetKeyDown(KeyCode.LeftArrow) && posicaoX != -constanteLateral) {
                transform.position = new Vector2(posicaoX - 1.75f, constanteProfundidade);
                anim.SetTrigger("Jump");
            }

            //Para direita - Teclado
            if (Input.GetKeyDown(KeyCode.RightArrow) && posicaoX != constanteLateral) {
                transform.position = new Vector2(posicaoX + 1.75f, constanteProfundidade);
                anim.SetTrigger("Jump");
            }
            
            if (Input.touchCount > 0) {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Began) {
                    firstPressPos = t.position;
                }

                if (t.phase == TouchPhase.Ended) {
                    Vector2 secondPressPos = t.position;          
                    
                    print(secondPressPos.x);                    

                    //Para esquerda - Swipe
                    if((secondPressPos.x < firstPressPos.x) && posicaoX != -constanteLateral) {
                        transform.position = new Vector2(posicaoX - 1.75f, constanteProfundidade);
                        anim.SetTrigger("Jump");
                    }

                    //Para direita - Swipe
                    if((secondPressPos.x > firstPressPos.x) && posicaoX != constanteLateral) {
                        transform.position = new Vector2(posicaoX + 1.75f, constanteProfundidade);
                        anim.SetTrigger("Jump");
                    }

                }
            }            
        } else {
            if (Input.touchCount > 0) {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Ended) {
                    GameControl.instance.NovoJogo();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "box") {
            collider = other;
        }
    }
}
