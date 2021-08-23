
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLeft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isLeft;
    private playerController player;

    public float movement;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && isLeft)
        {
            movement -= Time.deltaTime;

            if (movement < -1f)
            {
                movement = -1f;
            }

            player.movement = movement;
        }
    }

    //é chamado quando clicamos (ou touch) no elemento de UI
    public void OnPointerDown(PointerEventData eventData)
    {
        isLeft = true;
    }

    //é chamado quando tiramos o clique (ou touch) no elemento de UI
    public void OnPointerUp(PointerEventData eventData)
    {
        isLeft = false;
        movement = 0f;
    }
}