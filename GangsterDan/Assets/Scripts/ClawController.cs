using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dropSpeed = 10f;

    private Animator animator;

    private bool isDropping;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        MoveClaw();
        Grab();
    }

    private void MoveClaw()
    {
        float translation = 0;

        if (Input.GetKey(KeyCode.A))
        {
            translation -= moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            translation += moveSpeed;
        }

        isDropping = Input.GetKey(KeyCode.S);

        transform.Translate(translation * Time.deltaTime, isDropping ? -dropSpeed * Time.deltaTime : 0, 0);
    }

    private void Grab()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Close");
        }
    }
}

