using UnityEngine;

public class Table : MonoBehaviour
{
    [HideInInspector] public Visitor visitor;
    [SerializeField] public VisitorType type;
    public void FoodServed(GameObject gm)
    {
        Food food = gm.GetComponent<Food>();
        if (food == null)
        {
            visitor.toughts.text = "I can't eat this!";
            return;
        }

        foreach (Tags tag in food.tags)
        {
            if (visitor.dislikes.Contains(tag))
            {
                visitor.toughts.text = $"This is terrible, I did not ask for this!\nIt contains {tag}!";
                return;
            }
        }

        foreach (Tags tag in visitor.request)
        {
            if (!food.tags.Contains(tag))
            {
                visitor.toughts.text = $"I did not ask for this!\nIt does not contain {tag}";
                return;
            }
        }

        visitor.toughts.text = "Ohhh I love this!";

        Destroy(food.gameObject);
        visitor.LeaveNow();
    }
}
