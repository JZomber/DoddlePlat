using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject player; //Referencia al player
    public int playerLives;
    private int _playerCurrentLives;
    
    [SerializeField] private GameObject mainCamera; //Referencia a la camara
    
    [SerializeField] private GameObject portal; //Referencia a el portal
    
    [SerializeField] private GameObject holderPlayerWaypoints; //Objeto que almacena las pocisiones a donde "teletransportar" al player
    [SerializeField] private Transform[] playerWaypoints; //Array de posiciones
    private int _totalPlayerWaypoints;
    private int _indexPlayerWaypoint;
    
    [SerializeField] private GameObject holderCameraWaypoints; //Objeto que almacena las pocisiones a donde "teletransportar" a la cámara
    [SerializeField] private Transform[] cameraWaypoints; //Array de posiciones
    private int _totalCameraWaypoints;
    private int _indexCameraWaypoint;
    
    [SerializeField] private GameObject holderPortalWaypoints; //Objeto que almacena las pocisiones a donde "teletransportar" al portal
    [SerializeField] private Transform[] portalWaypoints; //Array de posiciones
    private int _totalPortalWaypoints;
    private int _indexPortalWaypoint;
    
    [SerializeField] private Animator transition; //Transición entre escenas

    [SerializeField] public int fruitCounter; //Frutas recolectadas
    
    // Start is called before the first frame update
    void Start()
    {
        _indexPlayerWaypoint = 0;
        _indexCameraWaypoint = 0;
        _indexPortalWaypoint = 0;

        fruitCounter = 0;
        
        _playerCurrentLives = playerLives;
        
        //============================================================
        
        _totalPlayerWaypoints = holderPlayerWaypoints.transform.childCount;
        playerWaypoints = new Transform[_totalPlayerWaypoints];

        for (int i = 0; i < _totalPlayerWaypoints; i++)
        {
            playerWaypoints[i] = holderPlayerWaypoints.transform.GetChild(i).transform;
        }
        
        //============================================================
        
        _totalCameraWaypoints = holderCameraWaypoints.transform.childCount;
        cameraWaypoints = new Transform[_totalCameraWaypoints];

        for (int i = 0; i < _totalCameraWaypoints; i++)
        {
            cameraWaypoints[i] = holderCameraWaypoints.transform.GetChild(i).transform;
        }
        
        //============================================================
        
        _totalPortalWaypoints = holderPortalWaypoints.transform.childCount;
        portalWaypoints = new Transform[_totalPortalWaypoints];

        for (int i = 0; i < _totalPortalWaypoints; i++)
        {
            portalWaypoints[i] = holderPortalWaypoints.transform.GetChild(i).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fruitCounter >= 5)
        {
            portal.SetActive(true);
        }

        if (_playerCurrentLives <= 0)
        {
            StartCoroutine(DefeatScreen(1.5f));
        }
    }
    
    public void TpWaypoint() //Posiciones a donde llevar al player cada vez que termina una sala
    {
        player.transform.position = playerWaypoints[_indexPlayerWaypoint].transform.position;
        
        player.GetComponent<PlayerDamage>().PlayerSpawnRelocate();
        
        //============================================================

        mainCamera.transform.position = cameraWaypoints[_indexCameraWaypoint].transform.position;
        
        portal.transform.position = portalWaypoints[_indexPortalWaypoint].transform.position;
        
        //============================================================
        
        _indexPlayerWaypoint++;
        _indexCameraWaypoint++;
        _indexPortalWaypoint++;
        
        fruitCounter = 0;
        portal.SetActive(false);
    }

    public void PlayerTakeDamage()
    {
        _playerCurrentLives--;
    }
    
    public IEnumerator VictoryScreen(float delay) //Pantalla de victoria
    {
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ScreenVictory");
    }
    
    public IEnumerator DefeatScreen(float delay) //Pantalla de derrota
    {
        yield return new WaitForSeconds(delay);
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ScreenDefeat");
    }
}
