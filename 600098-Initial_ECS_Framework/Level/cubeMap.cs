using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Components;
using OpenTK;

namespace OpenGL_Game.Level
{
    class cubeMap
    {
        Tuple<int, int> mapSize;
        bool[,] cubes;        
        public List<ComponentCollisionLine> collisionMesh = new List<ComponentCollisionLine>();
        string file;
        public cubeMap(int x, int y, string filename)
        {
            file = filename;
            mapSize = new Tuple<int,int>(x,y);
            cubes = new bool[mapSize.Item1, mapSize.Item2];
            cubes = LoadMap();
            collisionMesh = LoadCubeMapCollisionMesh();
        }

        public Tuple<int,int> MapSize
        {
            get { return mapSize; }
        }



        bool[,] LoadMap()
        {
            bool[,] cubeMap;
            cubeMap = new bool[mapSize.Item2, mapSize.Item1];
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    for (int y = 0; y < mapSize.Item2; y++)
                    {
                        string line = sr.ReadLine();
                        for (int x = 0; x < mapSize.Item1; x++)
                        {
                            cubeMap[y, x] = int.Parse(line[x].ToString()) != 0;
                        }
                    }
                }
            }
            return cubeMap;
        }
        List<ComponentCollisionLine> LoadCubeMapCollisionMesh()
        {
            List<ComponentCollisionLine> collisions = new List<ComponentCollisionLine>();
            for(int y = 0; y < cubes.GetLength(0); y++)
            {
                for(int x = 0; x < cubes.GetLength(1); x++)
                {
                    Vector2 blockOrigin = new Vector2((x - (mapSize.Item1 / 2)), (y - (mapSize.Item2 / 2)));
                    blockOrigin = Vector2.Multiply(blockOrigin, -1);
                    if (cubes[y , x])
                    {
                        if (x > 0)
                        {
                            if (!cubes[y, x - 1])
                            {
                                collisions.Add(new ComponentCollisionLine(blockOrigin, new Vector2(blockOrigin.X, blockOrigin.Y - 1), new Vector2(1, 0)));
                            }
                        }
                        if (x < 19)
                        {
                            if (!cubes[y, x + 1])
                            {
                                collisions.Add(new ComponentCollisionLine(new Vector2(blockOrigin.X - 1, blockOrigin.Y), new Vector2(blockOrigin.X - 1, blockOrigin.Y - 1), new Vector2(-1, 0)));
                            }
                        }
                        if (y > 0)
                        {
                            if (!cubes[y - 1, x])
                            {
                                collisions.Add(new ComponentCollisionLine(blockOrigin, new Vector2(blockOrigin.X - 1, blockOrigin.Y), new Vector2(0, 1)));
                            }
                        }
                        if (y < 19)
                        {
                            if (!cubes[y + 1, x])
                            {
                                collisions.Add(new ComponentCollisionLine(new Vector2(blockOrigin.X, blockOrigin.Y - 1), new Vector2(blockOrigin.X - 1, blockOrigin.Y - 1), new Vector2(0, -1)));
                            }
                        }
                    }
                    
                }
            }
            return collisions;
        }

    }
}
