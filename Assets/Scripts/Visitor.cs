using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{

    Transform distraction;
    Transform lookat;
    float time = 0;
    public void Distract(Transform goal, Transform lookat, float time)
    {
        this.lookat = lookat;
        this.time = time;
        distraction = goal;
    }

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

    public bool atTable;
    private IEnumerator GoToTable()
    {
        if (atTable)
            yield break;

        agent.destination = table.transform.position;
        yield return new WaitForSeconds(1);

        // Wait till we arive
        yield return new WaitUntil(() =>
        {
            return agent.velocity.sqrMagnitude < 0.01f;
        });

        atTable = true;
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

    [SerializeField] private MeshRenderer icon;

    [SerializeField] List<Texture> Icons;

    public void SetIcon(int icon)
    {
        this.icon.material.mainTexture = Icons[icon];
    }

    private IEnumerator Start()
    {

        if (Random.Range(0, 100) < 25)
        {
            request.Add(Tags.Cut);
        }

        icon.material = new Material(icon.material);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        yield return FindATable();

        yield return GoToTable();

        animator.SetBool("Sitting", true);
        ShowRequest();
        while (true)
        {
            if (!poison)
            {
                if (distraction != null)
                {
                    if (time < 0)
                    {
                        distraction = null;
                        yield return GoToTable();
                        animator.SetBool("Sitting", true);
                        ShowRequest();
                    }
                    else
                    {
                        Debug.Log(time);
                        atTable = false;
                        animator.SetBool("Sitting", false);
                        SetIcon(9);
                        if (Vector3.Distance(transform.position, distraction.transform.position) > 1)
                            agent.destination = distraction.transform.position;
                        else
                            agent.destination = transform.position;
                        yield return new WaitUntil(() =>
                        {
                            return agent.velocity.sqrMagnitude < 0.01f;
                        });

                        Vector3 look = lookat.position;
                        look.y = agent.transform.position.y;
                        agent.transform.LookAt(look);
                        time -= .1f;
                    }
                }

                else
                {
                    GoToTable();
                }
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    public void ShowRequest()
    {
        if (request.Contains(Tags.Leg) && request.Contains(Tags.Cooked) && !request.Contains(Tags.Cut))
        {
            icon.material.mainTexture = Icons[0];
        }

        if (request.Contains(Tags.Leg) && request.Contains(Tags.Cooked) && request.Contains(Tags.Cut))
        {
            icon.material.mainTexture = Icons[1];
        }

        if (request.Contains(Tags.Potato) && request.Contains(Tags.Cooked) && !request.Contains(Tags.Cut))
        {
            icon.material.mainTexture = Icons[2];
        }

        if (request.Contains(Tags.Potato) && request.Contains(Tags.Cooked) && request.Contains(Tags.Cut))
        {
            icon.material.mainTexture = Icons[3];
        }

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
        atTable = false;
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
        toughts.text = "";
        icon.material.mainTexture = Icons[4];
        StopAllCoroutines();
        Destroy(gameObject.GetComponent<NavMeshAgent>());
        Destroy(animator);
        gameObject.AddComponent<Rigidbody>();
        gameObject.tag = "Item";
        gameObject.AddComponent<Scarer>();
        animator.ResetControllerState(true);
        Scarable scare = GetComponent<Scarable>();
        if (scare != null)
            scare.Destroy();
        Destroy(this);
    }


    bool poison;
    private IEnumerator AtePoison()
    {
        icon.material.mainTexture = Icons[5];
        poison = true;
        atTable = false;
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
        if (visitorType == VisitorType.Human)
            yield return new WaitForSeconds(200);
        else
            yield return new WaitForSeconds(10);
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
