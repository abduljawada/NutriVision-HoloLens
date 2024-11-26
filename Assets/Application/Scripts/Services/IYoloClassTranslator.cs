namespace YoloHolo.Services
{
    public interface IYoloClassTranslator
    {
        FoodItem GetFoodItem(int classIndex);
    }

    public class FoodItem
    {
        public string Name { get; protected set; }
        public float Calories { get; protected set; }
        public float Carbohydrates { get; protected set; }
        public float Protein { get; protected set; }
        public float Fat { get; protected set; }

        public FoodItem(string name, float calories, float carbohydrates, float protein, float fat)
        {
            this.Name = name;
            this.Calories = calories;
            this.Carbohydrates = carbohydrates;
            this.Protein = protein;
            this.Fat = fat;
        }

        public override string ToString()
        {
            if (IsEnglish(Name))
            {
                return $"{Name}\n Calories: {Calories}kCal\n Carbs: {Carbohydrates}g\n Protein: {Protein}g\n Fat: {Fat}g";
            }
            else
            {
                return $"{Name}\n السعرات: {Calories}\n كربوهيدرات: {Carbohydrates} جم\n بروتينات: {Protein} جم\n دهون: {Fat} جم";
            }
        }

        private static bool IsEnglish(string input)
        {
            foreach (char c in input)
            {
                // Check if the character is in the English (Latin) alphabet range
                if ((c >= 0x0041 && c <= 0x005A) ||    // A-Z
                    (c >= 0x0061 && c <= 0x007A))      // a-z
                {
                    return true; // It contains English characters
                }
            }
            return false; // No English characters found
        }

        public FoodData ToFoodData()
        {
            return new FoodData
            {
                Name = Name,
                Calories = Calories,
                Protein = Protein,
                Carbs = Carbohydrates,
                Fats = Fat
            };
        }
    }
}