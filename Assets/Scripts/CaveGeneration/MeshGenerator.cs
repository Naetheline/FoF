using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Source : Sebastian Lague tutorial Procedural Cave Generation
// Youtube video serie : https://www.youtube.com/watch?v=2gIxh8CX3Hk

public class MeshGenerator : MonoBehaviour
{

    public const int NO_VERTEX_FOUND = -1;
    public const float WALL_HEIGHT = 5f;

    public const int TILE_AMOUNT = 5;

    public SquareGrid squareGrid;
    public MeshFilter walls;
    public MeshFilter cave;

    

    public bool is2D = false;

    List<Vector3> vertices;
    List<int> triangles;

    Dictionary<int, List<Triangle>> triangleDict = new Dictionary<int, List<Triangle> >();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();

    public void GenerateMesh(int[,] map, float squareSize)
    {
        outlines.Clear();
        checkedVertices.Clear();
        triangleDict.Clear();

        squareGrid = new SquareGrid(map, squareSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();


        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        cave.mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            float percentX = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].x) * TILE_AMOUNT;
            float percentY = Mathf.InverseLerp(-map.GetLength(1) / 2 * squareSize, map.GetLength(1) / 2 * squareSize, vertices[i].z) * TILE_AMOUNT;

            uvs[i] = new Vector2(percentX, percentY);
        }

        mesh.uv = uvs;

        if (!is2D)
        {
            CreateWallMesh();
        }
    }

    void CreateWallMesh()
    {
        CalculateMeshOutlines();

        List<Vector3> wallVertices = new List<Vector3>();
        List<int> wallTriangles = new List<int>();
        Mesh wallMesh = new Mesh();

        foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count - 1; i++)
            {
                int startIndex = wallVertices.Count;
                wallVertices.Add(vertices[outline[i]]); // Left vertex
                wallVertices.Add(vertices[outline[i + 1]]); // Right vertex
                wallVertices.Add(vertices[outline[i]] - Vector3.up * WALL_HEIGHT); // Bottom Left vertex
                wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * WALL_HEIGHT); // Bootom Right vertex

                wallTriangles.Add(startIndex + 0);
                wallTriangles.Add(startIndex + 2);
                wallTriangles.Add(startIndex + 3);

                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex + 0);

            }
        }
        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        walls.mesh = wallMesh;

        Vector2[] uvs = new Vector2[wallVertices.Count];
        for (int i = 0; i < wallVertices.Count; i++)
        {
            float percentX = Mathf.InverseLerp(0, 100, wallVertices[i].x) * TILE_AMOUNT ;
            float percentY = Mathf.InverseLerp(0, 100, wallVertices[i].z) * TILE_AMOUNT ;

            uvs[i] = new Vector2(percentX, percentY);
        }

        wallMesh.uv = uvs;


        MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider>();
        wallCollider.sharedMesh = wallMesh;
    }

    // Cut the squares into triangle for the mesh
    void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            case 0: break;
            // 1 point active
            case 1: MeshFromPoints(square.centerLeft, square.centerBottom, square.bottomLeft); break;
            case 2: MeshFromPoints(square.bottomRight, square.centerBottom, square.centerRight); break;
            case 4: MeshFromPoints(square.topRight, square.centerRight, square.centerTop); break;
            case 8: MeshFromPoints(square.topLeft, square.centerTop, square.centerLeft); break;

            // 2 points active
            case 3: MeshFromPoints(square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft); break;
            case 6: MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.centerBottom); break;
            case 9: MeshFromPoints(square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft); break;
            case 12: MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerLeft); break;
            case 5: MeshFromPoints(square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft); break;
            case 10: MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft); break;

            // 3 points active
            case 7: MeshFromPoints(square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft); break;
            case 11: MeshFromPoints(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft); break;
            case 13: MeshFromPoints(square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft); break;
            case 14: MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft); break;
            // 4 points active
            case 15: MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                checkedVertices.Add(square.topLeft.vertexIndex);
                checkedVertices.Add(square.topRight.vertexIndex);
                checkedVertices.Add(square.bottomRight.vertexIndex);
                checkedVertices.Add(square.bottomLeft.vertexIndex);
                break;
            default: break;

        }
    }

    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
    }

    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if(points[i].vertexIndex == NO_VERTEX_FOUND)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDict(a.vertexIndex, triangle);
        AddTriangleToDict(b.vertexIndex, triangle);
        AddTriangleToDict(c.vertexIndex, triangle);
    }

    void AddTriangleToDict(int vertexIndexKey, Triangle triangle)
    {
        if(triangleDict.ContainsKey(vertexIndexKey))
        {
            triangleDict[vertexIndexKey].Add(triangle);
        }
        else
        {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            triangleDict.Add(vertexIndexKey, triangleList);
        }
    }

    void CalculateMeshOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if(!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if(newOutlineVertex != NO_VERTEX_FOUND)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);

                }
            }
        }
    }

    void FollowOutline(int vertexIndex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

        if(nextVertexIndex != NO_VERTEX_FOUND)
        {
            FollowOutline(nextVertexIndex, outlineIndex);
        }
    }


    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = triangleDict[vertexIndex];

        foreach (Triangle triangle in trianglesContainingVertex)
        {
            foreach (int vertex in triangle.vertices)
            {
                if (vertex != vertexIndex && !checkedVertices.Contains(vertex))
                {
                    if (IsOutlineEdge(vertexIndex, vertex))
                    {
                        return vertex;
                    }
                }
            }
        }
        return NO_VERTEX_FOUND;
    }

    bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<Triangle> trianglesContainingVertexA = triangleDict[vertexA];
        int sharedTriangleCount = 0;

        foreach (Triangle triangle in trianglesContainingVertexA)
        {
            if (triangle.Contains(vertexB))
            {
                sharedTriangleCount++;
                if(sharedTriangleCount > 1)
                {
                    return false;
                }
            }
        }
        return sharedTriangleCount == 1;
    }


    struct Triangle
    {
        public int[] vertices; 

        public Triangle (int a, int b, int c)
        {
            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }

        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertices[0] || vertexIndex == vertices[1] || vertexIndex == vertices[2];
        }
    }
    

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWith = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWith / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode(pos, map[x,y] == MapGeneration.WALL, squareSize);
                }         
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }

        }
    }

    public class Square
    {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centerTop, centerRight, centerBottom, centerLeft;
        public int configuration;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft )
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomLeft = _bottomLeft;
            bottomRight = _bottomRight;

            centerTop = topLeft.right;
            centerRight = bottomRight.above;
            centerBottom = bottomLeft.right;
            centerLeft = bottomLeft.above;

            if (topLeft.active)
                configuration += 8;
            if (topRight.active)
                configuration += 4;
            if (bottomRight.active)
                configuration += 2;
            if (bottomLeft.active)
                configuration += 1;
        }
    }


    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node( Vector3 _pos)
        {
            position = _pos;
        }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize): base(_pos)
        {
            active = _active;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }
    }


}
