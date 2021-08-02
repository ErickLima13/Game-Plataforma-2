using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batIA : MonoBehaviour
{
    private GameController _GameController;
    private Animator batAnimator;

    private bool isFollow;

    public float speed;
    public bool isLookLeft;
    public GameObject hitbox;
    public GameObject Coin;
    public Transform drop;



    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;

        batAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_GameController.currentState != gameState.GAMEPLAY) { return; }

        if (isFollow == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _GameController.playerTransform.position, speed * Time.deltaTime);
        }

        if(transform.position.x < _GameController.playerTransform.position.x && isLookLeft == true)
        {
            Flip();
        }
        else if(transform.position.x > _GameController.playerTransform.position.x && isLookLeft == false)
        {
            Flip();
        }
    }

    private void OnBecameVisible()
    {
        isFollow = true;
    }

    void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z); 
        
    }

   
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "hitBox")
        {
           Destroy(hitbox);
           print("bateu");
           _GameController.playSFX(_GameController.sfxEnemyDead, 0.3f);
           batAnimator.SetTrigger("dead");
           
        }
     
    }

    void OnDead()
    {
        Destroy(this.gameObject);
        Coin = Instantiate(Coin, drop.position, transform.localRotation); // instacia uma moeda na morte 
    }

}
