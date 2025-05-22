using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;

    public UnityEvent onPress;

    public UnityEvent onRelease;

    private GameObject presser;

    private AudioSource sound;

    private bool isPressed;

    public GameObject pointerPrefab;

    private static readonly Vector3 SpawnPosition = new Vector3(0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            isPressed = false;
        }

    }

    public void SpawnPointer()
    {
        Instantiate(pointerPrefab, SpawnPosition, Quaternion.identity);
    }
}
