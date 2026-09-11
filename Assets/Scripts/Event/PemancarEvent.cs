using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PemancarEvent : MonoBehaviour
{
    public static event Action OnTekanSpasi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Pemancar: spasi ditekan, kirim event.");
            OnTekanSpasi?.Invoke();
        }
        
    }
}
