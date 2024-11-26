using Unity.Sentis;
using UnityEngine;

namespace YoloHolo.Services
{
    public abstract class YoloItem
    {
        public Vector2 Center { get; protected set; }

        public Vector2 Size { get; protected set; }

        public Vector2 TopLeft { get; protected set; }

        public Vector2 BottomRight { get; protected set; }

        public float Confidence { get; protected set; }

        public FoodItem MostLikelyObject { get; protected set; }

        public int maxIndex { get; protected set; }


        public static YoloItem Create(Tensor<float> tensorData, int boxIndex, IYoloClassTranslator translator)
        {
            return new YoloV7Item(tensorData, boxIndex, translator);
        }
    }
}