using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerLivesText;
    private int _playerLives = 5;
    
    [SerializeField] private GameObject bossUI;
    [SerializeField] private TMP_Text bossLivesText;
    private int _bossLives = 5;

    private LevelManager _levelManager;

    private PlayerDamage _playerDamage;

    private Rino _rinoScript;
    
    // Start is called before the first frame update
    void Start()
    {
        playerLivesText.SetText(_playerLives.ToString());
        bossUI.SetActive(false);

        _levelManager = FindObjectOfType<LevelManager>();
        if (_levelManager != null)
        {
            _levelManager.OnBossBattle += HandlerBossBattle;
        }

        _playerDamage = FindObjectOfType<PlayerDamage>();
        if (_playerDamage != null)
        {
            _playerDamage.OnTakeDamage += HandlerUpdatePlayerUI;
        }
    }

    private void HandlerUpdatePlayerUI()
    {
        _playerLives--;
        playerLivesText.SetText(_playerLives.ToString());
    }

    private void HandlerBossBattle()
    {
        Rino boss = FindObjectOfType<Rino>();
        
        _rinoScript = boss.GetComponent<Rino>();
        if (_rinoScript != null)
        {
            _rinoScript.OnTakeDamage += HandlerUpdateBossUI;
        }
        
        bossUI.SetActive(true);
        bossLivesText.SetText(_bossLives.ToString());
        
        _levelManager.OnBossBattle -= HandlerBossBattle;
    }
    
    private void HandlerUpdateBossUI()
    {
        _bossLives--;
        bossLivesText.SetText(_bossLives.ToString());
    }

    private void OnDisable()
    {
        if (_playerDamage != null)
        {
            _playerDamage.OnTakeDamage -= HandlerUpdatePlayerUI;
        }
        
        if (_rinoScript != null)
        {
            _rinoScript.OnTakeDamage -= HandlerUpdateBossUI;
        }
    }
}
