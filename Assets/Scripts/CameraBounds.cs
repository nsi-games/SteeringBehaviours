using UnityEngine;
using System.Collections;

public class CameraBounds : MonoBehaviour
{
    public Vector3 size = new Vector3(20f, 0f, 20f);

    public Vector3 GetAdjustedPos(Vector3 incomingPos)
    {
        Vector3 pos = transform.position;
        Vector3 halfSize = size * 0.5f;

        if (incomingPos.z > pos.z + halfSize.z)
        {
            incomingPos.z = pos.z + halfSize.z;
        }
        if (incomingPos.z < pos.z - halfSize.z)
        {
            incomingPos.z = pos.z - halfSize.z;
        }

        if (incomingPos.x > pos.x + halfSize.x)
        {
            incomingPos.x = pos.x + halfSize.x;
        }
        if (incomingPos.x < pos.x - halfSize.x)
        {
            incomingPos.x = pos.x - halfSize.x;
        }

        return incomingPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, size);
    }
}