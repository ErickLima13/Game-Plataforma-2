using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporte : MonoBehaviour
{
    private GameController _GameController;

    public Transform pontoSaida;
    public Transform posCamera;

    public Transform limeteCamEsc, limiteCamDir, limiteCamSup, limiteCamBaixo;
    public bool isForest = false;
    

    public musicaFase novaMusica;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            _GameController.trocarMusica(musicaFase.CAVERNA);
            col.transform.position = pontoSaida.position;
            Camera.main.transform.position = posCamera.position;

            _GameController.limiteCamEsq = limeteCamEsc;
            _GameController.limiteCamDir = limiteCamDir;
            _GameController.limiteCamSup = limiteCamSup;
            _GameController.limiteCamBaixo = limiteCamBaixo;
            print("caverna");
        }

        if(col.gameObject.tag == "Player" && isForest == true)
        {
            _GameController.trocarMusica(musicaFase.FLORESTA);
            col.transform.position = pontoSaida.position;
            Camera.main.transform.position = posCamera.position;

            _GameController.limiteCamEsq = limeteCamEsc;
            _GameController.limiteCamDir = limiteCamDir;
            _GameController.limiteCamSup = limiteCamSup;
            _GameController.limiteCamBaixo = limiteCamBaixo;
            print("floresta");


        }
    }

    


}
