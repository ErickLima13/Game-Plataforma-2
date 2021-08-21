using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private GameController _GameController;

    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private SpriteRenderer playerSr;


    [Header("Atributtes")] //Cabeçalho
    public float  speed;
    public float jumpForce;
    public int maxHp;
    public bool isLookLeft;

    [Header("Ground Check")]
    public Transform groundCheck;
    private bool isGrounded;

    private bool isAttack;

    public Transform mao;
    public GameObject hitBoxPrefab;

    public Color hitColor;
    public Color noHitColor;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>(); // recebe o componente Rigidbody2D (aplica fisica ao objeto) do player
        playerAnimator = GetComponent<Animator>(); // recebe o componente Animator (aplica animações ao objeto) do player
        playerSr = GetComponent<SpriteRenderer>(); // recebe o componente SpriteRenderer  do player

        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        _GameController.playerTransform = this.transform;


    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool("isGrounded", isGrounded); // da o valor de true a booleana do animator e a variavel  
        if (_GameController.currentState != gameState.GAMEPLAY)
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            playerAnimator.SetInteger("h", 0);
            return;
        }

        float h = Input.GetAxisRaw("Horizontal"); //recebe os input horizontal (já pré configurados na unity)

        if (isAttack && isGrounded) // impede de dar pulos seguidos e atacar andando
        {
            h = 0; // para o player 
        }

        if (h > 0 && isLookLeft) //verifico se  h é maior que 0 e se isLookLeft é true/ move para Direita
        {
            Flip(); // chamando o metodo 
        }
        else if(h < 0 && !isLookLeft) //verifico se  h é menor que 0 e se isLookLeft é false /move para Esquerda
        {
            Flip(); // chamando o metodo 
        }


        float speedY = playerRb.velocity.y; //valor da velocidade y do player

        playerRb.velocity = new Vector2(h * speed, speedY); // move o player

        if (Input.GetButtonDown("Jump") && isGrounded) // se for pressionado o botão Jump (Space) e isGrounded for true
        {
            _GameController.playSFX(_GameController.sfxJump, 0.1f); // chama o som do pulo 
            playerRb.AddForce(new Vector2(0, jumpForce)); // faz o player pular
        }

        if (Input.GetButtonDown("Fire1") && !isAttack) // se for pressionado o botão Fire1 (esquerdo mouse) e isAttack for false
        {
            isAttack = true;
            _GameController.playSFX(_GameController.sfxAttack, 0.3f); // chama o som do ataque 
            playerAnimator.SetTrigger("attack"); // ativa  o trigger (gatilho) do animator 
        }

        playerAnimator.SetInteger("h", (int) h ); // faz o parametro do animator h receber o valor da variavel h convertida com int 
       
        playerAnimator.SetFloat("speedY", speedY); // da o valor da variavel speedY pro parametro speedY do animator
        playerAnimator.SetBool("isAttack", isAttack); // da o valor de true a booleana do animator e a variavel  

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f); // desenha um circle collider no objeto groundCheck
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "coletavel")
        {
            _GameController.getCoin();
            _GameController.playSFX(_GameController.sfxCoin, 0.2f);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "damage")
        {
            _GameController.getHit();

            if (_GameController.vida > 0)
            {
                StartCoroutine("damageController");
                print("Dano");
            }
        }
        else if(col.gameObject.tag == "Flag")
        {
            _GameController.getEnd();
        }
        else if(col.gameObject.tag == "Abismo")
        {
            _GameController.Fall();
        }


        if(col.gameObject.tag == "Arrow")
        {
            col.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            col.gameObject.GetComponent<Animator>().enabled = true;
            print("caminho indicado");
            
        }

    }

    //funções criadas pelo programador
    void Flip() // Metodo para virar o player
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1; // inverte o sinal do scale x
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z); //passa o valor da variavel x, e mantem os que ja tinha de y e z do player
    }

    void OnEndAttack() //finaliza a animação e impede de atacar varias vezes seguidas  
    {
        isAttack = false; 
    }

    void hitBoxAttack() 
    {
        GameObject hitBoxtemp = Instantiate(hitBoxPrefab, mao.position, transform.localRotation); // instacia um colissor a frente 
        Destroy(hitBoxtemp, 1f); // destroi o objeto instanciado 
    }

    void footStep()
    {
        _GameController.playSFX(_GameController.sfxStep[Random.Range(0,_GameController.sfxStep.Length)], 0.5f); // chama o som dos passos 
    }

    IEnumerator damageController()
    {
        _GameController.playSFX(_GameController.sfxDamage, 0.2f);
        

        this.gameObject.layer = LayerMask.NameToLayer("Invencivel");
        playerSr.color = hitColor;
        yield return new WaitForSeconds(0.3f);
        playerSr.color = noHitColor;

        for(int i =0; i < 5; i++)
        {
            playerSr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        this.gameObject.layer = LayerMask.NameToLayer("Player");
        playerSr.color = Color.white;

    }
}
