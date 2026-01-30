using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public List<Tags> tags = new List<Tags>();

    public void AddTag(Tags tag)
    {
        tags.Add(tag);
    }
}
