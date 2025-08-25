using System;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterName = "Adventurer";
    
    // Lineage (Race)
    public enum Lineage { Human, Elf, Dwarf, Orc }
    public Lineage selectedLineage = Lineage.Human;
    
    // Calling (Class)
    public enum Calling { Fighter, Rogue, Wizard, Cleric }
    public Calling selectedCalling = Calling.Fighter;
    
    // Background
    public enum Background { Acolyte, Criminal, FolkHero, Noble, Sage, Sellsword }
    public Background selectedBackground = Background.Sellsword;
    
    // Attributes (D&D style)
    public int strength = 10;
    public int dexterity = 10;
    public int constitution = 10;
    public int intelligence = 10;
    public int wisdom = 10;
    public int charisma = 10;
    
    // Appearance (for visual representation)
    public int skinToneIndex = 0;
    public int hairStyleIndex = 0;
    public int hairColorIndex = 0;
    public int eyeColorIndex = 0;
    public int facialFeatureIndex = 0;
    public int clothingColorIndex = 0;
    
    // Derived properties
    public int MaxHealth => 10 + (constitution - 10) / 2;
    public int ArmorClass => 10 + (dexterity - 10) / 2;
    
    // Serialize the appearance for easy saving
    public string SerializeAppearance()
    {
        return $"{skinToneIndex},{hairStyleIndex},{hairColorIndex},{eyeColorIndex},{facialFeatureIndex},{clothingColorIndex}";
    }
    
    public void DeserializeAppearance(string data)
    {
        string[] values = data.Split(',');
        if (values.Length == 6)
        {
            skinToneIndex = int.Parse(values[0]);
            hairStyleIndex = int.Parse(values[1]);
            hairColorIndex = int.Parse(values[2]);
            eyeColorIndex = int.Parse(values[3]);
            facialFeatureIndex = int.Parse(values[4]);
            clothingColorIndex = int.Parse(values[5]);
        }
    }
}