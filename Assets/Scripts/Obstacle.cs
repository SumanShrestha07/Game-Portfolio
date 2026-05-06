using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private Slider slider;
    
    [SerializeField] private string title,type,description;

    [Header("Movement Settings")]
    [SerializeField] private float fallSpeed = 1.5f;
    [SerializeField] private float driftSpeed = 1.2f;
    [SerializeField] private float driftAmplitude = 2f;
    [SerializeField] private float diagonalHorizontalSpeed = 2f;
    [SerializeField] private float diagonalMultiplier = 5f;
    [SerializeField] private float curveAcceleration = 1.5f;

    private int health;
    private float driftOffset;
    private Camera mainCamera;

    private enum MovementPattern
    {
        FloatDown,
        DiagonalLeft,
        DiagonalRight,
        CurveLeft,
        CurveRight,
        Spiral,
        Dive,
    }

    private MovementPattern currentPattern;

    private void OnEnable()
    {
        health = Random.Range(5, 11);
        slider.value = health;
        slider.maxValue = health;
        mainCamera = Camera.main;
        driftOffset = Random.Range(0f, Mathf.PI * 2f);
        currentPattern = (MovementPattern)Random.Range(0, System.Enum.GetValues(typeof(MovementPattern)).Length);
        StartCoroutine(MoveRoutine());
    }

    #region MoveLogic
    private IEnumerator MoveRoutine()
    {
        switch (currentPattern)
        {
            case MovementPattern.FloatDown:     yield return StartCoroutine(FloatDownRoutine());   break;
            case MovementPattern.DiagonalLeft:  yield return StartCoroutine(DiagonalRoutine(-1));  break;
            case MovementPattern.DiagonalRight: yield return StartCoroutine(DiagonalRoutine(1));   break;
            case MovementPattern.CurveLeft:     yield return StartCoroutine(CurveRoutine(-1));     break;
            case MovementPattern.CurveRight:    yield return StartCoroutine(CurveRoutine(1));      break;
            case MovementPattern.Spiral:        yield return StartCoroutine(SpiralRoutine());      break;
            case MovementPattern.Dive:          yield return StartCoroutine(DiveRoutine());        break;
        }
    }

    private IEnumerator FloatDownRoutine()
    {
        float time = 0f;
        Vector3 startPos = obstacle.transform.position;

        while (true)
        {
            time += Time.deltaTime;
            float newX = startPos.x + Mathf.Sin((time * driftSpeed) + driftOffset) * driftAmplitude;
            float newY = obstacle.transform.position.y - fallSpeed * Time.deltaTime;
            obstacle.transform.position = new Vector3(newX, newY, 0f);
            if (IsOffScreen()) { Destroy(obstacle); yield break; }
            yield return null;
        }
    }

    private IEnumerator DiagonalRoutine(int direction)
    {
        while (true)
        {
            float newX = obstacle.transform.position.x + direction * diagonalHorizontalSpeed * diagonalMultiplier * Time.deltaTime;
            float newY = obstacle.transform.position.y - fallSpeed * Time.deltaTime;
            obstacle.transform.position = new Vector3(newX, newY, 0f);
            if (IsOffScreen()) { Destroy(obstacle); yield break; }
            yield return null;
        }
    }
    
    private IEnumerator CurveRoutine(int direction)
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;
            float horizontalPush = direction * time * curveAcceleration;
            float newX = obstacle.transform.position.x + horizontalPush * Time.deltaTime;
            float newY = obstacle.transform.position.y - fallSpeed * Time.deltaTime;
            obstacle.transform.position = new Vector3(newX, newY, 0f);
            if (IsOffScreen()) { Destroy(obstacle); yield break; }
            yield return null;
        }
    }

    private IEnumerator SpiralRoutine()
    {
        float time = 0f;
        Vector3 startPos = obstacle.transform.position;
        float radius = driftAmplitude;

        while (true)
        {
            time += Time.deltaTime;
            float shrinkingRadius = radius * Mathf.Max(0.1f, 1f - time * 0.1f);
            float newX = startPos.x + Mathf.Sin(time * driftSpeed * 1.5f) * shrinkingRadius;
            float newY = startPos.y - (fallSpeed * time) + Mathf.Cos(time * driftSpeed * 1.5f) * shrinkingRadius * 0.4f;
            obstacle.transform.position = new Vector3(newX, newY, 0f);
            if (IsOffScreen()) { Destroy(obstacle); yield break; }
            yield return null;
        }
    }

    private IEnumerator DiveRoutine()
    {
        float time = 0f;
        float hoverDuration = 1.5f;

        while (true)
        {
            time += Time.deltaTime;

            if (time < hoverDuration)
            {
                float currentFallSpeed = fallSpeed * 0.2f;
                float newX = obstacle.transform.position.x + Mathf.Sin(time * 3f) * 0.5f * Time.deltaTime;
                float newY = obstacle.transform.position.y - currentFallSpeed * Time.deltaTime;
                obstacle.transform.position = new Vector3(newX, newY, 0f);
            }
            else
            {
                float diveAcceleration = (time - hoverDuration) * 3f;
                float currentFallSpeed = fallSpeed + diveAcceleration;
                float newY = obstacle.transform.position.y - currentFallSpeed * Time.deltaTime;
                obstacle.transform.position = new Vector3(obstacle.transform.position.x, newY, 0f);
            }

            if (IsOffScreen()) { Destroy(obstacle); yield break; }
            yield return null;
        }
    }

    private bool IsOffScreen()
    {
        if (mainCamera == null) return false;
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(obstacle.transform.position);
        return viewportPos.y < -0.1f || viewportPos.x < -0.5f || viewportPos.x > 1.5f;
    }
    
#endregion

    public void TakeDamage()
    {
        health--;
        slider.value = health;
        if (health == 0) EnemyDestroyed();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().AsteroidCrash();
            obstacle.SetActive(false);
        }
    }

    private void EnemyDestroyed()
    {
        SummaryManager.Instance.ShowSummary(title,type,description);
        Destroy(obstacle);
    }
}