using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class MarchingCubeRenderer : MonoBehaviour
{

    public VoxelData voxel = new VoxelData();


    public int width;
    public int height;
    public int depth;

    public Material material;

    byte isoValue = 100;


    List<Vector3> vertices = new List<Vector3>();


    public void GenerateMesh()
    {
        int w = width;
        int h = height;
        int d = depth;

        vertices.Clear();
        for (int x = -1; x < w; x++)
        {
            for (int y = -1; y < h; y++)
            {
                for (int z = -1; z < d; z++)
                {
                    Marching(x, y, z);
                }
            }
        }

        const int maxVertexCount = 63000;
        int meshCount = vertices.Count / maxVertexCount + 1;
        for (int i = 0; i < meshCount; i++)
        {
            var meshFilter = GetMeshFilter(i);
            meshFilter.gameObject.SetActive(true);
            meshFilter.sharedMesh.Clear();
            meshFilter.sharedMesh.vertices = vertices.Skip(maxVertexCount * i).Take(maxVertexCount).ToArray();
            meshFilter.sharedMesh.triangles = vertices.Skip(maxVertexCount * i).Take(maxVertexCount).Select((v, index) => index).ToArray();
            meshFilter.sharedMesh.RecalculateNormals();
        }

        int childCount = transform.childCount;

        
        for (int i = childCount; i > meshCount; i--)
        {
            var child = transform.GetChild(i - 1);
            child.SetParent(null, false);
            if (Application.isPlaying)
            {
                Destroy(child.gameObject);
            }
            else
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    void Marching(int x, int y, int z)
    {
        MarchingCube.Polygonise(x, y, z, Lookup, vertices, new Vector3(x, y, z), isoValue);
    }

    byte Lookup(int x, int y, int z)
    {
        float nx = (float)x / width;
        float ny = (float)y / depth;
        float nz = (float)z / height;
        return voxel.LinearSample(nx, ny, nz);
    }
    
    MeshFilter GetMeshFilter(int index)
    {
        if (index >= transform.childCount)
        {
            var child = new GameObject(index.ToString());
            var rend = child.AddComponent<MeshRenderer>();
            rend.sharedMaterial = material;

            var filter = child.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            mesh.MarkDynamic();
            mesh.name = index.ToString();
            mesh.Clear();
            filter.sharedMesh = mesh;

            child.hideFlags = HideFlags.HideInHierarchy;
            child.transform.SetParent(transform, false);
        }

        return transform.GetChild(index).GetComponent<MeshFilter>();
    }
   
}
