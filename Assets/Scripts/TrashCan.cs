using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public void ThrowAway(GameObject game)
    {
        Destroy(game);
    }
}
