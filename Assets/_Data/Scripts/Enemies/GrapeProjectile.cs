using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY;
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private GameObject splatterPrefab;

    private void Start()
    {
        GameObject shadow = Instantiate(shadowPrefab, transform.position + new Vector3(0f, -0.3f, 0), Quaternion.identity);
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 starPosShadow = shadow.transform.position;
        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveGrapShadowRoutine(shadow, starPosShadow, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 starPos, Vector3 endPos)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(starPos, endPos, linearT) + new Vector2(0f, height);
            yield return null;
        }

        Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveGrapShadowRoutine(GameObject shadow, Vector3 starPos, Vector3 endPos)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            shadow.transform.position = Vector2.Lerp(starPos, endPos, linearT);
            yield return null;
        }

        Destroy(shadow);
    }
}
