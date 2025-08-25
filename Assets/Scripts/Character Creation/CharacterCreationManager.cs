using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreationManager : MonoBehaviour
{
    public CharacterData characterData = new CharacterData();
    
    [Header("UI References")]
    public InputField nameInput;
    public Dropdown lineageDropdown;
    public Dropdown callingDropdown;
    public Dropdown backgroundDropdown;
    
    public Text strengthText;
    public Text dexterityText;
    public Text constitutionText;
    public Text intelligenceText;
    public Text wisdomText;
    public Text charismaText;
    
    public Text availablePointsText;
    
    public Image characterPreview;
    public Sprite[] lineageSprites; // Sprites for different lineages
    
    [Header("Attribute Points")]
    public int availablePoints = 27; // Standard point-buy system
    private int[] baseAttributes = new int[6] { 8, 8, 8, 8, 8, 8 }; // Min values
    
    void Start()
    {
        InitializeUI();
        UpdateCharacterPreview();
        UpdateAttributeDisplays();
    }
    
    void InitializeUI()
    {
        // Populate dropdowns
        PopulateDropdownFromEnum(lineageDropdown, typeof(CharacterData.Lineage));
        PopulateDropdownFromEnum(callingDropdown, typeof(CharacterData.Calling));
        PopulateDropdownFromEnum(backgroundDropdown, typeof(CharacterData.Background));
        
        // Set up event listeners
        nameInput.onValueChanged.AddListener(OnNameChanged);
        lineageDropdown.onValueChanged.AddListener(OnLineageChanged);
        callingDropdown.onValueChanged.AddListener(OnCallingChanged);
        backgroundDropdown.onValueChanged.AddListener(OnBackgroundChanged);
    }
    
    void PopulateDropdownFromEnum(Dropdown dropdown, System.Type enumType)
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        
        foreach (var value in System.Enum.GetValues(enumType))
        {
            options.Add(value.ToString());
        }
        
        dropdown.AddOptions(options);
    }
    
    public void OnNameChanged(string newName)
    {
        characterData.characterName = newName;
    }
    
    public void OnLineageChanged(int index)
    {
        characterData.selectedLineage = (CharacterData.Lineage)index;
        UpdateCharacterPreview();
        
        // Apply lineage bonuses (example)
        switch (characterData.selectedLineage)
        {
            case CharacterData.Lineage.Human:
                // Humans get +1 to all stats
                break;
            case CharacterData.Lineage.Elf:
                // Elves get +2 Dexterity
                break;
            case CharacterData.Lineage.Dwarf:
                // Dwarves get +2 Constitution
                break;
            case CharacterData.Lineage.Orc:
                // Orcs get +2 Strength
                break;
        }
        
        UpdateAttributeDisplays();
    }
    
    public void OnCallingChanged(int index)
    {
        characterData.selectedCalling = (CharacterData.Calling)index;
        
        // Apply calling starting bonuses (example)
        switch (characterData.selectedCalling)
        {
            case CharacterData.Calling.Fighter:
                characterData.strength = 15;
                characterData.dexterity = 12;
                characterData.constitution = 14;
                characterData.intelligence = 8;
                characterData.wisdom = 10;
                characterData.charisma = 8;
                break;
            case CharacterData.Calling.Rogue:
                characterData.strength = 8;
                characterData.dexterity = 15;
                characterData.constitution = 12;
                characterData.intelligence = 10;
                characterData.wisdom = 8;
                characterData.charisma = 14;
                break;
            // Add other classes...
        }
        
        UpdateAttributeDisplays();
    }
    
    public void OnBackgroundChanged(int index)
    {
        characterData.selectedBackground = (CharacterData.Background)index;
    }
    
    void UpdateCharacterPreview()
    {
        if (lineageSprites.Length > (int)characterData.selectedLineage)
        {
            characterPreview.sprite = lineageSprites[(int)characterData.selectedLineage];
        }
    }
    
    void UpdateAttributeDisplays()
    {
        strengthText.text = characterData.strength.ToString();
        dexterityText.text = characterData.dexterity.ToString();
        constitutionText.text = characterData.constitution.ToString();
        intelligenceText.text = characterData.intelligence.ToString();
        wisdomText.text = characterData.wisdom.ToString();
        charismaText.text = characterData.charisma.ToString();
        
        availablePointsText.text = $"Points Available: {availablePoints}";
    }
    
    // Attribute adjustment methods
    public void IncreaseAttribute(int attributeIndex)
    {
        if (availablePoints <= 0) return;
        
        int cost = GetAttributeCost(attributeIndex);
        if (availablePoints >= cost)
        {
            switch (attributeIndex)
            {
                case 0: characterData.strength++; break;
                case 1: characterData.dexterity++; break;
                case 2: characterData.constitution++; break;
                case 3: characterData.intelligence++; break;
                case 4: characterData.wisdom++; break;
                case 5: characterData.charisma++; break;
            }
            
            availablePoints -= cost;
            UpdateAttributeDisplays();
        }
    }
    
    public void DecreaseAttribute(int attributeIndex)
    {
        int currentValue = 0;
        switch (attributeIndex)
        {
            case 0: currentValue = characterData.strength; break;
            case 1: currentValue = characterData.dexterity; break;
            case 2: currentValue = characterData.constitution; break;
            case 3: currentValue = characterData.intelligence; break;
            case 4: currentValue = characterData.wisdom; break;
            case 5: currentValue = characterData.charisma; break;
        }
        
        if (currentValue > baseAttributes[attributeIndex])
        {
            int refund = GetAttributeCost(attributeIndex) - 1;
            switch (attributeIndex)
            {
                case 0: characterData.strength--; break;
                case 1: characterData.dexterity--; break;
                case 2: characterData.constitution--; break;
                case 3: characterData.intelligence--; break;
                case 4: characterData.wisdom--; break;
                case 5: characterData.charisma--; break;
            }
            
            availablePoints += refund;
            UpdateAttributeDisplays();
        }
    }
    
    int GetAttributeCost(int attributeIndex)
    {
        int value = 0;
        switch (attributeIndex)
        {
            case 0: value = characterData.strength; break;
            case 1: value = characterData.dexterity; break;
            case 2: value = characterData.constitution; break;
            case 3: value = characterData.intelligence; break;
            case 4: value = characterData.wisdom; break;
            case 5: value = characterData.charisma; break;
        }
        
        // D&D point buy cost table
        if (value <= 13) return 1;
        if (value == 14) return 2;
        return 3; // For 15 and above
    }
    
    public void FinalizeCharacter()
    {
        // Save character data to persistent storage
        PlayerPrefs.SetString("CharacterName", characterData.characterName);
        PlayerPrefs.SetInt("CharacterLineage", (int)characterData.selectedLineage);
        PlayerPrefs.SetInt("CharacterCalling", (int)characterData.selectedCalling);
        PlayerPrefs.SetInt("CharacterBackground", (int)characterData.selectedBackground);
        
        PlayerPrefs.SetInt("CharacterSTR", characterData.strength);
        PlayerPrefs.SetInt("CharacterDEX", characterData.dexterity);
        PlayerPrefs.SetInt("CharacterCON", characterData.constitution);
        PlayerPrefs.SetInt("CharacterINT", characterData.intelligence);
        PlayerPrefs.SetInt("CharacterWIS", characterData.wisdom);
        PlayerPrefs.SetInt("CharacterCHA", characterData.charisma);
        
        PlayerPrefs.Save();
        
        // Load the game scene
        SceneManager.LoadScene("GameScene");
    }
    
    public void RandomizeCharacter()
    {
        // Randomize appearance
        characterData.skinToneIndex = Random.Range(0, 5);
        characterData.hairStyleIndex = Random.Range(0, 8);
        characterData.hairColorIndex = Random.Range(0, 10);
        characterData.eyeColorIndex = Random.Range(0, 6);
        
        // Randomize name from a list
        string[] names = { "Aelar", "Borin", "Cedric", "Diana", "Elena", "Fargrim", "Gwen", "Hector" };
        characterData.characterName = names[Random.Range(0, names.Length)];
        nameInput.text = characterData.characterName;
        
        // Randomize lineage and calling
        int randomLineage = Random.Range(0, System.Enum.GetValues(typeof(CharacterData.Lineage)).Length);
        lineageDropdown.value = randomLineage;
        OnLineageChanged(randomLineage);
        
        int randomCalling = Random.Range(0, System.Enum.GetValues(typeof(CharacterData.Calling)).Length);
        callingDropdown.value = randomCalling;
        OnCallingChanged(randomCalling);
        
        // Randomize background
        int randomBackground = Random.Range(0, System.Enum.GetValues(typeof(CharacterData.Background)).Length);
        backgroundDropdown.value = randomBackground;
        OnBackgroundChanged(randomBackground);
    }
}