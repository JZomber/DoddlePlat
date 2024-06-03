using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private LevelManager _levelManager;

    private bool _collected;

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_collected)
        {
            _animator.SetTrigger("isCollected");
            StartCoroutine(SelfDeactivate(0.5f));
            _levelManager.FruitCollected();
            _collected = true;
        }
    }

    private IEnumerator SelfDeactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
