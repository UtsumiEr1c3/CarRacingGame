using System;
using UnityEngine;
using UnityEngine.LowLevelPhysics;

public class WorldGenerator : MonoBehaviour
{
    public Material meshMaterial;
    public float scale;
    public float perlinScale;
    public float offset;
    public Vector2 dimension;
    public float waveHeight;

    private void Start()
    {
        CreateCylinder();
    }

    /// <summary>
    /// Mesh 通过网格绘制的, MeshFilter 持有 Mesh 的引用, MeshRenderer 
    /// </summary>
    void CreateCylinder()
    {
        // 创建 GameObject
        GameObject newCylinder = new GameObject();
        newCylinder.name = "WorldPieces";

        // 添加组件
        MeshFilter meshFilter = newCylinder.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newCylinder.AddComponent<MeshRenderer>();

        // 材质
        meshRenderer.material = meshMaterial;
        meshFilter.mesh = GenerateMesh();

        // 添加碰撞体
        newCylinder.AddComponent<MeshCollider>();
    }

    /// <summary>
    /// 创建一个网格
    /// </summary>
    /// <returns></returns>
    Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "MESH";

        // 需要uv, 顶点, 三角形等数据
        Vector3[] vertices = null;
        Vector2[] uvs = null;
        int[] triangles = null;

        // 创建形状
        CreateShape(ref vertices, ref uvs, ref triangles);

        // 赋值
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    /// <summary>
    /// 创建形状
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="uvs"></param>
    /// <param name="triangles"></param>
    void CreateShape(ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles)
    {
        // 向 z 轴里面延伸, x是横截面
        int xCount = (int)dimension.x;
        int zCount = (int)dimension.y;

        // 初始化顶点和 uv 数组, 通过定义的尺寸
        vertices = new Vector3[(xCount + 1) * (zCount + 1)];
        uvs = new Vector2[(xCount + 1) * (zCount + 1)];

        int index = 0;

        // 半径计算
        float radius = xCount * scale * 0.5f;

        // 通过一个双循环, 设置顶点和uv
        for (int x = 0; x < xCount + 1; x++)
        {
            for (int z = 0; z < zCount + 1; z++)
            {
                // 首先获取圆柱体的角度, 根据 x 的位置
                float angle = x * Mathf.PI * 2f / xCount;

                // 通过角度计算了顶点的值
                vertices[index] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, z * scale * Mathf.PI);

                // 计算 uv
                uvs[index] = new Vector2(x * scale, z * scale);

                // 用柏林噪声
                float pX = (vertices[index].x * perlinScale) * offset;
                float pZ = (vertices[index].z * perlinScale) * offset;

                // 需要一个中心点和当前的顶点做减法, 然后归一化, 再去计算柏林噪声
                Vector3 center = new Vector3(0, 0, vertices[index].z);
                vertices[index] += (center - vertices[index]).normalized * Mathf.PerlinNoise(pX, pZ) * waveHeight;

                index++;
            }
        }

        // 初始化三角形数组, x * z, 1个矩形2个三角形, 1个三角形3个顶点, 那么1个矩形6个顶点
        triangles = new int[xCount * zCount * 6];

        // 创建一个数组, 存储6个三角形顶点
        int[] boxBase = new int[6];
        int current = 0;

        for (int x = 0; x < xCount; x++)
        {
            // 每次重新赋值, 根据x的变化
            boxBase = new int[]
            {
                x * (zCount + 1),
                x * (zCount + 1) + 1,
                (x + 1) * (zCount + 1),
                x * (zCount + 1) + 1,
                (x + 1) * (zCount + 1) + 1,
                (x + 1) * (zCount + 1),
            };

            for (int z = 0; z < zCount; z++)
            {
                // 增长索引
                for (int i = 0; i < 6; i++)
                {
                    boxBase[i] = boxBase[i] + 1;
                }

                // 把6个顶点填充到三角形里
                for (int j = 0; j < 6; j++)
                {
                    triangles[current + j] = boxBase[j] - 1;
                }

                current += 6;
            }
        }
    }
}
