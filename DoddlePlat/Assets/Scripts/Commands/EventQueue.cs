using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using UnityEngine;

public class EventQueue : MonoBehaviour
{
    private List<ICommand> currentPhysicsCommands = new List<ICommand>();
    public static EventQueue Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void QueueCommands(ICommand command)
    {
        currentPhysicsCommands.Add(command);
    }

    private void FixedUpdate()
    {
        if (currentPhysicsCommands.Count == 0)
            return;
        
        foreach (var command in currentPhysicsCommands)
        {
            command.Execute();
        }
        
        currentPhysicsCommands.Clear();
    }
}
