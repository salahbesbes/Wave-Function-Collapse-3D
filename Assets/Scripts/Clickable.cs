using UnityEngine;

public class Clickable : MonoBehaviour
{
        public Cell cell;
        private void OnMouseDown()
        {
                cell.Collapse();
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