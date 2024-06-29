using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, ICollectable
{
    [SerializeField] private Animator _animator;

    private LevelManager _levelManager;

    private bool _collected;

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private IEnumerator SelfDeactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void CollectItem()
    {
        if (!_collected)
        {
            _animator.SetTrigger("isCollected");
            StartCoroutine(SelfDeactivate(0.5f));
            _levelManager.FruitCollected();
            _collected = true;
        }
    }
}
