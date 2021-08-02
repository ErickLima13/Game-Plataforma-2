using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum musicaFase
{
    FLORESTA, CAVERNA, GAMEOVER, THEEND   
}

public enum gameState
{
    TITULO, GAMEPLAY, END, GAMEOVER
}

public class GameController : MonoBehaviour
{
    public gameState currentState;
    public GameObject painelTitulo, painelGameOver, painelEnd;

    public Transform playerTransform;
    private Camera cam;

    public float speedCam;
    public Transform limiteCamEsq, limiteCamDir, limiteCamSup, limiteCamBaixo;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip sfxJump;
    public AudioClip sfxAttack;
    public AudioClip[] sfxStep;
    public AudioClip sfxCoin;
    public AudioClip sfxEnemyDead;
    public AudioClip sfxDamage;
    public AudioClip musicFloresta, musicCaverna, musicGameOver, musicEnd;


    public GameObject[] fase;

    public musicaFase musicaAtual;

    public int moedasColetadas;
    public Text moedasTxt;
    public Image[] coracoes;
    public int vida;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        painelTitulo.SetActive(true);

        heartController();

    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == gameState.TITULO && Input.GetKeyDown(KeyCode.Space))
        {
            currentState = gameState.GAMEPLAY;
            painelTitulo.SetActive(false);
        }
        else if(currentState == gameState.GAMEOVER && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(currentState == gameState.END && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void LateUpdate()
    {
        CamController();
    }

    void CamController()
    {
        float posCamX = playerTransform.position.x;
        float posCamY = playerTransform.position.y;

        // Limitadores horizontais da camera 
        if (cam.transform.position.x < limiteCamEsq.position.x && playerTransform.position.x < limiteCamEsq.position.x)
        {
            posCamX = limiteCamEsq.position.x;
        }
        else if (cam.transform.position.x > limiteCamDir.position.x && playerTransform.position.x > limiteCamDir.position.x)
        {
            posCamX = limiteCamDir.position.x;
        }

        // Limitadores verticais da camera 
        if (cam.transform.position.y < limiteCamBaixo.position.y && playerTransform.transform.position.y < limiteCamBaixo.position.y)
        {
            posCamY = limiteCamBaixo.position.y;
        }
        else if (cam.transform.position.y > limiteCamSup.position.y && playerTransform.transform.position.y > limiteCamSup.position.y)
        {
            posCamY = limiteCamSup.position.y;
        }

        Vector3 posCam = new Vector3(posCamX, posCamY, cam.transform.position.z);

        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, speedCam * Time.deltaTime);
    }


    public void playSFX(AudioClip sfxclip, float volume)
    {
        sfxSource.PlayOneShot(sfxclip, volume);
    }

    public void trocarMusica(musicaFase novaMusica)
    {
        AudioClip clip = null;

        switch (novaMusica)
        {
            case musicaFase.CAVERNA:
                clip = musicCaverna;
                break;
            case musicaFase.FLORESTA:
                clip = musicFloresta;
                break;
            case musicaFase.GAMEOVER:
                clip = musicGameOver;
                break;
            case musicaFase.THEEND:
                clip = musicEnd;
                break;
        }

        StartCoroutine("controleMusica", clip);
    }

    IEnumerator controleMusica(AudioClip musica)
    {
        float volumeMaximo = musicSource.volume;
        for(float volume = volumeMaximo; volume > 0; volume -= 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }

        musicSource.clip = musica;
        musicSource.Play();

        for(float volume = 0; volume < volumeMaximo; volume += 0.01f)
        {
            musicSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }

    }

    public void getHit()
    {
        vida -= 1;
        heartController();
        if(vida <= 0)
        {
            playerTransform.gameObject.SetActive(false);
            painelGameOver.SetActive(true);
            currentState = gameState.GAMEOVER;
            trocarMusica(musicaFase.GAMEOVER);
            
        }
    }

    public void getEnd()
    {
        playerTransform.gameObject.SetActive(false);
        painelEnd.SetActive(true);
        currentState = gameState.END;
        trocarMusica(musicaFase.THEEND);
    }

    public void Fall()
    {
        playerTransform.gameObject.SetActive(false);
        painelGameOver.SetActive(true);
        currentState = gameState.GAMEOVER;
        trocarMusica(musicaFase.GAMEOVER);
    }

    public void getCoin()
    {
        moedasColetadas += 1;
        moedasTxt.text = moedasColetadas.ToString();
    }

    
    public void heartController()
    {
        foreach(Image h in coracoes)
        {
            h.enabled = false;
        }
        for(int v = 0; v < vida; v++)
        {
            coracoes[v].enabled = true;
        }
    }




}
