using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeIA : MonoBehaviour
{
    private GameController _GameController;
    private Rigidbody2D slimeRb;
    private Animator slimeAnimator;

    [Header("Atributtes")]
    public float speed;
    public float timeToWalk;
    public GameObject hitBox;
    public GameObject Coin;
    public Transform drop;

    public bool isLookLeft;
    private int h;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;

        slimeRb = GetComponent<Rigidbody2D>();
        slimeAnimator = GetComponent<Animator>();

        StartCoroutine("SlimeWalk");

    }

    // Update is called once per frame
    void Update()
    {
        if (_GameController.currentState != gameState.GAMEPLAY) { return; }

        if (h > 0 && isLookLeft) //verifico se  h é maior que 0 e se isLookLeft é true/ move para Direita
        {
            Flip(); // chamando o metodo 
        }
        else if (h < 0 && !isLookLeft) //verifico se  h é menor que 0 e se isLookLeft é false /move para Esquerda
        {
            Flip(); // chamando o metodo 
        }

        slimeRb.velocity = new Vector2(h * speed, slimeRb.velocity.y);

        if(h != 0)
        {
            slimeAnimator.SetBool("isWalk", true);
        }
        else
        {
            slimeAnimator.SetBool("isWalk", false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "hitBox")
        {
            Destroy(hitBox);
            _GameController.playSFX(_GameController.sfxEnemyDead, 0.3f);
            slimeAnimator.SetTrigger("dead");
            print("bateu");
        }
    }

    void Flip() // Metodo para virar o player
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; // inverte o sinal do scale x
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z); //passa o valor da variavel x, e mantem os que ja tinha de y e z do player
    }

    IEnumerator SlimeWalk()
    {
        int rand = Random.Range(0, 100);
        if (rand < 33)
        {
            h = -1;
        }
        else if(rand < 66)
        {
            h = 0;
        }
        else if(rand < 100)
        {
            h = 1;
        }

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("SlimeWalk");
    }

    void OnDead()
    {
        Coin = Instantiate(Coin, drop.position, transform.localRotation); // instacia uma moeda na morte do slime
        h = 0;
        StopCoroutine("SlimeWalk");
        Destroy(this.gameObject);
        
    }


}
