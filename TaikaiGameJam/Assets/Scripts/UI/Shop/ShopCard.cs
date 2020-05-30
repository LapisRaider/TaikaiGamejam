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

    //TODO:: the current number planted or in inventory vs the max u can have
    public TextMeshProUGUI m_MaxPlantNumberText;

    public Image m_SpriteImage;

    public GameObject m_SoldOutSymbol;

    [Header("Store info")]
    public Plant_Types m_PlantType;
    int m_Price = 0;

    public void Start()
    {
        Init();
        if (m_SoldOutSymbol != null)
            m_SoldOutSymbol.SetActive(false);
    }

    public void OnEnable()
    {
        //check whether sold out TODO
        //CALL UPDATE UI
    }

    public void Init()
    {
        PlantScriptableObj plantData = MapManager.Instance.GetPlantDataBase().GetPlantData(m_PlantType);

        m_Price = plantData.m_Price;
        m_PlantType = plantData.m_PlantType;

        m_PlantNameText.text = plantData.m_PlantName;
        m_PlantDesriptionText.text = plantData.m_PlantDescription;
        m_TemperatureToGrowText.text = plantData.m_TemperatureToGrow.ToString();
        m_PlantPrice.text = "$" + m_Price.ToString();

        m_SpriteImage.sprite = plantData.m_PlantSprite;
    }

    public void UpdateUI()
    {
        //TODO:: update the number iin inventory and max u can grow
        //TODO:: add a sold out
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
    }
}
