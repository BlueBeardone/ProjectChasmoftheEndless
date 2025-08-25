using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CharacterCreationManager : MonoBehaviour
{
    public CharacterData characterData = new CharacterData();
    
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_Dropdown lineageDropdown;
    public TMP_Dropdown callingDropdown;
    public TMP_Dropdown backgroundDropdown;
    
    public TMP_Text strengthText;
    public TMP_Text dexterityText;
    public TMP_Text constitutionText;
    public TMP_Text intelligenceText;
    public TMP_Text wisdomText;
    public TMP_Text charismaText;
    
    public TMP_Text availablePointsText;
    
    [Header("Appearance UI References")]
    public TMP_Text skinToneText;
    public TMP_Text hairStyleText;
    public TMP_Text hairColorText;
    public TMP_Text eyeColorText;
    public TMP_Text facialFeatureText;
    public TMP_Text clothingColorText;
    
    public Image characterPreview;
    public Image[] appearanceLayers; // Layers for different appearance parts
    
    [Header("Appearance Options")]
    public Sprite[] skinTones;
    public Sprite[] hairStyles;
    public Sprite[] hairColors;
    public Sprite[] eyeColors;
    public Sprite[] facialFeatures;
    public Sprite[] clothingColors;
    
    [Header("Attribute Points")]
    public int availablePoints = 27; // Standard point-buy system
    private int[] baseAttributes = new int[6] { 8, 8, 8, 8, 8, 8 }; // Min values
    
    void Start()
    {
        InitializeUI();
        UpdateCharacterPreview();
        UpdateAttributeDisplays();
        UpdateAppearanceDisplays();
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
        
        // Initialize attributes to base values
        characterData.strength = baseAttributes[0];
        characterData.dexterity = baseAttributes[1];
        characterData.constitution = baseAttributes[2];
        characterData.intelligence = baseAttributes[3];
        characterData.wisdom = baseAttributes[4];
        characterData.charisma = baseAttributes[5];
    }
    
    void PopulateDropdownFromEnum(TMP_Dropdown dropdown, System.Type enumType)
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
        UpdateAttributeDisplays();
    }
    
    public void OnCallingChanged(int index)
    {
        characterData.selectedCalling = (CharacterData.Calling)index;
        UpdateCharacterPreview();
        // No longer setting default attributes for callings
    }
    
    public void OnBackgroundChanged(int index)
    {
        characterData.selectedBackground = (CharacterData.Background)index;
    }
    
    void UpdateCharacterPreview()
    {
        // Update the character preview based on all appearance options
        if (appearanceLayers.Length >= 6)
        {
            // Set each layer of the character appearance
            if (skinTones.Length > characterData.skinToneIndex)
                appearanceLayers[0].sprite = skinTones[characterData.skinToneIndex];
            
            if (hairStyles.Length > characterData.hairStyleIndex)
                appearanceLayers[1].sprite = hairStyles[characterData.hairStyleIndex];
            
            if (hairColors.Length > characterData.hairColorIndex)
                appearanceLayers[2].sprite = hairColors[characterData.hairColorIndex];
            
            if (eyeColors.Length > characterData.eyeColorIndex)
                appearanceLayers[3].sprite = eyeColors[characterData.eyeColorIndex];
            
            if (facialFeatures.Length > characterData.facialFeatureIndex)
                appearanceLayers[4].sprite = facialFeatures[characterData.facialFeatureIndex];
            
            if (clothingColors.Length > characterData.clothingColorIndex)
                appearanceLayers[5].sprite = clothingColors[characterData.clothingColorIndex];
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
        
        // Update button interactivity based on available points and min values
        UpdateAttributeButtonStates();
    }
    
    void UpdateAttributeButtonStates()
    {
        // This method would need to be implemented to enable/disable +/- buttons
        // based on available points and minimum values
        // You would need references to your button GameObjects
    }
    
    void UpdateAppearanceDisplays()
    {
        skinToneText.text = $"Skin Tone: {characterData.skinToneIndex + 1}";
        hairStyleText.text = $"Hair Style: {characterData.hairStyleIndex + 1}";
        hairColorText.text = $"Hair Color: {characterData.hairColorIndex + 1}";
        eyeColorText.text = $"Eye Color: {characterData.eyeColorIndex + 1}";
        facialFeatureText.text = $"Feature: {characterData.facialFeatureIndex + 1}";
        clothingColorText.text = $"Clothing: {characterData.clothingColorIndex + 1}";
    }
    
    // Appearance adjustment methods
    public void ChangeSkinTone(int direction)
    {
        characterData.skinToneIndex = (characterData.skinToneIndex + direction + skinTones.Length) % skinTones.Length;
        UpdateCharacterPreview();
        UpdateAppearanceDisplays();
    }
    
    public void ChangeHairStyle(int direction)
    {
        characterData.hairStyleIndex = (characterData.hairStyleIndex + direction + hairStyles.Length) % hairStyles.Length;
        UpdateCharacterPreview();
        UpdateAppearanceDisplays();
    }
    
    public void ChangeHairColor(int direction)
    {
        characterData.hairColorIndex = (characterData.hairColorIndex + direction + hairColors.Length) % hairColors.Length;
        UpdateCharacterPreview();
        UpdateAppearanceDisplays();
    }
    
    public void ChangeEyeColor(int direction)
    {
        characterData.eyeColorIndex = (characterData.eyeColorIndex + direction + eyeColors.Length) % eyeColors.Length;
        UpdateCharacterPreview();
        UpdateAppearanceDisplays();
    }
    
    public void ChangeFacialFeature(int direction)
    {
        characterData.facialFeatureIndex = (characterData.facialFeatureIndex + direction + facialFeatures.Length) % facialFeatures.Length;
        UpdateCharacterPreview();
        UpdateAppearanceDisplays();
    }
    
    public void ChangeClothingColor(int direction)
    {
        characterData.clothingColorIndex = (characterData.clothingColorIndex + direction + clothingColors.Length) % clothingColors.Length;
        UpdateCharacterPreview();
        UpdateAppearanceDisplays();
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
            int refund = GetAttributeCost(attributeIndex - 1); // Cost for the previous value
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
        // Validate that all points are spent (or allow unspent points)
        if (availablePoints > 0)
        {
            // You might want to show a confirmation dialog here
            Debug.Log($"You have {availablePoints} unspent points.");
        }
        
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
        
        // Save appearance data
        PlayerPrefs.SetString("CharacterAppearance", characterData.SerializeAppearance());
        
        PlayerPrefs.Save();
        
        // Load the game scene
        SceneManager.LoadScene("GameScene");
    }
    
    public void RandomizeCharacter()
    {
        // Randomize appearance
        characterData.skinToneIndex = Random.Range(0, skinTones.Length);
        characterData.hairStyleIndex = Random.Range(0, hairStyles.Length);
        characterData.hairColorIndex = Random.Range(0, hairColors.Length);
        characterData.eyeColorIndex = Random.Range(0, eyeColors.Length);
        characterData.facialFeatureIndex = Random.Range(0, facialFeatures.Length);
        characterData.clothingColorIndex = Random.Range(0, clothingColors.Length);
        
        // Update appearance displays
        UpdateAppearanceDisplays();
        UpdateCharacterPreview();
        
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
        
        // Randomize attributes using point buy system
        RandomizeAttributes();
    }
    
    void RandomizeAttributes()
    {
        // Reset attributes to base values
        characterData.strength = baseAttributes[0];
        characterData.dexterity = baseAttributes[1];
        characterData.constitution = baseAttributes[2];
        characterData.intelligence = baseAttributes[3];
        characterData.wisdom = baseAttributes[4];
        characterData.charisma = baseAttributes[5];
        
        availablePoints = 27; // Reset points
        
        // Randomly distribute points
        while (availablePoints > 0)
        {
            int attributeIndex = Random.Range(0, 6);
            int cost = GetAttributeCost(attributeIndex);
            
            if (availablePoints >= cost)
            {
                IncreaseAttribute(attributeIndex);
            }
        }
        
        UpdateAttributeDisplays();
    }
}