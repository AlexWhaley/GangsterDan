using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorItemController : MonoBehaviour
{
    [SerializeField] private ItemData Data;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Hopper"))
        {
            ScavengeManager.Instance.CollectItem(Data);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Bin"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Conveyor"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
        }
    }
}
