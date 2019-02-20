using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepetirBG : MonoBehaviour
{
    
    private BoxCollider2D bgCollider;
    private float bgTamanhoLateral;
    
    void Start() {
        bgCollider = GetComponent<BoxCollider2D> ();
        bgTamanhoLateral = bgCollider.size.x;
    }

    void Update() {
        if (transform.position.y < -bgTamanhoLateral) {
            ReposicionaBG();
        }
    }

    private void ReposicionaBG() {
        Vector2 bgOffset = new Vector2 (0, bgTamanhoLateral * 2f);
        transform.position = (Vector2) transform.position + bgOffset;
    }
}
