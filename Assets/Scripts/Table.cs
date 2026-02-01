using UnityEngine;

public class Table : MonoBehaviour
{
    [HideInInspector] public Visitor visitor;
    [SerializeField] public VisitorType type;

    public void Update()
    {
        if (visitor != null && visitor.atTable)
            visitor.transform.forward = transform.up;
    }

    public void FoodServed(GameObject gm)
    {
        if (gm == null)
        {
            visitor.ShowRequest();
            return;
        }

        Food food = gm.GetComponent<Food>();
        if (food == null)
        {
            visitor.SetIcon(6);
            return;
        }

        foreach (Tags tag in food.tags)
        {
            if (visitor.dislikes.Contains(tag))
            {
                visitor.SetIcon(6);
                return;
            }
        }

        foreach (Tags tag in visitor.request)
        {
            if (!food.tags.Contains(tag))
            {
                visitor.SetIcon(6);
                return;
            }
        }

        visitor.toughts.text = "Ohhh I love this!";

        visitor.SetIcon(7);

        Destroy(food.gameObject);
        if (food.tags.Contains(Tags.Poison))
        {
            FindAnyObjectByType<ScoreCounter>().AddScore(100);
            visitor.AtePoisonNow();
        }

        else
        {
            if (visitor.visitorType == VisitorType.Human)
                FindAnyObjectByType<ScoreCounter>().AddScore(100);

            if (visitor.visitorType == VisitorType.Monster)
                FindAnyObjectByType<ScoreCounter>().AddScore(1000);

            visitor.LeaveNow();
        }
    }
}
