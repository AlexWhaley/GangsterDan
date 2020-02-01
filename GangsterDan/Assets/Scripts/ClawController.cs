using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float DropSpeed = 10f;

    private Animator animator;

    private ClawState state;
    private float interactDelay = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        interactDelay -= Time.deltaTime;

        MoveClaw();
        Interact();
    }

    private void MoveClaw()
    {
        float translation = 0;

        if (Input.GetKey(KeyCode.A))
        {
            translation -= MoveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            translation += MoveSpeed;
        }

        transform.Translate(translation * Time.deltaTime, 0, 0);
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Space) && interactDelay <= 0)
        {
            switch (state)
            {
                case ClawState.Open:
                    animator.SetTrigger("Drop");
                    state = ClawState.Drop;
                    interactDelay = 1.5f;
                    break;
                case ClawState.Drop:
                    animator.SetTrigger("Return");
                    state = ClawState.Return;
                    interactDelay = 1.0f;
                    break;
                case ClawState.Return:
                    animator.SetTrigger("Open");
                    state = ClawState.Open;
                    interactDelay = 0.75f;
                    break;
                default:
                    break;
            }
        }
    }
}

public enum ClawState
{
    Open,
    Drop,
    Return
}

