using UnityEngine;

public class Clickable : MonoBehaviour
{
        private void OnMouseDown()
        {
                foreach (var node in Grid3D.Instance.nodes)
                {
                        //Debug.Log($"{node.position} {transform.position}");

                        if (CompaireVector3(node.position, transform.position))
                        {
                                //Debug.Log($" clicked on {node}");
                                Grid3D.Instance.ModuleSelected = node;

                                Grid3D.Instance.neighbours = node.neighbours;

                                if (node.collapsed == true) return;

                                node.Collapse();

                                return;
                        }
                }
                Debug.Log($" cant find position clicked ");
        }

        public bool CompaireVector3(Vector3 first, Vector3 second)
        {
                return RoundFloat(first.x, 2) == RoundFloat(second.x, 2) &&
                RoundFloat(first.y, 2) == RoundFloat(second.y, 2) &&
                RoundFloat(first.z, 2) == RoundFloat(second.z, 2);
        }

        protected float RoundFloat(float value, int nb)
        {
                float power = Mathf.Pow(10, nb);
                return Mathf.Round(value * power) * (1 / power);
        }
}