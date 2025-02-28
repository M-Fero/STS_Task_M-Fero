using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public int GetNextIndex(int wayPointIndex)
    {
        if(wayPointIndex + 1 == transform.childCount)
        {
            return 0;
        }
        return wayPointIndex + 1;
    }

    public Vector3 GetWayPoint(int i)
    {
        return transform.GetChild(i).position;
    }
}
