using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 *  This class is in charge of creating a map that looks 
 *  like a cave.
 * 
 * 
 */ 
public class MapGeneration : MonoBehaviour
{
    public const int WALL = 1;
    public const int EMPTY = 0;
    public const int NB_SMOOTHING = 5;
    public const int NB_TO_AGGREGATE = 4;

    public const int BORDER_SIZE = 20;

    public const int LOOKED = 1;
    public const int WALLTHRESHOLD = 50;
    public const int ROOMTHRESHOLD = 50;

    public const int PASSAGEWAY_WIDTH = 2;


    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0,100)]
    public int randomFillPercent;

    private int[,] map;

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            GenerateMap();
        }
    }

    // Create a map with 1 for walls and 0 for path.
    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < NB_SMOOTHING; i++)
        {
            SmoothMap();
        }


        // Supress too small walls and too small rooms
        ProcessMap();


        // Adding thick borders to the map
        int[,] borderedMap = new int[width + BORDER_SIZE * 2, height + BORDER_SIZE * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                if (x >= BORDER_SIZE && x < width + BORDER_SIZE && y >= BORDER_SIZE && y < height + BORDER_SIZE)
                {
                    borderedMap[x, y] = map[x - BORDER_SIZE, y - BORDER_SIZE];
                }
                else
                {
                    borderedMap[x, y] = WALL;
                }
            }
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderedMap, 1);

    }


    void ProcessMap()
    {
        // Remove small walls
        List<List<Coord>> wallRegions = GetRegions(WALL);

        foreach( List<Coord> wallRegion in wallRegions)
        {
            if( wallRegion.Count < WALLTHRESHOLD)
            {
                foreach(Coord tile in wallRegion)
                {
                    map[tile.x, tile.y] = EMPTY;
                }
            }
        }
         // Remove small rooms
        List<List<Coord>> roomRegions = GetRegions(EMPTY);
        List<Room> survivingRooms = new List<Room>();
        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < ROOMTHRESHOLD)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.x, tile.y] = WALL;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, map, this));
            }
        }
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;
        ConnectClosestRooms(survivingRooms);
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if(forceAccessibilityFromMainRoom)
        {
            foreach(Room room in allRooms)
            {
                if(room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int minDistance = int.MaxValue;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room(this);
        Room bestRoomB = new Room(this);

        foreach(Room roomA in roomListA)
        {
            if(!forceAccessibilityFromMainRoom)
            {
                minDistance = int.MaxValue;
                if(roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            
            foreach(Room roomB in roomListB)
            {
                if(roomA == roomB || roomA.isConnected(roomB))
                {
                    continue;
                }
                for (int tileIndexA = 0; tileIndexA < roomA.edgesTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgesTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgesTiles[tileIndexA];
                        Coord tileB = roomB.edgesTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.x - tileB.x,  2) + Mathf.Pow(tileA.y - tileB.y, 2));

                        if(distanceBetweenRooms < minDistance)
                        {
                            minDistance = distanceBetweenRooms;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
             if(minDistance < int.MaxValue && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }
        if (minDistance < int.MaxValue && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);

        List<Coord> line = GetLine(tileA, tileB);
        foreach(Coord c in line)
        {
            DrawCircle(c, PASSAGEWAY_WIDTH);
        }

    }

    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if(x*x + y*y <= r*r)
                {
                    int drawX = c.x + x;
                    int drawY = c.y + y;
                    if(isInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = EMPTY;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.x;
        int y = from.y;
        int dx = to.x - from.x;
        int dy = to.y - from.y;

        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Math.Abs(dx);
        int shortest = Math.Abs(dy);

        bool inverted = (longest < shortest);
        if(inverted)
        {
            longest = Math.Abs(dy);
            shortest = Math.Abs(dx);
            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));
            if(inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }
            gradientAccumulation += shortest;
            if(gradientAccumulation >= longest)
            {
                if(inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    Vector3 CoordToWorldPoint(Coord coord)
    {
        return new Vector3(-width / 2 + 0.5f + coord.x, 2, -height / 2 + 0.5f + coord.y);
    }

    // Get all regions of a given type
    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x,y] != LOOKED && map[x,y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);

                    regions.Add(newRegion);

                    foreach (Coord coord in newRegion)
                    {
                        mapFlags[coord.x, coord.y] = LOOKED;
                    }
                }
               
            }
        }
        return regions;
    }

    // Get the connected tiles of the same type
    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = LOOKED;

        while(queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.x - 1; x <= tile.x + 1; x++)
            {
                for (int y = tile.y - 1; y <= tile.y + 1; y++)
                {
                    if(isInMapRange(x, y) && (x == tile.x || y == tile.y))
                    {
                        if(mapFlags[x,y] != LOOKED && map[x,y] == tileType)
                        {
                            mapFlags[x, y] = LOOKED;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    bool isInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    // Randomly (with a pre-set seeed or not) fill the map with 0 and walls;
    // Cases on the border of the map are walls.
    void RandomFillMap()
    {
        if(useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = WALL;
                }
                map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : EMPTY; ;
            }
        }
    }

    // Take each point of the map and made them more like their neighbours.
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if(neighbourWallTiles > NB_TO_AGGREGATE)
                {
                    map[x, y] = WALL;
                }
                else if(neighbourWallTiles < NB_TO_AGGREGATE)
                {
                    map[x, y] = EMPTY;
                }
                // if == NB_TO_AGGREGATE : then nothing is done and the previous value is left.
            }
        }
    }

    // Count the surrounding walls for a given  point in the map.
    // Count all of the neighbours of a border case as walls
    // to encourage the creation of walls in the borders of the map.
    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        for (int x = gridX - 1; x <= gridX + 1 ; x++)
        {
            for (int y = gridY - 1; y <= gridY + 1; y++)
            {
                if (isInMapRange(x,y))
                {
                    if (x != gridX || y != gridY)
                    {
                        wallCount += map[x, y];
                    }
                }
                else
                {
                    // Encourage the building of walls around the map
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    struct Coord
    {
        public int x;
        public int y;

        public Coord(int tileX, int tileY)
        {
            x = tileX;
            y = tileY;
        }
    }

    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgesTiles;
        public List<Room> connectedRooms;

        public int roomSize;
        public bool isMainRoom;
        public bool isAccessibleFromMainRoom;

        private MapGeneration outerClass;

        public Room(MapGeneration outer)
        {
            outerClass = outer;
        }

        public Room(List<Coord> roomTiles, int[,] map, MapGeneration outer)
        {
            outerClass = outer;
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();
            edgesTiles = new List<Coord>();

            // Looking for neighboouring walls to determine if it's an edge tile.
            foreach( Coord tile in tiles)
            {
                for (int x = tile.x - 1; x <= tile.x + 1; x++)
                {
                    for (int y = tile.y - 1; y <= tile.y + 1; y++)
                    {
                        if( (x == tile.x || y == tile.y) && outerClass.isInMapRange(x, y) && map[x,y] == WALL)
                        {
                            edgesTiles.Add(tile);
                        }
                    }
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if(roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool isConnected(Room room)
        {
            return connectedRooms.Contains(room);
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }

        public void SetAccessibleFromMainRoom()
        {
            if(!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach(Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }
    }
}
