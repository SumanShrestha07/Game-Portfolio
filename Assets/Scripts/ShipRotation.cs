using System;
using UnityEngine;
using PrimeTween;

public class ShipRotation : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    private void Start()
    {
       //RotateTween();
    }

    
    [SerializeField] private float speed = 90f; // degrees per second

    private void Update()
    {
        transform.localRotation *= Quaternion.Euler(0f, speed * Time.deltaTime, 0f);
           
    }
    private void RotateTween()
    {
        Debug.Log("RotateTween");
        Tween.Rotation(gameObject.transform, new Vector3(0, 360, 0),duration, Ease.Linear,cycles: -1,CycleMode.Restart);
    }
}
