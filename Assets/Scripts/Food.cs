using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public List<Tags> tags = new List<Tags>();

    public void AddTag(Tags tag)
    {
        if (!tags.Contains(tag))
        {
            BroadcastMessage("TagApplied", tag);
            tags.Add(tag);
        }
    }
}
