using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传送点类
/// </summary>
public class TeleporterObject : MonoBehaviour
{
    public int ID;

    Mesh mesh = null;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(mesh != null)
        {
            Gizmos.DrawWireMesh(mesh, transform.position + Vector3.up * transform.localScale.y * 0.01f, transform.rotation, transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0,transform.position,transform.rotation,1f,EventType.Repaint);
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        PlayerInputController playerController = other.GetComponent<PlayerInputController>();
        if(playerController != null && playerController.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[ID];
            if(td == null)
            {
                Debug.LogFormat("TeleporterObject: Character [{0}] Enter Teleporter [{1}], But TeleporterDefine not existed", playerController.character.Name, ID);
                return;
            }
            Debug.LogFormat("TeleporterObject: Character [{0}] Enter Teleporter [{1}]:[{2}]", playerController.character.Name, td.ID, td.Name);
            if(td.LinkTo > 0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                {
                    MapService.Instance.SendMapTeleporter(ID);
                }
                else
                {
                    Debug.LogErrorFormat("Teleporter ID:[{0}] LinkTo [{1}] error!!",td.ID,td.LinkTo);
                }
            }
        }
    }
}
