using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
        public Node(int x, int y, Vector3 position, List<Module> modules)
        {
                this.x = x;
                this.y = y;
                this.position = position;
                neighbours = new List<Node>();
                collapsed = false;
                Entropy = 4;
                PotentialModules = new List<Module>();
                foreach (var item in modules)
                {
                        PotentialModules.Add(item);
                        allModules.Add(item);
                }
                Vector3 TopRightNode = new Vector3(position.x - 10 / 4, position.y, position.z + 10 / 4);
                DebugModules.Add(TopRightNode, null);

                Vector3 TopLeftNode = new Vector3(position.x + 10 / 4, position.y, position.z + 10 / 4);
                DebugModules.Add(TopLeftNode, null);

                Vector3 BotRightNode = new Vector3(position.x + 10 / 4, position.y, position.z - 10 / 4);
                DebugModules.Add(BotRightNode, null);

                Vector3 BotLefttNode = new Vector3(position.x - 10 / 4, position.y, position.z - 10 / 4);
                DebugModules.Add(BotLefttNode, null);
        }

        public override string ToString()
        {
                return $"node [{x},{y}]";
        }

        public int Entropy = 0;
        public bool collapsed;
        public List<Node> neighbours;
        public int x, y;
        public Vector3 position;

        public List<Module> PotentialModules = new List<Module>();
        public List<Module> allModules = new List<Module>();
        public Node Top;
        public Node Down;
        public Node Left;
        public Node Right;
        public Node Forward;
        public Node Back;
        public Module ModuleSelected;
        public Transform trans;
        public Dictionary<Vector3, GameObject> DebugModules = new Dictionary<Vector3, GameObject>();

        public List<Module> CheckConstaint(Node node)
        {
                List<Module> tmp = allModules;
                List<Module> tmp2 = new List<Module>();

                if (node.Left != null)
                {
                        foreach (var item in node.Left.PotentialModules)
                        {
                                tmp2 = tmp2.Concat(item.Right).ToList();
                        }
                        tmp = tmp.Intersect(tmp2).ToList();
                        tmp2.Clear();
                }
                if (node.Right != null)
                {
                        foreach (var item in node.Right.PotentialModules)
                        {
                                tmp2 = tmp2.Concat(item.Left).ToList();
                        }
                        tmp = tmp.Intersect(tmp2).ToList();
                        tmp2.Clear();
                }


                if (node.Forward != null)
                {
                        foreach (var item in node.Forward.PotentialModules)
                        {
                                tmp2 = tmp2.Concat(item.Back).ToList();
                        }
                        tmp = tmp.Intersect(tmp2).ToList();
                        tmp2.Clear();
                }

                if (node.Back != null)
                {
                        foreach (var item in node.Back.PotentialModules)
                        {
                                tmp2 = tmp2.Concat(item.Forward).ToList();
                        }
                        tmp = tmp.Intersect(tmp2).ToList();
                        tmp2.Clear();
                }
                if (tmp.Count == 0)
                {
                        Debug.Log($" we need to back up  ");
                }
                return tmp;
        }

        public void Propagate(Module module)
        {
                if (Forward != null && Forward.collapsed == false)
                {
                        Forward.PotentialModules = CheckConstaint(Forward);
                        Forward.Entropy = Forward.PotentialModules.Count;
                        if (Forward.PotentialModules.Count == 1)
                        {
                                Forward.Collapse();
                        }
                }
                if (Back != null && Back.collapsed == false)
                {
                        Back.PotentialModules = CheckConstaint(Back);
                        Back.Entropy = Back.PotentialModules.Count;
                        if (Back.PotentialModules.Count == 1)
                        {
                                Back.Collapse();
                        }
                }
                if (Right != null && Right.collapsed == false)
                {
                        Right.PotentialModules = CheckConstaint(Right);
                        Right.Entropy = Right.PotentialModules.Count;
                        if (Right.PotentialModules.Count == 1)
                        {
                                Right.Collapse();
                        }
                }
                if (Left != null && Left.collapsed == false)
                {
                        Left.PotentialModules = CheckConstaint(Left);
                        Left.Entropy = Left.PotentialModules.Count;
                        if (Left.PotentialModules.Count == 1)
                        {
                                Left.Collapse();
                        }
                }
        }

        public void Collapse()
        {
                Grid3D.Instance.CreateBackUp();
                PotentialModules = CheckConstaint(this);
                Module module = SelectModule();
                if (module == null) return;
                collapsed = true;
                Entropy = 1;
                PotentialModules.Clear();
                PotentialModules.Add(module);
                ModuleSelected = module;
                Propagate(module);

                ModuleSelected = GameObject.Instantiate(module, position, module.transform.rotation);
                Grid3D.Instance.UpdateModulesDebug();
        }

        public Module SelectModule()
        {
                //int startwith = PotentialModules.Count;

                //if (Right != null && Right.ModuleSelected != null)
                //{
                //        if (Right.collapsed == true)
                //        {
                //                PotentialModules = PotentialModules.Intersect(Right.ModuleSelected.Left).ToList();
                //        }
                //        //Debug.Log($"in the Right.Left ({Right.ModuleSelected?.name}) count {Right.ModuleSelected?.Left.Count} PotentialModules count {PotentialModules.Count}");
                //}
                //if (Left != null && Left.ModuleSelected != null)
                //{
                //        if (Left.collapsed == true)
                //        {
                //                PotentialModules = PotentialModules.Intersect(Left.ModuleSelected.Right).ToList();
                //        }
                //        //Debug.Log($"in the Left.Right  ({Left.ModuleSelected?.name}) count {Left.ModuleSelected?.Right.Count} PotentialModules count {PotentialModules.Count}");
                //}
                //Debug.Log($"module intersection left + right = {PotentialModules.Count}");

                //if (Back != null && Back.ModuleSelected != null)
                //{
                //        if (Back.collapsed == true)
                //        {
                //                PotentialModules = PotentialModules.Intersect(Back.ModuleSelected.Forward).ToList();
                //        }
                //}
                //if (Forward != null && Forward.ModuleSelected != null)
                //{
                //        if (Forward.collapsed == true)
                //        {
                //                PotentialModules = PotentialModules.Intersect(Forward.ModuleSelected.Back).ToList();
                //        }
                //}
                //if (PotentialModules.Count == 0)
                //{
                //        // Back to previous BackUp
                //        Debug.Log($" restore last backup  satrt with {startwith} mod, but now 0");
                //        Grid3D.Instance.RestoreLastBackUp();
                //        return null;
                //}

                //if (Right != null && Right.ModuleSelected?.Left != null)
                return PotentialModules[UnityEngine.Random.Range(0, PotentialModules.Count)];
        }

        public void RestorTo(Node backUpNode)
        {
                if (collapsed != backUpNode.collapsed)
                {
                        Debug.Log($"different collapse");
                        Entropy = backUpNode.Entropy;
                        collapsed = backUpNode.collapsed;
                        PotentialModules = backUpNode.PotentialModules;
                        GameObject.DestroyImmediate(ModuleSelected);
                        ModuleSelected = backUpNode.ModuleSelected;
                }
        }
}

public class Grid3D : MonoBehaviour
{
        public GameObject model;
        public static Grid3D Instance;
        public int CellSize = 10;
        public bool once = true;
        public List<Module> Modules = new List<Module>();

        // col and row == 4
        public int Dimention = 4;
        public Node[,] nodes;
        public List<Node> neighbours = new List<Node>();
        public Node ModuleSelected;

        public Node[,] previousGridLayout;

        private void Awake()
        {
                if (Instance == null)
                {
                        Instance = this;

                        nodes = new Node[Dimention, Dimention];
                        Generate3DMatrix();
                        foreach (var node in nodes)
                        {
                                node.trans = new GameObject().transform;
                                node.trans.position = node.position;
                                node.trans.name = $"[{node.x} {node.y}]";
                        }
                        GenerateModelsToDebug();
                        WaveFuntion();
                }
        }

        private void GenerateModelsToDebug()
        {
                foreach (var node in nodes)
                {
                        foreach (Module module in Modules)
                        {
                                var emptySpot = node.DebugModules.FirstOrDefault(el => el.Value == null);

                                GameObject obj = GameObject.Instantiate(module, emptySpot.Key, module.gameObject.transform.rotation).gameObject;
                                obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                obj.transform.SetParent(node.trans);
                                node.DebugModules[emptySpot.Key] = obj;
                        }
                }
        }

        public void UpdateModulesDebug()
        {
                foreach (Node node in nodes)
                {
                        if (node.collapsed)
                        {
                                foreach (Transform item in node.trans)
                                {
                                        GameObject.Destroy(item.gameObject);
                                }
                        }
                        else
                        {
                                foreach (Transform item in node.trans)
                                {
                                        GameObject.Destroy(item.gameObject);
                                }

                                node.DebugModules.Keys.ToList().ForEach(x => node.DebugModules[x] = null);

                                foreach (var module in node.PotentialModules)
                                {
                                        var emptySpot = node.DebugModules.FirstOrDefault(el => el.Value == null);

                                        GameObject obj = GameObject.Instantiate(module, emptySpot.Key, module.gameObject.transform.rotation).gameObject;
                                        obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                                        obj.transform.SetParent(node.trans);
                                        node.DebugModules[emptySpot.Key] = obj;
                                }
                        }
                }
        }

        public void Generate3DMatrix()
        {
                nodes = new Node[Dimention, Dimention];

                Vector3 botLeft = new Vector3(transform.position.x - Dimention * CellSize / 2, transform.position.y - Dimention * CellSize / 2, transform.position.z - Dimention * CellSize / 2) + new Vector3(5, 5, 5);

                Vector3 cell;
                for (int y = 0; y < Dimention; y++) // forward
                {
                        for (int x = 0; x < Dimention; x++) // horizental
                        {
                                cell = botLeft + new Vector3(RoundFloat(x * CellSize, 2), botLeft.y, RoundFloat(y * CellSize, 2));
                                nodes[x, y] = new Node(x, y, cell, Modules);
                        }
                }

                // create a graph
                //for (int up = 0; up < Dimention; up++)
                //{
                for (int y = 0; y < Dimention; y++)
                {
                        for (int x = 0; x < Dimention; x++)
                        {
                                Node currentNode = nodes[x, y];
                                if (x > 0)
                                {
                                        currentNode.neighbours.Add(nodes[x - 1, y]);
                                        currentNode.Left = nodes[x - 1, y];
                                }
                                //X is not mapSizeX - 1, then we can add right (x + 1)
                                if (x < Dimention - 1)
                                {
                                        currentNode.neighbours.Add(nodes[x + 1, y]);
                                        currentNode.Right = nodes[x + 1, y];
                                }
                                //Y is not 0, then we can add downwards (y - 1 )
                                if (y > 0)
                                {
                                        currentNode.neighbours.Add(nodes[x, y - 1]);
                                        currentNode.Back = nodes[x, y - 1];
                                }
                                //Y is not mapSizeY -1, then we can add upwards (y + 1)
                                if (y < Dimention - 1)
                                {
                                        currentNode.neighbours.Add(nodes[x, y + 1]);
                                        currentNode.Forward = nodes[x, y + 1];
                                }
                                //if (up > 0)
                                //{
                                //        currentNode.neighbours.Add(grid[x, y - 1]);
                                //        currentNode.Down = grid[x, y - 1];
                                //}
                                //if (up < Dimention - 1)
                                //{
                                //        currentNode.neighbours.Add(grid[x, y + 1]);
                                //        currentNode.Top = grid[x, y + 1];
                                //}
                        }
                }
                //}

                if (transform.childCount == 0)
                {
                        foreach (var celll in nodes)
                        {
                                // remove this line when finish 2d debugging
                                Instantiate(model, celll.position, Quaternion.identity, GameObject.Find("Debug").transform);
                        }
                }
        }

        // Start is called before the first frame update
        private void Start()
        {
        }

        private void WaveFuntion()
        {
                nodes[0, 0].Collapse();
                //grid[2, 2].Collapse();
                //grid[3, 3].Collapse();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private void OnDrawGizmos()
        {
                return;
                Vector3 botLeft = new Vector3(transform.position.x - Dimention * CellSize / 2, transform.position.y - Dimention * CellSize / 2, transform.position.z - Dimention * CellSize / 2) + new Vector3(5, 5, 5);
                Gizmos.DrawLine(botLeft, botLeft + Vector3.up * 5);
                //neighbours = new List<Node>();
                if (nodes == null || nodes.Length == 0)
                {
                        nodes = new Node[Dimention, Dimention];
                        Generate3DMatrix();
                        ModuleSelected = nodes[0, 0];
                }

                for (int i = 0; i < Dimention; i++)
                {
                        for (int j = 0; j < Dimention; j++)
                        {
                                Node current = nodes[i, j];
                                Gizmos.color = Color.yellow;
                                int nbofmodules = current.PotentialModules.Count;
                                Gizmos.DrawSphere(current.position, 2f);

                                if (nbofmodules != 0)
                                {
                                        for (int k = 0; k < nbofmodules; k++)
                                        {
                                                Gizmos.color = Color.yellow;

                                                Vector3 module;

                                                Gizmos.color = Color.green;
                                                Vector3 TopRightNode = new Vector3(current.position.x - CellSize / 4, current.position.y, current.position.z + CellSize / 4);
                                                Gizmos.DrawSphere(TopRightNode, 0.4f);

                                                Gizmos.color = Color.red;
                                                Vector3 TopLeftNode = new Vector3(current.position.x + CellSize / 4, current.position.y, current.position.z + CellSize / 4);
                                                Gizmos.DrawSphere(TopLeftNode, 0.4f);

                                                Gizmos.color = Color.blue;
                                                Vector3 BotRightNode = new Vector3(current.position.x + CellSize / 4, current.position.y, current.position.z - CellSize / 4);
                                                Gizmos.DrawSphere(BotRightNode, 0.4f);

                                                Gizmos.color = Color.black;
                                                Vector3 BotLefttNode = new Vector3(current.position.x - CellSize / 4, current.position.y, current.position.z - CellSize / 4);
                                                Gizmos.DrawSphere(BotLefttNode, 0.4f);
                                        }
                                }
                        }
                }
                if (ModuleSelected != null)
                {
                        if (ModuleSelected.Forward != null)
                        {
                                Gizmos.color = Color.green;
                                Gizmos.DrawSphere(ModuleSelected.Forward.position, 2);
                        }
                        if (ModuleSelected.Back != null)
                        {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(ModuleSelected.Back.position, 2);
                        }
                        if (ModuleSelected.Right != null)
                        {
                                Gizmos.color = Color.black;
                                Gizmos.DrawSphere(ModuleSelected.Right.position, 2);
                        }
                        if (ModuleSelected.Left != null)
                        {
                                Gizmos.color = Color.cyan;
                                Gizmos.DrawSphere(ModuleSelected.Left.position, 2);
                        }
                }

                Gizmos.color = Color.green;
        }

        protected float RoundFloat(float value, int nb)
        {
                float power = Mathf.Pow(10, nb);
                return Mathf.Round(value * power) * (1 / power);
        }

        public void CreateBackUp()
        {
                previousGridLayout = new Node[Dimention, Dimention];
                foreach (var item in nodes)
                {
                        Node node = nodes[item.x, item.y];

                        previousGridLayout[item.x, item.y] = new Node(node.x, node.y, node.position, node.PotentialModules);

                        previousGridLayout[item.x, item.y].neighbours = node.neighbours;
                        previousGridLayout[item.x, item.y].Back = node.Back;
                        previousGridLayout[item.x, item.y].Left = node.Left;
                        previousGridLayout[item.x, item.y].Right = node.Right;
                        previousGridLayout[item.x, item.y].Top = node.Top;
                        previousGridLayout[item.x, item.y].Down = node.Down;
                        previousGridLayout[item.x, item.y].Forward = node.Forward;

                        previousGridLayout[item.x, item.y].ModuleSelected = node.ModuleSelected;
                        previousGridLayout[item.x, item.y].collapsed = node.collapsed;
                        previousGridLayout[item.x, item.y].Entropy = node.Entropy;
                }
        }

        public void RestoreLastBackUp()
        {
                foreach (var node in nodes)
                {
                        Node backUpNode = previousGridLayout[node.x, node.y];

                        node.RestorTo(backUpNode);
                }
        }
}