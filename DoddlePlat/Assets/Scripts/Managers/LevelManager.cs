using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject player; //Referencia al player
    private PlayerDamage _playerDamage;
    
    private Rino _rinoScript;
    private bool _isBossBattle;
    
    [SerializeField] private GameObject mainCamera; //Referencia a la camara
    
    [SerializeField] private GameObject portal; //Referencia a el portal
    private Portal _portalScript;
    
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

    private CollectableManager collectableManager;

    public event Action OnBossBattle;
    
    // Start is called before the first frame update
    void Start()
    {
        collectableManager = FindObjectOfType<CollectableManager>();

        if (collectableManager != null)
        {
            collectableManager.OnAllFruitsCollected += FruitsCollected;
        }

        _portalScript = portal.GetComponent<Portal>();

        if (_portalScript != null)
        {
            _portalScript.OnTpRequest += TpWaypoint;
        }

        _playerDamage = player.GetComponent<PlayerDamage>();
        if (_playerDamage != null)
        {
            _playerDamage.OnPlayerKilled += HandlerPlayerKilled;
        }
        
        _indexPlayerWaypoint = 0;
        _indexCameraWaypoint = 0;
        _indexPortalWaypoint = 0;
        
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
    
    private void TpWaypoint() //Posiciones a donde llevar al player cada vez que termina una sala
    {
        if (_isBossBattle)
        {
            StartCoroutine(VictoryScreen(1.5f));
            player.SetActive(false);
        }
        else
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
        
            portal.SetActive(false);
        }
        
        if (_indexPlayerWaypoint == _totalPlayerWaypoints)
        {
            OnBossBattle?.Invoke();
            _isBossBattle = true;
            
            _rinoScript = FindObjectOfType<Rino>();
            if (_rinoScript != null)
            {
                _rinoScript.OnBossKilled += HandlerBossKilled;
            }
        }
    }

    private void HandlerPlayerKilled()
    {
        StartCoroutine(DefeatScreen(1.5f));
    }

    private void HandlerBossKilled()
    {
        portal.SetActive(true);
    }

    private void FruitsCollected()
    {
        portal.SetActive(true);
    }
    
    private IEnumerator VictoryScreen(float delay) //Pantalla de victoria
    {
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ScreenVictory");
    }
    
    private IEnumerator DefeatScreen(float delay) //Pantalla de derrota
    {
        yield return new WaitForSeconds(delay);
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ScreenDefeat");
    }

    private void OnDisable()
    {
        if (collectableManager != null)
        {
            collectableManager.OnAllFruitsCollected -= FruitsCollected;
        }

        if (_portalScript != null)
        {
            _portalScript.OnTpRequest -= TpWaypoint;
        }

        if (_playerDamage != null)
        {
            _playerDamage.OnPlayerKilled -= HandlerPlayerKilled;
        }
        
        if (_rinoScript != null)
        {
            _rinoScript.OnBossKilled -= HandlerBossKilled;
        }
    }
}
