using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnPoint : MonoBehaviour
{
    Mesh mesh = null;
    public int ID;
    // Start is called before the first frame update
    void Start()
    {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
    }
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Vector3 pos = this.transform.position + Vector3.up * this.transform.localPosition.y * 1.5f;
        Gizmos.color = Color.red;
        if (this.mesh != null)
        {
            Gizmos.DrawWireMesh(this.mesh,pos, this.transform.rotation, this.transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, this.transform.position, this.transform.rotation, 1f, EventType.Repaint);
        UnityEditor.Handles.Label(pos,"Spawnpoint:"+this.ID);
    }

#endif
}
