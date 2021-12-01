using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

// Build and update a localized navmesh from the sources marked by NavMeshSourceTag
[DefaultExecutionOrder(-102)]
public class LocalNavMeshBuilder : MonoBehaviour
{
    public static LocalNavMeshBuilder Instance;
    // The center of the build
    public Transform m_Tracked;

    // The size of the build bounds
    public Vector3 m_Size = new Vector3(48.0f, 20.0f, 48.0f);

    public NavMeshData m_NavMesh;
    public AsyncOperation m_Operation;
    public NavMeshDataInstance m_Instance;
    public List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();
    public bool updateNavMesh = true;
    
    public void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    public void settingOnNavMeshSize(int roomMaxDistance) {
        Vector3 updateSize = new Vector3((m_Size.x * 2) * roomMaxDistance, m_Size.y, (m_Size.z * 2) * roomMaxDistance);

        m_Size = updateSize;
    }
    IEnumerator Start()
    {
        while (updateNavMesh)
        {
            UpdateNavMesh(true);
            Debug.Log("LocalNavMeshBuilder Call");
            yield return m_Operation;
        }
    }
    public void UpdateNavMeshStatus(bool boolean)    {
        updateNavMesh = boolean;
    }
    void OnEnable()
    {
        // Construct and add navmesh
        m_NavMesh = new NavMeshData();
        m_Instance = NavMesh.AddNavMeshData(m_NavMesh);
        if (m_Tracked == null)
            m_Tracked = transform;
        UpdateNavMesh(false);
    }

    void OnDisable()
    {
        // Unload navmesh and clear handle
        m_Instance.Remove();

    }
    
    public void UpdateNavMesh(bool asyncUpdate = false)
    {
        NavMeshSourceTag.Collect(ref m_Sources);
        var defaultBuildSettings = NavMesh.GetSettingsByID(0);
        var bounds = QuantizedBounds();

        if (asyncUpdate)
            m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
        else
            NavMeshBuilder.UpdateNavMeshData(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
    }

    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    Bounds QuantizedBounds()
    {
        // Quantize the bounds to update only when theres a 10% change in size
        var center = m_Tracked ? m_Tracked.position : transform.position;
        return new Bounds(Quantize(center, 0.1f * m_Size), m_Size);
    }

    void OnDrawGizmosSelected()
    {
        if (m_NavMesh)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(m_NavMesh.sourceBounds.center, m_NavMesh.sourceBounds.size);
        }

        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = m_Tracked ? m_Tracked.position : transform.position;
        Gizmos.DrawWireCube(center, m_Size);
    }
}
