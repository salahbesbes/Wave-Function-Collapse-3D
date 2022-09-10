using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveFunctionCollapse))]
public class CustomInspector : Editor
{
        public override void OnInspectorGUI()
        {
                WaveFunctionCollapse wfc = (WaveFunctionCollapse)target;

                DrawDefaultInspector();
                GUILayout.Space(5);
                if (GUILayout.Button("Generate Modules"))
                {

                        int count = 1;
                        wfc.tiles.Clear();
                        foreach (Transform child in wfc.collection)
                        {
                                Tile tile = child.GetComponent<Tile>();


                                tile.Init();
                                tile.CreateFourSideColor();
                                tile.DestroyClones();
                                tile.Rotate90();
                                tile.name = tile.name.Substring(0, tile.name.Length - 2);
                                tile.transform.name += $"_{count}";
                                wfc.tiles.Add(tile);
                                count++;
                                foreach (Tile clone in tile.Clones)
                                {
                                        clone.transform.name += $"_{count}";
                                        count++;
                                        wfc.tiles.Add(clone);
                                }

                        }
                }
                GUILayout.Space(10);

                if (GUILayout.Button(" Reinit The Tiles"))
                {
                        wfc.Generate3DMatrix();
                        foreach (Cell cell in wfc.grid)
                        {
                                Clickable click = Instantiate(wfc.cellPrefab, wfc.transform);
                                cell.trans = click.transform;
                                cell.trans.position = cell.position;
                                cell.trans.name = $"[{cell.x} {cell.y}]";
                                cell.trans.SetParent(click.transform);
                                click.cell = cell;

                        }
                        wfc.PlaceAllPossibleCells();
                }

                if (GUILayout.Button(" Test 2 Faces"))
                {
                        wfc.tiles1.RightFace.Equals(wfc.tiles2.LeftFace);
                }
                //add everthing the button would do.
        }
}