
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreppingStation : MonoBehaviour
{
    Food food;
    float time = 0;
    Knive knive;


    private void Start()
    {
        if (requiresKnife)
        {
            knive = GetComponent<Knive>();
            alert = GetComponentInChildren<TMP_Text>();
        }
    }
    [SerializeField] bool requiresKnife;
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

    TMP_Text alert;

    private void Update()
    {
        if (food == null)
        {
            alert.text = "";
            return;
        }

        if (requiresKnife && Vector3.Distance(knive.transform.position, transform.position) > 3 && knive.transform.parent != null) // Assume parent is player
        {
            alert.text = "Must Hold Knife!";
            return;
        }
        else { alert.text = ""; }


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
