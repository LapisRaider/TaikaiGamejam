using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCard : MonoBehaviour
{
    [Header("UI related stuff")]
    public TextMeshProUGUI m_PlantNameText;
    public TextMeshProUGUI m_PlantDesriptionText;

    public TextMeshProUGUI m_TemperatureToGrowText;
    public TextMeshProUGUI m_PlantPrice;

    public Button m_BuyButton;
    public Image m_SpriteImage;
    public GameObject m_SoldOutSymbol;

    [Header("Store info")]
    public Plant_Types m_PlantType;
    int m_Price = 0;
    int m_MaxPlant = 0;

    public void Start()
    {
        Init();
        UpdateUI();
    }

    public void OnEnable()
    {
        UpdateUI();
    }

    public void Init()
    {
        PlantScriptableObj plantData = MapManager.Instance.GetPlantDataBase().GetPlantData(m_PlantType);
        if (plantData == null)
            return;

        m_Price = plantData.m_Price;
        m_PlantType = plantData.m_PlantType;

        m_PlantNameText.text = plantData.m_PlantName;
        m_PlantDesriptionText.text = plantData.m_PlantDescription;

        switch (plantData.m_TemperatureToGrow)
        {
            case TemperatureType.EXTREMELY_HOT:
                m_TemperatureToGrowText.text = "Can survive all temperatures";
                break;
            case TemperatureType.VERY_HOT:
                m_TemperatureToGrowText.text = "Can survive Scroch and below temperatures";
                break;
            case TemperatureType.HOT:
                m_TemperatureToGrowText.text = "Can survive hot and below temperatures";
                break;
            case TemperatureType.NICE:
                m_TemperatureToGrowText.text = "Can only survive nice temperature";
                break;
        }

        m_PlantPrice.text = "Price: $" + m_Price.ToString() + " per seed";
        m_MaxPlant = plantData.m_MaxNumberToPlant;
        m_SpriteImage.sprite = plantData.m_PlantSprite;
    }

    public void UpdateUI()
    {
        int totalPlayerHas = 0;

        //check in inventory + number planted
        Inventory inventory = GameStats.Instance.m_Inventory;
        if (inventory == null)
            return;

        totalPlayerHas = inventory.GetAmtInInventory(m_PlantType);
        totalPlayerHas += MapManager.Instance.GetAmtOfPlantOnMap(m_PlantType);

        //if more than the max plant amount, sold out, disable button
        if (totalPlayerHas >= m_MaxPlant)
        {
            if (m_BuyButton != null)
                m_BuyButton.interactable = false;

            if (m_SoldOutSymbol != null)
                m_SoldOutSymbol.SetActive(true);
        }
        else
        {
            if (m_BuyButton != null)
                m_BuyButton.interactable = true;

            if (m_SoldOutSymbol != null)
                m_SoldOutSymbol.SetActive(false);
        }
    }

    public void ClickBuyPlant()
    {
        //add in inventory
        Inventory inventory = GameStats.Instance.m_Inventory;
        if (inventory != null)
        {
            inventory.BuyOneIntoInventory(m_PlantType);
        }

        //deduct money from bank account
        Money money = GameStats.Instance.m_Money;
        if (money != null)
        {
            money.ReduceMoney(m_Price);
        }

        UpdateUI();
    }
}
