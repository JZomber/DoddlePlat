using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Collectable : MonoBehaviour, ICollectable
{
    [SerializeField] private Animator animator;

    private bool _collected;

    public event Action OnFruitCollected; 

    private void Start()
    {
        
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
            animator.SetTrigger("isCollected");
            OnFruitCollected?.Invoke();
            StartCoroutine(SelfDeactivate(0.5f));
            _collected = true;
        }
    }
}
