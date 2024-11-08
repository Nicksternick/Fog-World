using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Debuff : MonoBehaviour
{
    private NavMeshAgent enemyAgent;
    private Renderer enemyRenderer;
    private Color originalColor;

    [SerializeField] private float iceSlowFactor = 0.5f;
    [SerializeField] private float debuffDuration = 3f;

    private float originalSpeed;
    private Dictionary<Elements, Coroutine> activeDebuffs = new Dictionary<Elements, Coroutine>();

    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyRenderer = GetComponentInChildren<Renderer>();

        if (enemyAgent != null)
            originalSpeed = enemyAgent.speed;

        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;

        Debug.Log(enemyRenderer.material.name);
    }

    public void TriggerDebuff(Elements elementType)
    {
        if (activeDebuffs.ContainsKey(elementType))
        {
            StopCoroutine(activeDebuffs[elementType]);
            activeDebuffs[elementType] = StartCoroutine(ApplyDebuff(elementType));
        }
        else
        {
            activeDebuffs[elementType] = StartCoroutine(ApplyDebuff(elementType));
        }
    }

    private IEnumerator ApplyDebuff(Elements element)
    {
        switch (element)
        {
            case Elements.Ice:
                ApplyIceDebuff();
                yield return new WaitForSeconds(debuffDuration);
                ResetIceDebuff();
                break;

            default:
                yield break;
        }
        activeDebuffs.Remove(element);
    }

    private void ApplyIceDebuff()
    {
        if (enemyAgent != null)
        {
            enemyAgent.speed *= iceSlowFactor;
            if (enemyRenderer != null)
                enemyRenderer.material.color = Color.blue;
        }
    }

    private void ResetIceDebuff()
    {
        if (enemyAgent != null)
            enemyAgent.speed = originalSpeed;

        if (enemyRenderer != null)
            enemyRenderer.material.color = originalColor;
    }
}
