using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EnemyBat : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] [Tooltip("Rango en el cual el enemigo puede detectar al Player")]
    private float detectionRange;

    //[SerializeField] [Tooltip("Visualiza el rango de detección (DEBUG purpose)")]
    //private GameObject debugDetectionArea;
    
    private float _distance;

    [SerializeField] private float speed;

    private EnemyDamage _enemyDamage;
    
    // Start is called before the first frame update
    void Start()
    {
        _enemyDamage = GetComponent<EnemyDamage>();
        //debugDetectionArea.transform.localScale = new Vector3(detectionRange, detectionRange, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (target && _enemyDamage.currentLives > 0)
        {
            // Perseguir al Jugador
            _distance = Vector2.Distance(transform.position, target.transform.position);
                
            if (_distance < detectionRange*2)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
                
                _enemyDamage.animator.SetBool("playerDetected", true);
            }
        }
    }
}
