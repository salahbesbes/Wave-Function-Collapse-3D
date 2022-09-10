using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Face
{
        public List<Color> Colors = new List<Color>();
        private int index = 0;
        [SerializeField]
        private Direction direction;
        public Direction opposetFace;

        public Direction Direction
        {
                get => direction; set
                {
                        direction = value;
                        if (value == Direction.Left)
                                opposetFace = Direction.Right;

                        if (value == Direction.Right)
                                opposetFace = Direction.Left;

                        if (value == Direction.Forward)
                                opposetFace = Direction.Back;

                        if (value == Direction.Back)
                                opposetFace = Direction.Forward;
                }
        }


        public Face CloneFace(Direction direction)
        {
                Face newface = new Face(1, direction);

                foreach (Color color in Colors)
                {
                        newface.Colors.Add(color);
                }
                return newface;
        }
        public Face(int ColorCount, Direction direction)
        {
                //Colors = new Color[ColorCount * ColorCount];
                this.Direction = direction;


        }
        public void AddColor(Color color)
        {
                Colors[index] = color;
                index++;
        }
        public override int GetHashCode()
        {
                return 0;
        }
        public override bool Equals(object obj)
        {

                if (obj.GetType() != this.GetType() || obj == null)
                {
                        Debug.Log($" not same type ");
                        return false;
                }
                Face otherFace = (Face)obj;

                if (otherFace.Colors == null)
                {
                        Debug.Log($" face Colors are null");
                        return false;
                }

                if (otherFace.opposetFace != Direction)
                {
                        Debug.Log($" can onlt connect opposit faces");
                        return false;
                }

                bool Result = true;
                //Debug.Log($" colors of both side is equal  {otherFace.Colors.Count == Colors.Count}");
                if (otherFace.Colors.Count == Colors.Count)
                {

                        for (int i = 0; i < Colors.Count; i++)
                        {

                                if (otherFace.Colors[i] != this.Colors[i])
                                {
                                        //Debug.Log($"other color {otherFace.Colors[i]} myColor {Colors[i]}");
                                        Result = false;
                                        break;
                                }

                        }
                }
                else
                {
                        Result = false;
                }

                List<Color> tmp1 = new List<Color>();
                foreach (Color color in Colors)
                {
                        if (ColorUtility.ToHtmlStringRGB(color) != "000000")
                                tmp1.Add(color);
                }
                List<Color> tmp2 = new List<Color>();
                foreach (Color color in otherFace.Colors)
                {
                        if (ColorUtility.ToHtmlStringRGB(color) != "000000")
                                tmp2.Add(color);
                }

                string color1 = "";
                //Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}> ################### </color>");

                foreach (var color in tmp1)
                {
                        color1 += $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>#</color>";
                }

                if (Result)
                {
                        color1 += " Match ";
                }
                else
                {
                        color1 += " Does Not Match ";
                }

                foreach (var color in tmp2)
                {
                        color1 += $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>#</color>";
                }


                Debug.Log($"{color1}");

                return Result;
        }

        public bool CanConnect(Face otherFace)
        {
                // can only connect opposet Sides
                if (otherFace.Direction != opposetFace) return false;
                return Equals(otherFace);
        }
}
public class Tile : MonoBehaviour
{
        [HideInInspector]
        public int rotationNumber = 0;
        public float VoxelSize = 0.1f;
        public int TileSideVoxels = 3;
        public List<Tile> forwardCantBe = new List<Tile>();
        public List<Tile> backCantBe = new List<Tile>();
        public List<Tile> rightCantBe = new List<Tile>();
        public List<Tile> leftCantBe = new List<Tile>();
        [Range(1, 100)]
        public int Weight = 50;

        public RotationType Rotation;
        public List<Tile> Clones = new List<Tile>();


        public Face LeftFace;
        public Face RightFace;
        public Face ForwardFace;
        public Face BackFace;
        public bool EnableGizmoz = false;

        public enum RotationType
        {
                FourRotations,
                OnlyRotation,
                TwoRotations,
        }
        public void Rotate90(int times = 1)
        {

                Vector3 newPos = transform.position + Vector3.left;

                if (Rotation == RotationType.OnlyRotation)
                        return;
                else if (Rotation == RotationType.TwoRotations)
                {
                        Tile clone = Instantiate(this, newPos, Quaternion.identity);

                        clone.Init();
                        clone.transform.name = $"{transform.name}_{90}";
                        clone.rotationNumber = 1;
                        clone.transform.rotation = Quaternion.Euler(0, 90, 0);
                        clone.CreateFourSideColor();
                        clone.EnableGizmoz = EnableGizmoz;



                        // rotate Faces
                        clone.RightFace = ForwardFace;
                        clone.BackFace = RightFace;
                        clone.LeftFace = BackFace;
                        clone.ForwardFace = LeftFace;

                        clone.RightFace = ForwardFace.CloneFace(Direction.Right);
                        clone.BackFace = RightFace.CloneFace(Direction.Back);
                        clone.LeftFace = BackFace.CloneFace(Direction.Left);
                        clone.ForwardFace = LeftFace.CloneFace(Direction.Forward);



                        clone.rightCantBe = forwardCantBe;
                        clone.backCantBe = rightCantBe;
                        clone.leftCantBe = backCantBe;
                        clone.forwardCantBe = leftCantBe;
                        Clones.Add(clone);
                }
                else if (Rotation == RotationType.FourRotations)
                {
                        List<Tile> tmpforward = forwardCantBe;
                        List<Tile> tmpright = rightCantBe;
                        List<Tile> tmpback = backCantBe;
                        List<Tile> tmpleft = leftCantBe;



                        Face tmpRightFace = RightFace;
                        Face tmpForwardFace = ForwardFace;
                        Face tmpBackFace = BackFace;
                        Face tmpLeftFace = LeftFace;
                        for (int i = 1; i <= 3; i++)
                        {

                                Tile clone = Instantiate(this, newPos, Quaternion.identity);
                                clone.Init();
                                clone.transform.name = $"{transform.name}_{90 * i}";
                                clone.rotationNumber = i;
                                clone.transform.rotation = Quaternion.Euler(0, 90 * i, 0);
                                //clone.CreateFourSideColor();
                                clone.EnableGizmoz = EnableGizmoz;


                                // rotate faces
                                clone.RightFace = tmpForwardFace.CloneFace(Direction.Right);
                                clone.BackFace = tmpRightFace.CloneFace(Direction.Back);
                                clone.LeftFace = tmpBackFace.CloneFace(Direction.Left);
                                clone.ForwardFace = tmpLeftFace.CloneFace(Direction.Forward);

                                clone.ForwardFace.Direction = tmpForwardFace.Direction;
                                clone.RightFace.Direction = tmpRightFace.Direction;
                                clone.BackFace.Direction = tmpBackFace.Direction;
                                clone.LeftFace.Direction = tmpLeftFace.Direction;

                                tmpRightFace = clone.RightFace;
                                tmpForwardFace = clone.ForwardFace;
                                tmpBackFace = clone.BackFace;
                                tmpLeftFace = clone.LeftFace;



                                // rotate constraints
                                clone.rightCantBe = tmpforward;
                                clone.backCantBe = tmpright;
                                clone.leftCantBe = tmpback;
                                clone.forwardCantBe = tmpleft;

                                tmpforward = clone.forwardCantBe;
                                tmpright = clone.rightCantBe;
                                tmpback = clone.backCantBe;
                                tmpleft = clone.leftCantBe;


                                Clones.Add(clone);
                                newPos = clone.transform.position + Vector3.left;
                        }
                }

        }


        private void Start()
        {


                //Debug.Log($" left == forwatd {CanAppendTile(LeftFace.Colors, ForwardFace.Colors)}");


                //Debug.Log($" left == back {CanAppendTile(LeftFace.Colors, BackFace.Colors)}");


                //Debug.Log($" left == left {CanAppendTile(LeftFace.Colors, LeftFace.Colors)}");



                //Debug.Log($" Forward == back {CanAppendTile(ForwardFace.Colors, BackFace.Colors)}");


                //Debug.Log($" Forward == right {CanAppendTile(ForwardFace.Colors, RightFace.Colors)}");


                //Debug.Log($" Forward == back {CanAppendTile(ForwardFace.Colors, BackFace.Colors)}");


                //Debug.Log($" Forward == left {CanAppendTile(ForwardFace.Colors, LeftFace.Colors)}");

        }
        public void DestroyClones()
        {
                foreach (Tile prefab in Clones)
                {
                        if (prefab != null)
                                Destroy(prefab.gameObject);
                }

                Clones.Clear();
        }

        public void CreateFourSideColor()
        {
                MeshCollider meshCollider = transform.GetComponentInChildren<MeshCollider>();
                Vector3 rayStart = Vector3.zero; ;
                float offset = VoxelSize / 2;
                Vector3 rayDir = Vector3.zero;


                Vector3 colliderMinBound = meshCollider.bounds.min;
                Vector3 colliderMaxBound = meshCollider.bounds.max;
                Vector3[] list = RotatecolliderBounds(meshCollider, colliderMinBound, colliderMaxBound);
                colliderMinBound = list[0];
                colliderMaxBound = list[1];
                Face CurrentFace = null;
                for (int i = 0; i < TileSideVoxels; i++)
                {
                        for (int j = 0; j < TileSideVoxels; j++)
                        {

                                Vector3[] potentialRotation = new Vector3[4] { transform.right, -transform.right, transform.forward, -transform.forward };

                                foreach (Vector3 direction in potentialRotation)
                                {

                                        if (direction == transform.right)
                                        {
                                                rayStart = GetRayStartPoint(direction, rotationNumber, colliderMaxBound, colliderMinBound, offset, j);
                                                rayDir = -transform.right;
                                                CurrentFace = GetCurrentFace(direction);
                                        }


                                        if (direction == -transform.right)
                                        {

                                                rayDir = transform.right;
                                                rayStart = GetRayStartPoint(direction, rotationNumber, colliderMaxBound, colliderMinBound, offset, j);
                                                CurrentFace = GetCurrentFace(direction);
                                        }

                                        if (direction == transform.forward)
                                        {

                                                rayDir = -transform.forward;
                                                rayStart = GetRayStartPoint(direction, rotationNumber, colliderMaxBound, colliderMinBound, offset, j);
                                                CurrentFace = GetCurrentFace(direction);

                                        }

                                        if (direction == -transform.forward)
                                        {

                                                rayDir = transform.forward;
                                                rayStart = GetRayStartPoint(direction, rotationNumber, colliderMaxBound, colliderMinBound, offset, j);
                                                CurrentFace = GetCurrentFace(direction);
                                        }


                                        rayStart.y = meshCollider.bounds.min.y + offset + i * VoxelSize;

                                        if (Physics.Raycast(new Ray(rayStart, rayDir), out RaycastHit hit, VoxelSize))
                                        {


                                                Renderer renderer = hit.collider.GetComponent<MeshRenderer>();
                                                Texture2D texture2D = renderer.sharedMaterial.mainTexture as Texture2D;
                                                Vector2 pCoord = hit.textureCoord;
                                                pCoord.x *= texture2D.width;
                                                pCoord.y *= texture2D.height;

                                                Vector2 tiling = renderer.sharedMaterial.mainTextureScale;
                                                Color color = texture2D.GetPixel(Mathf.FloorToInt(pCoord.x * tiling.x), Mathf.FloorToInt(pCoord.y * tiling.y));

                                                //Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}> ################### </color>");

                                                //CurrentFace.Colors[i * TileSideVoxels + j] = color;
                                                CurrentFace.Colors.Add(color);

                                        }
                                        else
                                        {
                                                //CurrentFace.Colors[i * TileSideVoxels + j] = new Color(0, 0, 0, 0);

                                        }
                                }
                        }
                }
        }

        private Face GetCurrentFace(Vector3 direction)
        {
                Face CurrentFace = null;
                if (direction == transform.right)
                {
                        if (rotationNumber % 2 == 0)
                                CurrentFace = RightFace;
                        else
                                CurrentFace = LeftFace;

                }
                if (direction == -transform.right)
                {
                        if (rotationNumber % 2 == 0)
                                CurrentFace = LeftFace;
                        else
                                CurrentFace = RightFace;
                }
                if (direction == transform.forward)
                {
                        if (rotationNumber % 2 == 0)
                                CurrentFace = ForwardFace;
                        else
                                CurrentFace = BackFace;

                }
                if (direction == -transform.forward)
                {
                        if (rotationNumber % 2 == 0)
                                CurrentFace = BackFace;
                        else
                                CurrentFace = ForwardFace;

                }
                return CurrentFace;
        }

        private Direction GetCurrentDirection(Direction direction)
        {
                Direction CurrentDir = Direction.None;
                if (direction == Direction.Right)
                {
                        if (rotationNumber % 2 == 0)
                                CurrentDir = Direction.Right;
                        else
                                CurrentDir = Direction.Left;

                }
                if (direction == Direction.Left)
                {
                        if (rotationNumber % 2 == 0)
                                CurrentDir = Direction.Left;
                        else
                                CurrentDir = Direction.Right;
                }
                if (direction == Direction.Forward)
                {
                        if (rotationNumber % 2 == 0)
                                CurrentDir = Direction.Forward;
                        else
                                CurrentDir = Direction.Back;

                }
                if (direction == Direction.Back)
                {
                        if (rotationNumber % 2 == 0)
                                CurrentDir = Direction.Back;
                        else
                                CurrentDir = Direction.Forward;

                }
                return CurrentDir;
        }


        Vector3[] RotatecolliderBounds(MeshCollider meshCollider, Vector3 colliderMinBound, Vector3 colliderMaxBound)
        {

                Vector3 center = (meshCollider.bounds.max + meshCollider.bounds.min) / 2;

                Vector3 minDirection = colliderMinBound - center;
                Vector3 minRotated = Quaternion.Euler(0, 90 * rotationNumber, 0) * minDirection;
                colliderMinBound = center + minRotated;


                Vector3 maxDirection = colliderMaxBound - center;
                Vector3 maxRotated = Quaternion.Euler(0, 90 * rotationNumber, 0) * maxDirection;
                colliderMaxBound = center + maxRotated;
                return new Vector3[2] { colliderMinBound, colliderMaxBound };


        }

        [ExecuteInEditMode]
        private void OnDrawGizmos()
        {
                if (EnableGizmoz == false) return;
                MeshCollider meshCollider = transform.GetComponentInChildren<MeshCollider>();
                Vector3 rayStart = Vector3.zero; ;
                float offset = VoxelSize / 2;
                Vector3 rayDir = Vector3.zero;
                Vector3 center = (meshCollider.bounds.max + meshCollider.bounds.min) / 2;
                Gizmos.color = Color.black;

                Gizmos.DrawSphere(center, 0.03f);

                Vector3 colliderMinBound = meshCollider.bounds.min;
                Vector3 colliderMaxBound = meshCollider.bounds.max;

                // if rotation != 0 we want to rotate the colliderMinBound && colliderMaxBound
                if (rotationNumber != 0)
                {
                        Vector3 minDirection = colliderMinBound - center;
                        Vector3 minRotated = Quaternion.Euler(0, 90 * rotationNumber, 0) * minDirection;
                        colliderMinBound = center + minRotated;


                        Vector3 maxDirection = colliderMaxBound - center;
                        Vector3 maxRotated = Quaternion.Euler(0, 90 * rotationNumber, 0) * maxDirection;
                        colliderMaxBound = center + maxRotated;
                }

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(colliderMinBound, 0.07f);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(colliderMaxBound, 0.07f);


                for (int i = 0; i < TileSideVoxels; i++)
                {
                        for (int j = 0; j < TileSideVoxels; j++)
                        {


                                Vector3[] potentialRotation = new Vector3[4] { transform.right, -transform.right, transform.forward, -transform.forward };

                                foreach (Vector3 direction in potentialRotation)
                                {
                                        Gizmos.color = Color.red;
                                        Gizmos.DrawWireSphere(meshCollider.bounds.min, 0.03f);
                                        Gizmos.color = Color.blue;
                                        Gizmos.DrawWireSphere(meshCollider.bounds.max, 0.03f);

                                        if (direction == transform.right)
                                        {
                                                rayStart = GetRayStartPoint(direction, rotationNumber, colliderMaxBound, colliderMinBound, offset, j);
                                                rayDir = -transform.right;
                                                Gizmos.color = Color.black;
                                        }


                                        if (direction == -transform.right)
                                        {

                                                rayDir = transform.right;
                                                Gizmos.color = Color.white;
                                                rayStart = GetRayStartPoint(direction, rotationNumber, colliderMaxBound, colliderMinBound, offset, j);

                                        }

                                        if (direction == transform.forward)
                                        {

                                                rayDir = -transform.forward;
                                                Gizmos.color = Color.cyan;
                                                rayStart = GetRayStartPoint(direction, rotationNumber, colliderMaxBound, colliderMinBound, offset, j);


                                        }

                                        if (direction == -transform.forward)
                                        {

                                                rayDir = transform.forward;
                                                Gizmos.color = Color.red;
                                                rayStart = GetRayStartPoint(direction, rotationNumber, colliderMaxBound, colliderMinBound, offset, j);

                                        }


                                        rayStart.y = meshCollider.bounds.min.y + offset + i * VoxelSize;
                                        Gizmos.DrawSphere(rayStart, 0.01f);

                                        if (Physics.Raycast(new Ray(rayStart, rayDir), out RaycastHit hit, VoxelSize))
                                        {

                                                Gizmos.color = Color.magenta;
                                                Gizmos.DrawLine(rayStart, rayStart + rayDir * VoxelSize);

                                                Renderer renderer = hit.collider.GetComponent<MeshRenderer>();
                                                Texture2D texture2D = renderer.sharedMaterial.mainTexture as Texture2D;
                                                Vector2 pCoord = hit.textureCoord;
                                                pCoord.x *= texture2D.width;
                                                pCoord.y *= texture2D.height;

                                                Vector2 tiling = renderer.sharedMaterial.mainTextureScale;
                                                Color color = texture2D.GetPixel(Mathf.FloorToInt(pCoord.x * tiling.x), Mathf.FloorToInt(pCoord.y * tiling.y));

                                                //Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}> ################### </color>");



                                        }
                                        else
                                        {

                                        }
                                }
                        }
                }

                //rayStart.y = meshCollider.bounds.min.y + half + Y * TileSideVoxels;





        }

        private Vector3 GetRayStartPoint(Vector3 direction, int rotationNumber, Vector3 colliderMaxBound, Vector3 colliderMinBound, float offset, int j)
        {
                Vector3 rayStart = Vector3.zero;
                if (direction == transform.right)
                {
                        if (rotationNumber == 0)
                        {
                                rayStart = colliderMaxBound +
                                                new Vector3(offset, 0, -offset - (TileSideVoxels - j - 1) * VoxelSize);
                        }
                        else if (rotationNumber == 1)
                        {
                                rayStart = colliderMaxBound +
                                                new Vector3(-offset - (TileSideVoxels - j - 1) * VoxelSize, 0, -offset);
                        }
                        else if (rotationNumber == 2)
                        {
                                rayStart = colliderMaxBound +
                                 new Vector3(-offset, 0, offset + j * VoxelSize);
                        }
                        else if (rotationNumber == 3)
                        {
                                rayStart = colliderMaxBound +
                                  new Vector3(offset + j * VoxelSize, 0, offset);
                        }
                }
                if (direction == -transform.right)
                {
                        if (rotationNumber == 0)
                        {
                                rayStart = colliderMinBound +
                                   new Vector3(-offset, 0, offset + j * VoxelSize);
                        }
                        else if (rotationNumber == 1)
                        {
                                rayStart = colliderMinBound +
                                  new Vector3(offset + j * VoxelSize, 0, offset);
                        }
                        else if (rotationNumber == 2)
                        {
                                rayStart = colliderMinBound +
                                   new Vector3(offset, 0, -offset - (TileSideVoxels - j - 1) * VoxelSize);
                        }
                        else if (rotationNumber == 3)
                        {
                                rayStart = colliderMinBound +
                                                new Vector3(-offset - (TileSideVoxels - j - 1) * VoxelSize, 0, -offset);
                        }

                }
                if (direction == transform.forward)
                {
                        if (rotationNumber == 0)
                        {
                                rayStart = colliderMaxBound +
                                          new Vector3(-offset - (TileSideVoxels - j - 1) * VoxelSize, 0, offset);
                        }
                        else if (rotationNumber == 1)
                        {
                                rayStart = colliderMaxBound +
                                                 new Vector3(offset, 0, offset + j * VoxelSize);
                        }
                        else if (rotationNumber == 2)
                        {
                                rayStart = colliderMaxBound +
                                                new Vector3(offset + j * VoxelSize, 0, -offset);
                        }
                        else if (rotationNumber == 3)
                        {
                                rayStart = colliderMaxBound +
                                   new Vector3(-offset, 0, -offset - (TileSideVoxels - j - 1) * VoxelSize);
                        }

                }
                if (direction == -transform.forward)
                {
                        if (rotationNumber == 0)
                        {
                                rayStart = colliderMinBound +
                                  new Vector3(offset + j * VoxelSize, 0, -offset);
                        }
                        else if (rotationNumber == 1)
                        {
                                rayStart = colliderMinBound +
                                   new Vector3(-offset, 0, -offset - (TileSideVoxels - j - 1) * VoxelSize);
                        }
                        else if (rotationNumber == 2)
                        {
                                rayStart = colliderMinBound +
                                          new Vector3(-offset - (TileSideVoxels - j - 1) * VoxelSize, 0, offset);
                        }
                        else if (rotationNumber == 3)
                        {
                                rayStart = colliderMinBound +
                                                new Vector3(offset, 0, offset + j * VoxelSize);
                        }

                }

                return rayStart;
        }

        public void Init()
        {

                LeftFace = new Face(TileSideVoxels, Direction.Left);

                RightFace = new Face(TileSideVoxels, Direction.Right);

                ForwardFace = new Face(TileSideVoxels, Direction.Forward);

                BackFace = new Face(TileSideVoxels, Direction.Back);
                MeshCollider meshCollider = transform.GetComponentInChildren<MeshCollider>();
                TileSideVoxels = Mathf.RoundToInt(meshCollider.bounds.size.x / VoxelSize);

        }

}
