using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dropSpeed = 10f;

    private Animator animator;

    private bool isDropping;
    private bool isGrabbing;
    private bool justGrabbed;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        MoveClaw();
        ToggleGrab();
    }

    private void MoveClaw()
    {
        Vector2 translation = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            translation.x -= moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            translation.x += moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            translation.y -= dropSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            translation.y += dropSpeed;
        }

        isDropping = Input.GetKey(KeyCode.S);

        transform.Translate(translation.x * Time.deltaTime, translation.y * Time.deltaTime, 0);
    }

    private void ToggleGrab()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGrabbing = !isGrabbing;
            justGrabbed = isGrabbing;
            animator.SetTrigger(isGrabbing ? "Close" : "Open");
        }
        else
        {
            justGrabbed = false;
        }
    }
}

