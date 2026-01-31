
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{

    public Animator animator;
    [SerializeField] public VisitorType visitorType;

    Table table = null;
    NavMeshAgent agent;

    private void Update()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    /// <summary>
    /// Wait for a table to be available of our type
    /// </summary>
    /// <returns></returns>
    private IEnumerator FindATable()
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

    private IEnumerator GoToTable()
    {

        agent.destination = table.transform.position;
        yield return new WaitForSeconds(1);

        // Wait till we arive
        yield return new WaitUntil(() =>
        {
            return agent.velocity.sqrMagnitude < 0.01f;
        });
    }

    private IEnumerator WaitForPlayerNear()
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
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        yield return FindATable();

        yield return GoToTable();

        animator.SetBool("Sitting", true);
        toughts.text = "Waiting...";

        //  yield return WaitForPlayerNear();

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

        //   yield return LeaveAfterAWhile();
    }

    private IEnumerator LeaveAfterAWhile()
    {
        yield return new WaitForSeconds(120f);

        toughts.text = "This is taking so long, I am leaving soon!";
        yield return new WaitForSeconds(30f);

        toughts.text = "This took way to long!";
        yield return Leave();
    }


    private IEnumerator Leave()
    {
        animator.SetBool("Sitting", false);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = startPosition;
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() =>
        {
            return agent.velocity.sqrMagnitude < 0.01f;
        });

        Destroy(gameObject);
    }

    public void LeaveNow()
    {
        StartCoroutine(Leave());
    }

    public void AtePoisonNow()
    {
        StartCoroutine(AtePoison());
    }

    public void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject.GetComponent<NavMeshAgent>());
        Destroy(animator);
        gameObject.AddComponent<Rigidbody>();
        gameObject.tag = "Item";
        gameObject.AddComponent<Scarer>();
        animator.ResetControllerState(true);
        Destroy(this);
    }

    private IEnumerator AtePoison()
    {
        yield return new WaitForSeconds(3);
        toughts.text = "I am not feeling well.";
        animator.SetBool("Sitting", false);
        yield return new WaitForSeconds(1);

        Toilet toilet = FindAnyObjectByType<Toilet>();

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = toilet.transform.position;

        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() =>
        {
            return agent.velocity.sqrMagnitude < 0.01f;
        });

        animator.SetBool("Barfing", true);
        yield return new WaitForSeconds(200);
        animator.SetBool("Barfing", false);
        LeaveNow();

    }

    Vector3 startPosition;

    [SerializeField] public TMP_Text toughts;
    [SerializeField] public List<Tags> request;
    [SerializeField] public List<Tags> dislikes;
}

public enum VisitorType
{
    Monster,
    Human
}
