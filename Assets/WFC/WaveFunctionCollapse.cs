using UnityEngine;

public enum CubeFace : sbyte
{
        Back = -3,  // -z
        Bottom = -2,  // -y
        Left = -1,  // -x
        None = 0,
        Right = 1,  // +x
        Top = 2,  // +y
        Front = 3   // +z
}
[ExecuteAlways]
public class WaveFunctionCollapse : MonoBehaviour
{

        public Tile tile;
        public Transform point;

        private void Start()
        {
        }
        private void Update()
        {
        }
        [ExecuteInEditMode]
        private void OnGUI()
        {
                if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
                {

                        tile.LeftFace = new Face(tile.TileSideVoxels);

                        tile.RightFace = new Face(tile.TileSideVoxels);

                        tile.ForwardFace = new Face(tile.TileSideVoxels);

                        tile.BackFace = new Face(tile.TileSideVoxels);

                        tile.CreateFourSideColor();
                        tile.DestroyClones();
                        tile.Rotate90();
                        Tile clone = tile.Clones[0];
                        tile.LeftFace.CanConnect(clone.RightFace);
                        tile.ForwardFace.CanConnect(clone.RightFace);


                }
        }
        private void OnDrawGizmos()
        {

        }
}
