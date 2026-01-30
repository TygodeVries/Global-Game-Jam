
using System;
using System.Collections.Generic;
using UnityEngine;

public class PreppingStation : MonoBehaviour
{
    Food food;
    float time = 0;
    public void OnPlaceItem(GameObject gameObject)
    {
        if (gameObject != null)
        {
            Food food = gameObject.GetComponent<Food>();
            this.food = food;
        }
        else
        {
            food = null;
        }

        Debug.Log("Updated!");
        step = 0;
        time = 0;
    }

    private void Update()
    {
        if (food == null)
        {
            return;
        }

        time += Time.deltaTime;

        if (step < preppingSteps.Count && time > preppingSteps[step].time)
        {
            food.AddTag(preppingSteps[step].tag);
            step++;
        }
    }

    int step = 0;
    [SerializeField] private List<PreppingStep> preppingSteps = new List<PreppingStep>();
}

[Serializable]
public class PreppingStep
{
    public float time;
    public Tags tag;
}
