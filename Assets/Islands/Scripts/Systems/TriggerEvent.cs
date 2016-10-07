using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;
    // Use this for initialization
    void Start()
    {
        if (onEnter == null)
            onEnter = new UnityEvent();
        if (onExit == null)
            onExit = new UnityEvent();
    }
    //3d
    void OnTriggerEnter(Collider collider)
    {
        onEnter.Invoke();
    }
    void OnTriggerExit(Collider collider)
    {
        onExit.Invoke();
    }
    void OnCollisionEnter(Collision collision)
    {
        onEnter.Invoke();
    }
    void OnCollissionExit(Collision collision)
    {
        onExit.Invoke();
    }
    //2d
    void OnTriggerEnter2D(Collider2D collider)
    {
        onEnter.Invoke();
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        onExit.Invoke();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        onEnter.Invoke();
    }
    void OnCollissionExit2D(Collision2D collision)
    {
        onExit.Invoke();
    }
}