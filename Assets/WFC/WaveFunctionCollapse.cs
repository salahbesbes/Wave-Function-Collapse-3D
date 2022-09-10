using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class WaveFunctionCollapse : MonoBehaviour
{
        public Tile tiles1;
        public Tile tiles2;
        public Tile EmptySpace;
        public SerialisableGrid.Grid grid2d;
        public static WaveFunctionCollapse Instance;
        public Transform collection;
        public Transform point;
        public List<Tile> tiles = new List<Tile>();
        public Cell[,] grid;
        public int Dimention = 3;
        public int CellSize = 1;
        public Clickable cellPrefab;
        private void Start()
        {
                if (Instance == null)
                { Instance = this; }

                //foreach (Transform child in transform)
                //{
                //        DestroyImmediate(child.gameObject);
                //}

                //Generate3DMatrix();
                //foreach (Cell cell in grid)
                //{
                //        Clickable click = Instantiate(cellPrefab);
                //        cell.trans = click.transform;
                //        cell.trans.position = cell.position;
                //        cell.trans.name = $"[{cell.x} {cell.y}]";
                //        cell.trans.SetParent(transform);
                //        Debug.Log($"{cell}");
                //        click.cell = cell;
                //}
                //PlaceAllPossibleCells();

                DestroyImmediate(point.gameObject);
                point = null;
                point = new GameObject("parent").transform;
                point.SetParent(transform);

                Generate3DMatrix();
                foreach (Cell cell in grid)
                {
                        Clickable click = Instantiate(cellPrefab, point);
                        cell.trans = click.transform;
                        cell.trans.position = cell.position;
                        cell.trans.name = $"[{cell.x} {cell.y}]";
                        cell.trans.SetParent(click.transform);
                        click.cell = cell;

                }
                PlaceAllPossibleCells();
        }

        public void PlaceAllPossibleCells()
        {
                foreach (Cell node in grid)
                {
                        //node.RemoveConstrain();
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


        public bool CheckForAnyConnectionBetweenTiles(Tile tile1, Tile tile2, Direction directionToConnectTile2ToTile1)
        {
                if (tile1 == null)
                { Debug.Log($" tile 1 is null"); }
                if (tile2 == null)
                { Debug.Log($"tile 2 is null"); }
                if (directionToConnectTile2ToTile1 == Direction.Right)
                {
                        return tile1.RightFace.Equals(tile2.LeftFace);
                }
                if (directionToConnectTile2ToTile1 == Direction.Left)
                {
                        return tile1.LeftFace.Equals(tile2.RightFace);
                }
                if (directionToConnectTile2ToTile1 == Direction.Forward)
                {
                        return tile1.ForwardFace.Equals(tile2.BackFace);
                }
                if (directionToConnectTile2ToTile1 == Direction.Back)
                {
                        return tile1.BackFace.Equals(tile2.ForwardFace);
                }
                return false;

                //Face[] tile1VerticalFaces = new Face[2] { tile1.BackFace, tile1.ForwardFace };
                //Face[] tile2VerticalFaces = new Face[2] { tile2.BackFace, tile2.ForwardFace };

                //Face[] tile1HorizentalFaces = new Face[2] { tile1.LeftFace, tile1.RightFace };
                //Face[] tile2HorizentalFaces = new Face[2] { tile2.LeftFace, tile2.RightFace };

                //bool foundOne = false;
                //foreach (Face faceT1 in tile1HorizentalFaces)
                //{
                //        foreach (Face faeT2 in tile2HorizentalFaces)
                //        {
                //                if (faceT1.CanConnect(faeT2) && faeT2.direction == directionToConnectTile2ToTile1)
                //                {
                //                        return true;
                //                }

                //        }
                //}


                //foreach (Face faceT1 in tile1VerticalFaces)
                //{
                //        foreach (Face faeT2 in tile2VerticalFaces)
                //        {
                //                if (faceT1.CanConnect(faeT2))
                //                {
                //                        foundOne = true;
                //                        //Debug.Log($" connection between  piece [{tile1.name.Substring(tile1.name.Length - 2)}] ({faceT1.direction} face)  and  piece [{tile2.name.Substring(tile2.name.Length - 2)}] ({faeT2.direction} face)");
                //                }

                //        }
                //}

                //return foundOne;

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

                        }
                }



                for (int y = 0; y < Dimention; y++) // forward
                {
                        for (int x = 0; x < Dimention; x++) // horizental
                        {
                                if (x > 0)
                                {
                                        grid[x, y].Left = grid[x - 1, y];
                                }
                                //X is not mapSizeX - 1, then we can add right (x + 1)
                                if (x < Dimention - 1)
                                {
                                        grid[x, y].Right = grid[x + 1, y];
                                }
                                //Y is not 0, then we can add downwards (y - 1 )
                                if (y > 0)
                                {
                                        grid[x, y].Back = grid[x, y - 1];
                                }
                                //Y is not mapSizeY -1, then we can add upwards (y + 1)
                                if (y < Dimention - 1)
                                {
                                        grid[x, y].Forward = grid[x, y + 1];
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


[Serializable]
public class Cell
{
        public int Entropy = 0;
        public bool collapsed = false;
        public bool visisted = false;
        public Cell Right { get; set; }
        public Cell Left;
        public Cell Forward;
        public Cell Back;
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

        internal void Collapse()
        {
                Tile SelectedTile = SelectRandomTile();
                if (SelectedTile == null)
                {
                        Debug.Log($" selected tile is null ");
                        return;
                }

                foreach (Transform child in trans)
                {
                        GameObject.Destroy(child.gameObject);
                }
                //Debug.Log($" {this} has selected  \"{SelectedTile}\"");
                ModuleSelected = SelectedTile;
                collapsed = true;
                UpdatePotentialTiles();
                GameObject.Instantiate(SelectedTile.gameObject, trans.position, SelectedTile.transform.rotation);



        }


        public void UpdateNeighbour(Direction direction)
        {
                visisted = true;
                Cell Neghibour = null;
                if (direction == Direction.Left)
                {
                        Neghibour = Left;
                }
                if (direction == Direction.Right)
                {
                        Neghibour = Right;
                }
                if (direction == Direction.Forward)
                {
                        Neghibour = Forward;
                }
                if (direction == Direction.Back)
                {
                        Neghibour = Back;
                }
                if (Neghibour == null) return;
                if (Neghibour.collapsed == true) return;
                //if (Neghibour.visisted == true) return;

                //Debug.Log($"-------- check between {this} {Neghibour} {direction} -------------");
                List<Tile> tmp = new List<Tile>();
                foreach (Tile tile in Neghibour.PotentialTiles)
                {
                        if (ModuleSelected == null)
                        {
                                break;
                        }
                        else
                        {
                                bool foundAtLeastOne = WaveFunctionCollapse.Instance.CheckForAnyConnectionBetweenTiles(ModuleSelected, tile, direction);
                                if (foundAtLeastOne)
                                {
                                        tmp.Add(tile);
                                }
                        }
                        //if (ModuleSelected == null) Debug.Log($"{Neghibour} has  ModuleSelected is null");
                }
                if (tmp.Count > 0)
                {
                        foreach (Transform child in Neghibour.trans)
                        {
                                GameObject.Destroy(child.gameObject);
                        }
                        foreach (var key in Neghibour.DebugModules.Keys.ToList())
                                Neghibour.DebugModules[key] = null;


                        // remove tile constrains

                        Neghibour.PotentialTiles = tmp;
                        Neghibour.RemoveConstrain();


                        foreach (Tile tile in Neghibour.PotentialTiles)
                        {
                                var emptySpot = Neghibour.DebugModules.FirstOrDefault(el => el.Value == null);
                                GameObject obj = GameObject.Instantiate(tile, emptySpot.Key, tile.gameObject.transform.rotation).gameObject;
                                obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                obj.transform.SetParent(Neghibour.trans);
                                Neghibour.DebugModules[emptySpot.Key] = obj;
                        }
                        if (Neghibour.PotentialTiles.Count == 1)
                        {
                                Neghibour.Collapse();
                        }
                        else
                        {
                                Neghibour.UpdateNeighbour(Direction.Left);
                                Neghibour.UpdateNeighbour(Direction.Right);
                                Neghibour.UpdateNeighbour(Direction.Forward);
                                Neghibour.UpdateNeighbour(Direction.Back);
                        }
                }
        }

        public void RemoveConstrain()
        {
                List<Tile> CopyOfPotentialTiles = new List<Tile>(PotentialTiles);
                if (PotentialTiles.Count == 0) Debug.Log($"PotentialTiles is already empty ");
                List<Tile> constraints = new List<Tile>();
                Direction[] FourDirection = new Direction[4] { Direction.Left, Direction.Right, Direction.Back, Direction.Forward };
                foreach (Tile tile in CopyOfPotentialTiles)
                {
                        foreach (Direction direction in FourDirection)
                        {
                                if (direction == Direction.Left)
                                {
                                        if (Left == null)
                                        {
                                                constraints = tile.leftCantBe;
                                                PotentialTiles = PotentialTiles.Where(el => !el.leftCantBe.Contains(WaveFunctionCollapse.Instance.EmptySpace)).ToList();
                                        }
                                        else if (Left.collapsed)
                                        {
                                                constraints = Left.ModuleSelected.rightCantBe;
                                                PotentialTiles = PotentialTiles.Where(el => !constraints.Contains(el)).ToList();
                                        }
                                }
                                if (direction == Direction.Right)
                                {
                                        if (Right == null)
                                        {
                                                constraints = tile.rightCantBe;
                                                PotentialTiles = PotentialTiles.Where(el => !el.rightCantBe.Contains(WaveFunctionCollapse.Instance.EmptySpace)).ToList();

                                        }
                                        else if (Right.collapsed)
                                        {
                                                constraints = Right.ModuleSelected.leftCantBe;
                                                PotentialTiles = PotentialTiles.Where(el => !constraints.Contains(el)).ToList();
                                        }

                                }
                                if (direction == Direction.Forward)
                                {
                                        if (Forward == null)
                                        {
                                                constraints = tile.forwardCantBe;
                                                PotentialTiles = PotentialTiles.Where(el => !el.forwardCantBe.Contains(WaveFunctionCollapse.Instance.EmptySpace)).ToList();

                                        }
                                        else if (Forward.collapsed)
                                        {
                                                constraints = Forward.ModuleSelected.backCantBe;
                                                PotentialTiles = PotentialTiles.Where(el => !constraints.Contains(el)).ToList();
                                        }
                                }
                                if (direction == Direction.Back)
                                {
                                        if (Back == null)
                                        {
                                                constraints = tile.backCantBe;
                                                PotentialTiles = PotentialTiles.Where(el => !el.backCantBe.Contains(WaveFunctionCollapse.Instance.EmptySpace)).ToList();

                                        }
                                        else if (Back.collapsed)
                                        {
                                                constraints = Back.ModuleSelected.forwardCantBe;
                                        }
                                        PotentialTiles = PotentialTiles.Where(el => !constraints.Contains(el)).ToList();
                                }
                        }
                }



        }

        private void UpdatePotentialTiles()
        {
                if (ModuleSelected != null)
                {
                        UpdateNeighbour(Direction.Left);
                        UpdateNeighbour(Direction.Right);
                        UpdateNeighbour(Direction.Forward);
                        UpdateNeighbour(Direction.Back);
                }
        }
        private Tile SelectRandomTile()
        {

                RemoveConstrain();
                if (PotentialTiles.Count == 0)
                {
                        Debug.Log($" No Potential tiles found ");
                        return null;
                }
                int random = UnityEngine.Random.Range(0, PotentialTiles.Count);

                if (PotentialTiles.Contains(null) || PotentialTiles[random] == null)
                {
                        Debug.Log($" PotentialTiles contains null");
                }

                return PotentialTiles[random];
        }
}