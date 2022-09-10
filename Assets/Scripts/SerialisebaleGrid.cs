using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace SerialisableGrid
{


        [Serializable]
        public class Node
        {
                [SerializeField]
                private int i;
                [SerializeField]
                private int j;
                public int Value;

                public Node(int i, int j)
                {
                        this.i = i;
                        this.j = j;
                }



                //public override string ToString()
                //{
                //        return $"({i},{j})";
                //}

                public override string ToString()
                {
                        return $"({Value})";
                }

                public virtual Node clone()
                {
                        Node clone = new Node(i, j);
                        clone.Value = Value;
                        return clone;
                }
        }
        [Serializable]
        public class Grid
        {
                public int size;
                [SerializeField]
                private Node[,] BackUp;
                public Node test = new Node(0, 0);

                public void UpdateMainGrid(int i, int j, int Value)
                {
                        if (i >= size || j >= size)
                        {
                                Console.WriteLine(" index out of range ");
                                return;
                        }
                        Nodes[i, j].Value = Value;
                }

                public void LoadBackUp()
                {

                        for (int i = 0; i < size; i++)
                        {
                                for (int j = 0; j < size; j++)
                                {
                                        Nodes[i, j] = BackUp[i, j];
                                }
                        }

                }

                public void Save()
                {
                        BackUp = new Node[size, size];

                        for (int i = 0; i < size; i++)
                        {
                                for (int j = 0; j < size; j++)
                                {


                                        BackUp[i, j] = Nodes[i, j].clone();
                                }
                        }
                }

                public void PrintBackUp()
                {
                        Console.WriteLine("_________________________ Back Up ______________________");
                        for (int i = 0; i < size; i++)
                        {
                                string line = "";
                                string sep = ", ";
                                for (int j = 0; j < size; j++)
                                {
                                        if (j == size - 1) sep = "";

                                        Node node = BackUp[i, j];
                                        line += $"{node}{sep}";

                                }
                                Console.WriteLine(line);
                        }
                        Console.WriteLine("____________________________________________________");

                }

                public void Print()
                {
                        Console.WriteLine("____________________ Main Grid __________________________");


                        string completeGrid = "";
                        for (int i = 0; i < size; i++)
                        {
                                string line = "";
                                string sep = ", ";
                                for (int j = 0; j < size; j++)
                                {
                                        if (j == size - 1) sep = "";

                                        Node node = Nodes[i, j];
                                        line += $"{node}{sep}";

                                }
                                Console.WriteLine(line);
                                completeGrid += $"{line}\n";
                        }
                        Console.WriteLine("____________________________________________________");
                }


                public void Print(Node[,] nodes, int size)
                {
                        Console.WriteLine("____________________ Main Grid __________________________");


                        string completeGrid = "";
                        for (int i = 0; i < size; i++)
                        {
                                string line = "";
                                string sep = ", ";
                                for (int j = 0; j < size; j++)
                                {
                                        if (j == size - 1) sep = "";

                                        Node node = nodes[i, j];
                                        line += $"{node}{sep}";

                                }
                                Console.WriteLine(line);
                                completeGrid += $"{line}\n";
                        }
                        Console.WriteLine("____________________________________________________");
                }
                public override string ToString()
                {
                        string completeGrid = "";
                        for (int i = 0; i < size; i++)
                        {
                                string line = "";
                                string sep = ",";
                                for (int j = 0; j < size; j++)
                                {
                                        if (j == size - 1) sep = "";

                                        Node node = Nodes[i, j];
                                        line += $"{node.Value}{sep}";

                                }
                                completeGrid += $"{line}\n";
                        }
                        return completeGrid;
                }
                [SerializeField]
                Node[,] Nodes = new Node[1, 1];
                public Node this[int x, int y] => Nodes[x, y];
                public Grid(int size)
                {
                        this.size = size;
                        Nodes = new Node[size, size];
                        InitializeTheGrid();

                }

                public void InitializeTheGrid()
                {
                        for (int i = 0; i < size; i++)
                        {
                                for (int j = 0; j < size; j++)
                                {
                                        Nodes[i, j] = new Node(i, j);
                                        Nodes[i, j].Value = 0;
                                }
                        }
                }
                public void RotateMatrix(int val)
                {
                        Node[,] ret = new Node[size, size];



                        switch (val)
                        {
                                case 90:
                                        for (int i = 0; i < size; ++i)
                                        {
                                                for (int j = 0; j < size; ++j)
                                                {
                                                        //totate 90°
                                                        ret[i, j] = Nodes[size - j - 1, i];
                                                }
                                        }
                                        Nodes = ret;
                                        break;

                                case 180:
                                        for (int i = 0; i < size; ++i)
                                        {
                                                for (int j = 0; j < size; ++j)
                                                {
                                                        //totate 180°
                                                        ret[i, j] = Nodes[size - i - 1, size - j - 1];
                                                }
                                        }
                                        Nodes = ret;
                                        break;

                                case -90:
                                        for (int i = 0; i < size; ++i)
                                        {
                                                for (int j = 0; j < size; ++j)
                                                {
                                                        // totate -90°
                                                        ret[j, i] = Nodes[i, size - j - 1];
                                                }
                                        }
                                        Nodes = ret;
                                        break;

                                default:
                                        Console.WriteLine("Cant Rotate: Pls Set Val to -90, 90, 180 ");
                                        break;
                        }


                }


                public void writeToFile(string path, string text)
                {
                        File.WriteAllText(path, text);
                }
                public void SerialiseToJson(string path)
                {

                        string str = JsonConvert.SerializeObject(this, Formatting.Indented);
                        Debug.Log($"{str}");

                }
                public void Desirealise(string path)
                {
                        string str = File.ReadAllText(path);

                        Node[,] newnodes = JsonConvert.DeserializeObject<Node[,]>(str);
                        Nodes = newnodes;
                }




        }

}
