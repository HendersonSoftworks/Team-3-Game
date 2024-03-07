using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellShop : MonoBehaviour
{
    public Text[] spellNameTexts; // Array of Text components for displaying spell names
    public Text[] spellDescriptionTexts; // Array of Text components for displaying spell descriptions
    public Text[] spellCostTexts; // Array of Text components for displaying spell costs
    public Image[] spellIconImages; // Array of Image components for displaying spell icons

    public Color highlightColor = Color.blue; // Color for highlighting selected spell
    public Color defaultColor = Color.white; // Default color for unselected spells

    public SpellData[] allSpells; // Array of all available spells
    public int maxInventorySize = 5; // Prefab for the inventory slot UI

    public List<SpellData> playerInventory = new List<SpellData>(); // Player's inventory data structure
    public Text[] inventorySpellName; // Text components for displaying spell names
    public Image[] inventorySpellIcon; // Image components for displaying spell icons
    public Button[] deleteButtons; // Array of delete buttons for each spell in the inventory

    public int playerCurrency = 1000;
    public Text currencyText;

    public GameObject spellPopupWindow; // Reference to the pop-up window GameObject
    public Image spellIconImage; // Image component for displaying spell icon in the pop-up window
    public Text spellPopupNameText; // Text component for displaying spell name in the pop-up window
    public Text spellPopupDescriptionText; // Text component for displaying spell description in the pop-up window
    public Text spellPopupCostText; // Text component for displaying spell cost in the pop-up window
    public Text inventoryFull; // Text component for displaying the inventory full text in the pop-up window
    public Text inventoryFullDes; // Text component for displaying the inventroy full description text in the pop-up window

    public GameObject spellSelectionPanel;
    public GameObject inventoryPanel;

    // References to the cancel and purchase buttons
    public Button cancelPurchaseButton;
    public Button confirmPurchaseButton;

    private bool isPopupActive = false; // Flag to track whether the pop-up window is active

    private int currentSpellIndex = 0; // Index of the currently selected spell

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.spellShop = gameObject.GetComponent<SpellShop>();

        Shuffle(allSpells); // Shuffle the spells once at the start
        UpdateSpellDisplay();
        UpdateCurrencyUI();

        // Add MM to invetory
        for (int i = 0; i < allSpells.Length; i++)
        {
            print(allSpells[i].spellName);
            if (allSpells[i].spellName == "Magic Missile")
            {
                playerInventory.Add(allSpells[i]);
            }
        }

        UpdateInventoryUI();
        spellPopupWindow.SetActive(false); // Deactivate the pop-up window initially

        // Assign onClick events to the buttons
        cancelPurchaseButton.onClick.AddListener(CancelPurchase);
        confirmPurchaseButton.onClick.AddListener(ConfirmPurchase);

        for (int i = 0; i < deleteButtons.Length; i++)
        {
            int index = i; // Capture the index in a local variable to avoid closure-related issues
            deleteButtons[i].onClick.AddListener(() => DeleteSpellFromInventory(index));
        }

    }

    void Update()
    {
        if (!isPopupActive)
        {
            HandleKeyboardInput();
            HandleMouseInput();
        }
    }

    //void DeleteSpellFromInventory(int index)
    //{
    //    if (index >= 0 && index < playerInventory.Count)
    //    {
    //        // Remove the spell at the specified index from the player's inventory
    //        playerInventory.RemoveAt(index);

    //        // Update the inventory UI
    //        UpdateInventoryUI();
    //    }
    //}

    public void DeleteSpellFromInventory(int index)
    {
        // Check if the index is valid
        if (index >= 0 && index < playerInventory.Count)
        {
            // Get the cost of the spell being deleted
            int spellCost = playerInventory[index].cost;

            // Add the cost of the spell back to the player's currency
            playerCurrency += spellCost;

            // Remove the spell from the inventory
            playerInventory.RemoveAt(index);

            // Update the UI
            UpdateCurrencyUI();
            UpdateInventoryUI();

            // Check if the inventory is no longer full
            if (playerInventory.Count < maxInventorySize)
            {
                // If the inventory is not full, hide the inventory full error message
                inventoryFull.gameObject.SetActive(false);
                inventoryFullDes.gameObject.SetActive(false);

                // Show the spell information and allow the user to purchase spells again
                spellPopupNameText.gameObject.SetActive(true);
                spellPopupDescriptionText.gameObject.SetActive(true);
                spellPopupCostText.gameObject.SetActive(true);
                spellIconImage.gameObject.SetActive(true);
            }
        }
    }

    void UpdateSpellDisplay()
    {
        for (int i = 0; i < spellNameTexts.Length; i++)
        {
            SpellData spell = allSpells[i];
            spellNameTexts[i].text = spell.spellName;
            spellDescriptionTexts[i].text = spell.description;
            spellCostTexts[i].text = spell.cost.ToString();
            spellIconImages[i].sprite = spell.icon;

            // Set color based on selection
            spellNameTexts[i].color = (i == currentSpellIndex) ? highlightColor : defaultColor;
            spellDescriptionTexts[i].color = (i == currentSpellIndex) ? highlightColor : defaultColor;
            spellCostTexts[i].color = (i == currentSpellIndex) ? highlightColor : defaultColor;
            spellIconImages[i].color = (i == currentSpellIndex) ? highlightColor : defaultColor;
        }
    }

    void UpdateCurrencyUI()
    {
        currencyText.text = playerCurrency.ToString();
    }

    void UpdateInventoryUI()
    {
        // Iterate over the inventory UI elements
        for (int i = 0; i < inventorySpellName.Length; i++)
        {
            // Check if the index is within the player's inventory size
            if (i < playerInventory.Count)
            {
                // Set UI element values based on spell data
                inventorySpellName[i].text = playerInventory[i].spellName;
                inventorySpellIcon[i].sprite = playerInventory[i].icon;
            }
            else
            {
                // If the index is beyond the player's inventory size,
                // clear the UI elements
                inventorySpellName[i].text = "";
                inventorySpellIcon[i].sprite = null;
            }

            // Update the delete button's onClick event based on whether
            // there's a spell in the inventory slot
            if (i < deleteButtons.Length)
            {
                int index = i; // Capture the index in a local variable
                deleteButtons[i].onClick.RemoveAllListeners(); // Clear existing listeners
                deleteButtons[i].onClick.AddListener(() => DeleteSpellFromInventory(index));
                deleteButtons[i].gameObject.SetActive(i < playerInventory.Count); // Show the button only if there's a spell in the slot
            }
        }
    }

    void HandleKeyboardInput()
    {
        // Handle keyboard input for spell selection
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousSpell();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextSpell();
        }
        else if (Input.GetKeyDown(KeyCode.Return)) // Enter key
        {
            DisplaySpellPopup();
        }
    }

    void HandleMouseInput()
    {
        // Handle mouse input for spell selection
        for (int i = 0; i < spellIconImages.Length; i++)
        {
            if (IsMouseOverSpell(spellIconImages[i]))
            {
                currentSpellIndex = i;
                UpdateSpellDisplay();

                if (Input.GetMouseButtonDown(0)) // Left mouse button
                {
                    DisplaySpellPopup();
                }

                break;
            }
        }
    }

    bool IsMouseOverSpell(Image spellIcon)
    {
        // Check if mouse is over the spell icon
        RectTransform rectTransform = spellIcon.GetComponent<RectTransform>();
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
        return rectTransform.rect.Contains(localMousePosition);
    }

    void SelectPreviousSpell()
    {
        currentSpellIndex = (currentSpellIndex - 1 + allSpells.Length) % allSpells.Length;
        UpdateSpellDisplay();
    }

    void SelectNextSpell()
    {
        currentSpellIndex = (currentSpellIndex + 1) % allSpells.Length;
        UpdateSpellDisplay();
    }

    void DisplaySpellPopup()
    {
        SpellData selectedSpell = allSpells[currentSpellIndex];

        // Check if the inventory is full, if yes, only return the inventory full messages. Do not show spell descriptions.
        if (playerInventory.Count >= maxInventorySize)
        {
            inventoryFull.gameObject.SetActive(true);
            inventoryFullDes.gameObject.SetActive(true);

            inventoryFull.text = "Inventory Full";
            inventoryFullDes.text = "You have reached the maximum number of spells in your inventory.";

            // Change the purchase button to a button that brings the user back to the shop
            confirmPurchaseButton.GetComponentInChildren<Text>().text = "Back to Shop";
            confirmPurchaseButton.onClick.RemoveAllListeners();
            confirmPurchaseButton.onClick.AddListener(BackToShop);

            spellPopupNameText.gameObject.SetActive(false);
            spellPopupDescriptionText.gameObject.SetActive(false);
            spellPopupCostText.gameObject.SetActive(false);
            spellIconImage.gameObject.SetActive(false);
        }

        else if (playerCurrency < selectedSpell.cost)
        {
            inventoryFull.gameObject.SetActive(true);
            inventoryFullDes.gameObject.SetActive(true);

            inventoryFull.text = "Insufficient Funds";
            inventoryFullDes.text = "You don't have enough currency to purchase this spell.";

            // Change the purchase button to a button that brings the user back to the shop
            confirmPurchaseButton.GetComponentInChildren<Text>().text = "Back to Shop";
            confirmPurchaseButton.onClick.RemoveAllListeners();
            confirmPurchaseButton.onClick.AddListener(BackToShop);

            spellPopupNameText.gameObject.SetActive(false);
            spellPopupDescriptionText.gameObject.SetActive(false);
            spellPopupCostText.gameObject.SetActive(false);
            spellIconImage.gameObject.SetActive(false);
        }

        else
        {
            // Update the pop-up window with information about the selected spell
            SpellData spell = allSpells[currentSpellIndex];
            spellPopupNameText.text = spell.spellName;
            spellPopupDescriptionText.text = spell.description;
            spellPopupCostText.text = spell.cost.ToString();
            spellIconImage.sprite = spell.icon;

            inventoryFull.gameObject.SetActive(false);
            inventoryFullDes.gameObject.SetActive(false);

            // Change the purchase button to a button that brings the user back to the shop
            confirmPurchaseButton.GetComponentInChildren<Text>().text = "Purchase";
            confirmPurchaseButton.onClick.RemoveAllListeners();
            confirmPurchaseButton.onClick.AddListener(ConfirmPurchase);
        }


        // Activate the pop-up window
        spellPopupWindow.SetActive(true);
        spellSelectionPanel.SetActive(false);
        inventoryPanel.SetActive(false);

        isPopupActive = true;
    }

    public void CancelPurchase()
    {
        // Close the pop-up window without making a purchase
        spellPopupWindow.SetActive(false);
        spellSelectionPanel.SetActive(true);
        inventoryPanel.SetActive(true);

        isPopupActive = false;
    }

    public void ConfirmPurchase()
    {
        // Check if the player's inventory is full
        if (playerInventory.Count >= maxInventorySize)
        {
            // Display a message indicating inventory full
            spellPopupNameText.text = "Inventory Full";
            spellPopupDescriptionText.text = "You have reached the maximum number of spells in your inventory.";

            // Change the purchase button to a button that brings the user back to the shop
            confirmPurchaseButton.GetComponentInChildren<Text>().text = "Back to Shop";
            confirmPurchaseButton.onClick.RemoveAllListeners();
            confirmPurchaseButton.onClick.AddListener(BackToShop);

            // Hide the cost text
            spellPopupCostText.gameObject.SetActive(false);
            spellIconImage.gameObject.SetActive(false);

            // Exit the method since the inventory is full
            return;
        }

        // Implement purchasing logic for selected spell
        SpellData selectedSpell = allSpells[currentSpellIndex];
        Debug.Log("Purchased spell: " + selectedSpell.spellName);

        // Check if the player has enough currency
        if (playerCurrency < selectedSpell.cost)
        {
            // Display a message indicating insufficient funds
            spellPopupNameText.text = "Insufficient Funds";
            spellPopupDescriptionText.text = "You don't have enough currency to purchase this spell.";

            // Change the purchase button to a button that brings the user back to the shop
            confirmPurchaseButton.GetComponentInChildren<Text>().text = "Back to Shop";
            confirmPurchaseButton.onClick.RemoveAllListeners();
            confirmPurchaseButton.onClick.AddListener(BackToShop);

            // Exit the method since there are insufficient funds
            return;
        }

        // Deduct the cost of the spell from the player's currency
        playerCurrency -= selectedSpell.cost;

        // Add the spell to the player's inventory
        playerInventory.Add(selectedSpell);

        // Update UI to reflect changes
        UpdateCurrencyUI();

        // Close the pop-up window after confirming purchase
        spellPopupWindow.SetActive(false);
        spellSelectionPanel.SetActive(true);
        inventoryPanel.SetActive(true);

        isPopupActive = false;

        // Update the inventory UI
        UpdateInventoryUI();
    }

    void BackToShop()
    {
        // Close the pop-up window
        spellPopupWindow.SetActive(false);
        spellSelectionPanel.SetActive(true);
        inventoryPanel.SetActive(true);

        isPopupActive = false;
    }

    // Fisher-Yates shuffle algorithm to shuffle an array
    public void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
