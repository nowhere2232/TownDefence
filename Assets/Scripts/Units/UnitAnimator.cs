using System;
using System.Collections;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Transform _visual;
    [SerializeField] private float _spawnDuration = 0.2f;
    [SerializeField] private float _punchDuration = 0.12f;
    [SerializeField] private float _punchDistance = 0.15f;
    [SerializeField] private float _deathDuration = 0.2f;

    private Vector3 _baseScale;
    private Vector3 _baseLocalPos;
    private Coroutine _animRoutine;

    void Awake()
    {
        if (_visual == null)
            _visual = transform;
        _baseScale = _visual.localScale;
        _baseLocalPos = _visual.localPosition;
    }

    public void PlaySpawn()
    {
        StopCurrent();
        _animRoutine = StartCoroutine(SpawnRoutine());
    }

    public void PlayPunch(Vector2 dir)
    {
        StopCurrent();
        _animRoutine = StartCoroutine(PunchRoutine(dir.normalized));
    }

    public void PlayDeath(Action onComplete)
    {
        StopCurrent();
        _animRoutine = StartCoroutine(DeathRoutine(onComplete));
    }

    private void StopCurrent()
    {
        if (_animRoutine != null)
        {
            StopCoroutine(_animRoutine);
            _animRoutine = null;
            _visual.localScale = _baseScale;
            _visual.localPosition = _baseLocalPos;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        float t = 0f;
        while (t < _spawnDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / _spawnDuration);
            _visual.localScale = _baseScale * k;
            yield return null;
        }
        _visual.localScale = _baseScale;
        _animRoutine = null;
    }

    private IEnumerator PunchRoutine(Vector3 dir)
    {
        Vector3 peakPos = _baseLocalPos + dir * _punchDistance;
        float half = _punchDuration * 0.5f;

        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / half);
            _visual.localPosition = Vector3.Lerp(_baseLocalPos, peakPos, k);
            yield return null;
        }
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / half);
            _visual.localPosition = Vector3.Lerp(peakPos, _baseLocalPos, k);
            yield return null;
        }
        _visual.localPosition = _baseLocalPos;
        _animRoutine = null;
    }

    private IEnumerator DeathRoutine(Action onComplete)
    {
        Vector3 startScale = _visual.localScale;
        float t = 0f;
        while (t < _deathDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / _deathDuration);
            _visual.localScale = Vector3.Lerp(startScale, Vector3.zero, k);
            yield return null;
        }
        _visual.localScale = Vector3.zero;
        onComplete?.Invoke();
    }
}
