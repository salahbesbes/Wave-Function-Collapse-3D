using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class WaveFunctionCollapse : MonoBehaviour
{

        public Transform collection;
        public Transform point;
        public List<Tile> tiles = new List<Tile>();
        public Cell[,] grid;
        public int Dimention = 3;
        public int CellSize = 1;

        private void Start()
        {

        }

        private void PlaceAllPossibleCells()
        {
                foreach (Cell node in grid)
                {
                        foreach (Tile module in node.PotentialTiles)
                        {
                                var emptySpot = node.DebugModules.FirstOrDefault(el => el.Value == null);
                                GameObject obj = GameObject.Instantiate(module, emptySpot.Key, module.gameObject.transform.rotation).gameObject;
                                obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                obj.transform.SetParent(node.trans);
                                node.DebugModules[emptySpot.Key] = obj;
                        }
                }
        }

        private void Update()
        {
        }
        [ExecuteInEditMode]
        private void OnGUI()
        {
                if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
                {
                        int count = 1;
                        tiles.Clear();
                        foreach (Transform child in collection)
                        {
                                Tile tile = child.GetComponent<Tile>();


                                tile.Init();
                                tile.CreateFourSideColor();
                                tile.DestroyClones();
                                tile.Rotate90();
                                tile.name = tile.name.Substring(0, tile.name.Length - 2);
                                tile.transform.name += $"_{count}";
                                tiles.Add(tile);
                                count++;
                                foreach (Tile clone in tile.Clones)
                                {
                                        clone.transform.name += $"_{count}";
                                        count++;
                                        tiles.Add(clone);
                                }

                        }


                }

                if (GUI.Button(new Rect(700, 70, 70, 30), "Analyze"))
                {

                        bool foundOne = CheckForAnyConnectionBetweenTiles(tiles[12], tiles[5]);
                        if (foundOne == false)
                        {
                                Debug.Log($" didnt found any connection");
                        }

                }
                if (GUI.Button(new Rect(700, 150, 70, 30), "Generate"))
                {

                        Generate3DMatrix();
                        foreach (Cell node in grid)
                        {
                                node.trans = new GameObject().transform;
                                node.trans.position = node.position;
                                node.trans.name = $"[{node.x} {node.y}]";
                                node.trans.SetParent(transform);
                        }
                        PlaceAllPossibleCells();

                }

        }

        private bool CheckForAnyConnectionBetweenTiles(Tile tile1, Tile tile2)
        {
                Face[] tile1VerticalFaces = new Face[2] { tile1.BackFace, tile1.ForwardFace };
                Face[] tile2VerticalFaces = new Face[2] { tile2.BackFace, tile2.ForwardFace };

                Face[] tile1HorizentalFaces = new Face[2] { tile1.LeftFace, tile1.RightFace };
                Face[] tile2HorizentalFaces = new Face[2] { tile2.LeftFace, tile2.RightFace };

                bool foundOne = false;
                foreach (Face faceT1 in tile1HorizentalFaces)
                {
                        foreach (Face faeT2 in tile2HorizentalFaces)
                        {
                                if (faceT1.CanConnect(faeT2))
                                {
                                        foundOne = true;
                                        Debug.Log($" connection between  piece [{tile1.name.Substring(tile1.name.Length - 2)}] ({faceT1.direction} face)  and  piece [{tile2.name.Substring(tile2.name.Length - 2)}] ({faeT2.direction} face)");
                                }

                        }
                }


                foreach (Face faceT1 in tile1VerticalFaces)
                {
                        foreach (Face faeT2 in tile2VerticalFaces)
                        {
                                if (faceT1.CanConnect(faeT2))
                                {
                                        foundOne = true;
                                        Debug.Log($" connection between  piece [{tile1.name.Substring(tile1.name.Length - 2)}] ({faceT1.direction} face)  and  piece [{tile2.name.Substring(tile2.name.Length - 2)}] ({faeT2.direction} face)");
                                }

                        }
                }

                return foundOne;

        }
        protected float RoundFloat(float value, int nb)
        {
                float power = Mathf.Pow(10, nb);
                return Mathf.Round(value * power) * (1 / power);
        }

        public void Generate3DMatrix()
        {
                grid = new Cell[Dimention, Dimention];

                Vector3 botLeft = new Vector3(transform.position.x - Dimention * CellSize / 2, transform.position.y, transform.position.z - Dimention * CellSize / 2);

                Vector3 cell;
                for (int y = 0; y < Dimention; y++) // forward
                {
                        for (int x = 0; x < Dimention; x++) // horizental
                        {
                                cell = botLeft + new Vector3(RoundFloat(x * CellSize, 2), botLeft.y, RoundFloat(y * CellSize, 2));
                                grid[x, y] = new Cell(x, y, tiles, cell);

                                Cell currentNode = grid[x, y];
                                if (x > 0)
                                {
                                        currentNode.neighbours.Add(grid[x - 1, y]);
                                        //currentNode.Left = grid[x - 1, y];
                                }
                                //X is not mapSizeX - 1, then we can add right (x + 1)
                                if (x < Dimention - 1)
                                {
                                        currentNode.neighbours.Add(grid[x + 1, y]);
                                        //currentNode.Right = grid[x + 1, y];
                                }
                                //Y is not 0, then we can add downwards (y - 1 )
                                if (y > 0)
                                {
                                        currentNode.neighbours.Add(grid[x, y - 1]);
                                        //currentNode.Back = grid[x, y - 1];
                                }
                                //Y is not mapSizeY -1, then we can add upwards (y + 1)
                                if (y < Dimention - 1)
                                {
                                        currentNode.neighbours.Add(grid[x, y + 1]);
                                        //currentNode.Forward = grid[x, y + 1];
                                }
                        }
                }
        }

        private void OnDrawGizmos()
        {
                grid = new Cell[Dimention, Dimention];

                Vector3 botLeft = new Vector3(transform.position.x - Dimention * CellSize / 2, transform.position.y + 0.5f, transform.position.z - Dimention * CellSize / 2);

                Vector3 cell;
                for (int y = 0; y < Dimention; y++) // forward
                {
                        for (int x = 0; x < Dimention; x++) // horizental
                        {
                                cell = botLeft + new Vector3(RoundFloat(x * CellSize, 2), botLeft.y, RoundFloat(y * CellSize, 2));
                                //grid[x, y] = new Cell(x, y, tiles, cell);
                                //Gizmos.color = Color.gray;
                                //Gizmos.DrawSphere(cell, 0.1f);
                                //foreach (var dic in grid[x, y].DebugModules)
                                //{
                                //        Gizmos.color = Color.red;
                                //        Gizmos.DrawSphere(dic.Key, 0.1f);

                                //}


                        }
                }
        }
}



public class Cell
{
        public int Entropy = 0;
        public bool collapsed = false;
        public List<Cell> neighbours;
        public int x, y;
        public Vector3 position;

        public List<Tile> PotentialTiles = new List<Tile>();
        public Tile ModuleSelected;
        public Transform trans;
        public Dictionary<Vector3, GameObject> DebugModules = new Dictionary<Vector3, GameObject>();

        public Cell(int x, int y, List<Tile> alltiles, Vector3 position)
        {
                PotentialTiles = alltiles;
                this.x = x;
                this.y = y;
                this.position = position;
                neighbours = new List<Cell>();
                Vector3 topLeft = position + (Vector3.left + Vector3.forward) * 0.33f;
                Vector3 topRight = position + (Vector3.right + Vector3.forward) * 0.33f;
                float dist = (topLeft - topRight).magnitude / 3;
                DebugModules.Add(position + (Vector3.left + Vector3.forward) * 0.33f, null);
                DebugModules.Add(topLeft + Vector3.right * dist, null);
                DebugModules.Add(topRight + Vector3.left * dist, null);
                DebugModules.Add(position + (Vector3.right + Vector3.forward) * 0.33f, null);


                Vector3 midLeft = position + (Vector3.left) * 0.33f;
                Vector3 midRight = position + (Vector3.right) * 0.33f;


                DebugModules.Add(midRight + (Vector3.forward) * 0.09f, null);
                DebugModules.Add(midRight + (Vector3.forward * 0.09f + Vector3.left * dist), null);
                DebugModules.Add(midLeft + (Vector3.forward) * 0.09f, null);
                DebugModules.Add(midLeft + (Vector3.forward * 0.09f + Vector3.right * dist), null);


                DebugModules.Add(midRight + (Vector3.back) * 0.09f, null);
                DebugModules.Add(midRight + (Vector3.left * dist + Vector3.back * 0.09f), null);
                DebugModules.Add(midLeft + (Vector3.back) * 0.09f, null);
                DebugModules.Add(midLeft + (Vector3.right * dist + Vector3.back * 0.09f), null);


                Vector3 botLeft = position + (Vector3.left + Vector3.back) * 0.33f;
                Vector3 botRight = position + (Vector3.right + Vector3.back) * 0.33f;

                DebugModules.Add(position + (Vector3.left + Vector3.back) * 0.33f, null);
                DebugModules.Add(botLeft + Vector3.right * dist, null);
                DebugModules.Add(botRight + Vector3.left * dist, null);
                DebugModules.Add(position + (Vector3.right + Vector3.back) * 0.33f, null);

        }

        public override string ToString()
        {
                return $"cell [{x},{y}]";
        }
}