using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorItemController : MonoBehaviour
{
    private bool hasBeenGrabbed;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Hopper")
        {
            Debug.Log("Collected item: " + name);
        }
        else if (collision.tag == "Bin")
        {
            Destroy(gameObject);
        }
        else if (collision.tag == "Conveyor")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
        }
    }
}
