using System;
using System.Collections.Generic;
using YoloHolo.Services;

namespace YoloHolo.YoloLabeling
{
    [Serializable]
    public class ArDemoModelTranslator : IYoloClassTranslator
    {
        public FoodItem GetFoodItem(int classIndex)
        {
            return detectableObjects[classIndex];
        }

        private static List<FoodItem> detectableObjects = new()
        {
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            new FoodItem("موز", 105f, 27.1f, 1.3f, 0.35f),
            new FoodItem("تفاح", 95f, 25.5f, 0.55f, 0.36f),
            null,
            new FoodItem("برتقال", 110f, 28.2f, 2.12f, 0.24f),
            new FoodItem("بروكلي", 51f, 9.96f, 4.23f, 0.55f),
            new FoodItem("جزر", 25.01f, 5.84f, 0.57f, 0.15f),
            new FoodItem("نقانق", 151f, 2.2f, 5f, 13f),
            null,
            new FoodItem("دونات", 250f, 34f, 3f, 11.9f),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        };
    }
}
