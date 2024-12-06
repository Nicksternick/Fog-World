using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveArrow : MonoBehaviour
{
    private GameObject currentObjective = null;

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.ImportantEnemies.Count != 0)
        {
            foreach (GameObject objective in LevelManager.Instance.ImportantEnemies)
            {
                if (objective == null)
                    continue;

                if (currentObjective == null)
                {
                    currentObjective = objective;
                }
                else
                {
                    float distanceToCurrentObjective = Vector3.Distance(transform.position, currentObjective.transform.position);
                    float distanceToNewObjective = Vector3.Distance(transform.position, objective.transform.position);
                    if (distanceToNewObjective < distanceToCurrentObjective)
                    {
                        currentObjective = objective;
                    }
                }
            }

            if (currentObjective != null)
                transform.LookAt(currentObjective.transform.position);
        }
    }
}
