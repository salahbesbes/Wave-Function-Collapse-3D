using UnityEngine;
using UnityEngine.UI;

public class GridLayout : MonoBehaviour
{
        public UnityEngine.UI.GridLayoutGroup gridLayout;
        public Grid3D grid;
        public Text uiText;

        private Text[,] tmp;

        private void Awake()
        {
                gridLayout.constraint = UnityEngine.UI.GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount = grid.Dimention;
                tmp = new Text[grid.Dimention, grid.Dimention];
        }

        private void Start()
        {
                foreach (var item in grid.nodes)
                {
                        uiText.text = $"{item.Entropy}";
                        tmp[item.x, item.y] = Instantiate(uiText, transform);
                }
        }

        private void Update()
        {
                foreach (var item in grid.nodes)
                {
                        Text textTmp = tmp[item.x, item.y];
                        textTmp.text = $"{item.Entropy}";
                }
        }
}