using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractPressurePlate : MonoBehaviour
{

    [SerializeField] private GameObject door;
    private float timer;
    // Start is called before the first frame update

    private void Update()
    {
            if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                door.GetComponent<Animator>().Play("CloseDoor"); //Should play the animation
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            door.GetComponent<Animator>().Play("DoorOpen"); //play the animation
            
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            timer = 0.09f;
        }
    }

}
