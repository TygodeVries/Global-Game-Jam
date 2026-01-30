
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    [SerializeField] public VisitorType visitorType;

    Table table = null;


    /// <summary>
    /// Wait for a table to be available of our type
    /// </summary>
    /// <returns></returns>
    public IEnumerator FindATable()
    {
        while (table == null)
        {
            Table[] tables = FindObjectsByType<Table>(FindObjectsSortMode.None);

            foreach (Table t in tables)
            {
                if (t.type == visitorType && t.visitor == null)
                {
                    table = t;
                    t.visitor = this;
                    break;
                }
            }

            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator GoToTable()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        agent.destination = table.transform.position;
        yield return new WaitForSeconds(1);

        // Wait till we arive
        yield return new WaitUntil(() =>
        {
            return agent.velocity.sqrMagnitude < 0.01f;
        });
    }

    public IEnumerator WaitForPlayerNear()
    {
        bool waitingForPlayer = true;
        while (waitingForPlayer)
        {
            PlayerMovement[] players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

            foreach (PlayerMovement p in players)
            {
                float dis = Vector3.Distance(p.transform.position, transform.position);
                if (dis < 3)
                {
                    waitingForPlayer = false;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator Start()
    {
        yield return FindATable();

        yield return GoToTable();

        toughts.text = "Waiting...";

        yield return WaitForPlayerNear();

        toughts.text = "I want:\n";
        foreach (Tags tag in request)
        {
            toughts.text += $"- {tag}\n";
        }

        toughts.text += "I don't want:\n";
        foreach (Tags tag in dislikes)
        {
            toughts.text += $"- {tag}\n";
        }
    }


    [SerializeField] public TMP_Text toughts;
    [SerializeField] public List<Tags> request;
    [SerializeField] public List<Tags> dislikes;
}

public enum VisitorType
{
    Monster,
    Human
}
