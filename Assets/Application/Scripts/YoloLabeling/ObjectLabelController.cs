using TMPro;
using UnityEngine;

namespace YoloHolo.YoloLabeling
{
    public class ObjectLabelController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMesh;

        [SerializeField] private GameObject contentParent;

        [SerializeField] private ArabicFixer3D arabicFixer3D;

        private FoodData foodData;

        public void SetText(string text)
        {
            if (arabicFixer3D != null)
            {
                arabicFixer3D.fixedText = text;
            }
            textMesh.text = text;
        }

        public void SetFoodData(FoodData newFoodData)
        {
            foodData = newFoodData;
        }

        private void Start()
        {
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, contentParent.transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }
        public void AddFood()
        {
            UIManager.Instance.AddFood(foodData);
        }
    }
}
