using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class SerializableMeshInfo
{
    private float[] vertices;
    private int[] triangles;
    private float[] uv;
    private float[] uv2;
    private float[] normals;
    private float[] colors;

    public SerializableMeshInfo(Mesh m) // Constructor: takes a mesh and fills out SerializableMeshInfo data structure which basically mirrors Mesh object's parts.
    {
        vertices = new float[m.vertexCount * 3]; // initialize flattened vertices array.
        for (int i = 0; i < m.vertexCount; i++)
        {
            vertices[i * 3] = m.vertices[i].x;
            vertices[i * 3 + 1] = m.vertices[i].y;
            vertices[i * 3 + 2] = m.vertices[i].z;
        }
        triangles = new int[m.triangles.Length]; // initialize triangles array (1-dimensional so no need for flattening)
        for (int i = 0; i < m.triangles.Length; i++)
        {
            triangles[i] = m.triangles[i];
        }
        uv = new float[m.uv.Length * 2]; // initialize flattened uvs array
        for (int i = 0; i < m.uv.Length; i++)
        {
            uv[i * 2] = m.uv[i].x;
            uv[i * 2 + 1] = m.uv[i].y;
        }
        uv2 = new float[m.uv2.Length * 2]; // uv2
        for (int i = 0; i < m.uv2.Length; i++)
        {
            uv2[i * 2] = m.uv2[i].x;
            uv2[i * 2 + 1] = m.uv2[i].y;
        }
        normals = new float[m.normals.Length * 3]; // initialize flattened normals array
        for (int i = 0; i < m.normals.Length; i++)
        {
            normals[i * 3] = m.normals[i].x;
            normals[i * 3 + 1] = m.normals[i].y;
            normals[i * 3 + 2] = m.normals[i].z;
        }

        colors = new float[m.colors.Length * 4];
        for (int i = 0; i < m.colors.Length; i++)
        {
            colors[i * 4] = m.colors[i].r;
            colors[i * 4 + 1] = m.colors[i].g;
            colors[i * 4 + 2] = m.colors[i].b;
            colors[i * 4 + 3] = m.colors[i].a;
        }
    }

    // GetMesh gets a Mesh object from the current data in this SerializableMeshInfo object.
    // Sequential values are deserialized to Mesh original data types like Vector3 for vertices.
    public Mesh GetMesh()
    {
        Mesh m = new Mesh();
        List<Vector3> verticesList = new List<Vector3>();
        for (int i = 0; i < vertices.Length / 3; i++)
        {
            verticesList.Add(new Vector3(
                    vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2]
                ));
        }
        m.SetVertices(verticesList);
        m.triangles = triangles;
        List<Vector2> uvList = new List<Vector2>();
        for (int i = 0; i < uv.Length / 2; i++)
        {
            uvList.Add(new Vector2(
                    uv[i * 2], uv[i * 2 + 1]
                ));
        }
        m.SetUVs(0, uvList);
        List<Vector2> uv2List = new List<Vector2>();
        for (int i = 0; i < uv2.Length / 2; i++)
        {
            uv2List.Add(new Vector2(
                    uv2[i * 2], uv2[i * 2 + 1]
                ));
        }
        m.SetUVs(1, uv2List);
        List<Vector3> normalsList = new List<Vector3>();
        for (int i = 0; i < normals.Length / 3; i++)
        {
            normalsList.Add(new Vector3(
                    normals[i * 3], normals[i * 3 + 1], normals[i * 3 + 2]
                ));
        }
        m.SetNormals(normalsList);

        List<Color> colorsList = new List<Color>();
        for (int i = 0; i < colors.Length / 4; i++)
        {
            colorsList.Add(new Color(
                    colors[i * 4],
                    colors[i * 4 + 1],
                    colors[i * 4 + 2],
                    colors[i * 4 + 3]
                ));
        }
        m.SetColors(colorsList);

        return m;
    }

}
