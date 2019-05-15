using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public float amount;
    public Animator animator;

    public TextMeshPro text;
    public SpriteRenderer spriteRenderer;

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

}
