using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Basket UI")]
    // UI elements for total nutritional info
    [SerializeField] private TextMeshProUGUI totalCaloriesText;
    [SerializeField] private TextMeshProUGUI totalProteinText;
    [SerializeField] private TextMeshProUGUI totalCarbsText;
    [SerializeField] private TextMeshProUGUI totalFatsText;
    [SerializeField] private TextMeshProUGUI totalQuantText;

    [SerializeField] private GameObject scrollViewContent;

    [Space]
    [Header("Basket ScrollView")]
    
    public GameObject scrollViewElement;
    public FoodManager foodManager;

    private BasketManager basketManager;

    private Dictionary<string, GameObject> basketItems = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foodManager = new FoodManager();
        basketManager = new BasketManager(scrollViewContent.transform, scrollViewElement);
    }

    public void AddFood(FoodData selectedFood)
    {
        foodManager.AddFood(selectedFood);
        basketManager.UpdateBasket();
        UpdateTotalUI();
    }

    public void RemoveFood(string foodName) 
    {
        foodManager.RemoveFood(foodName);
        //basketManager.RemoveItem(foodName); //Not Implemented
        UpdateTotalUI();
    }

    public void OnAddJournalButtonClicked()
    {
        SaveEntryToFirebase();
        foodManager.ClearFoodList();
        basketManager.Clear();
        UpdateTotalUI();
    }

    private void UpdateTotalUI()
    {
        //Update total nutritional values
        var totals = foodManager.GetTotals();
        totalCaloriesText.text = $"{totals.totalCalories}\nkCal";
        totalProteinText.text = $"{totals.totalProtein}g";
        totalCarbsText.text = $"{totals.totalCarbs}g";
        totalFatsText.text = $"{totals.totalFats}g";
        totalQuantText.text = $"{totals.totalQuantity} Items";
    }

    private void SaveEntryToFirebase()
    {
        var totals = foodManager.GetTotals();
        List<Dictionary<string,object>> foodList = new List<Dictionary<string, object>>();

        foreach (var food in foodManager.GetFoodList())
        {
            foodList.Add(new Dictionary<string, object>
            {
                { "name", food.Name },
                { "quantity", food.Quantity },
                { "calories", food.Calories }
            });
        }

        Dictionary<string, object> journalEntry = new Dictionary<string, object>
        {
            {"timestamp", System.DateTime.UtcNow.ToString("o")},
            {"totalCalories", totals.totalCalories},
            {"totalProtien", totals.totalProtein},
            {"totalCarbs", totals.totalCarbs},
            {"totalFats", totals.totalFats},
            {"totalQuantity", totals.totalQuantity},
            {"foodItems", foodList}
        };

        //FirebaseManager.Instance.SaveJournalEntry(journalEntry);
    }
    //public void OnProfileNavButton() => FirebaseManager.Instance.LogOut();

}
