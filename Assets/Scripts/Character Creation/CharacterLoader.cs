using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public GameObject[] playerPrefabs; // Different prefabs for different lineages/classes
    
    void Start()
    {
        LoadCharacter();
    }
    
    void LoadCharacter()
    {
        if (PlayerPrefs.HasKey("CharacterName"))
        {
            // Create character data from saved values
            CharacterData characterData = new CharacterData
            {
                characterName = PlayerPrefs.GetString("CharacterName"),
                selectedLineage = (CharacterData.Lineage)PlayerPrefs.GetInt("CharacterLineage"),
                selectedCalling = (CharacterData.Calling)PlayerPrefs.GetInt("CharacterCalling"),
                selectedBackground = (CharacterData.Background)PlayerPrefs.GetInt("CharacterBackground"),
                strength = PlayerPrefs.GetInt("CharacterSTR"),
                dexterity = PlayerPrefs.GetInt("CharacterDEX"),
                constitution = PlayerPrefs.GetInt("CharacterCON"),
                intelligence = PlayerPrefs.GetInt("CharacterINT"),
                wisdom = PlayerPrefs.GetInt("CharacterWIS"),
                charisma = PlayerPrefs.GetInt("CharacterCHA")
            };

            // Instantiate the appropriate player prefab
            int prefabIndex = (int)characterData.selectedCalling; // Or combine lineage and calling
            if (prefabIndex < playerPrefabs.Length)
            {
                GameObject player = Instantiate(playerPrefabs[prefabIndex], Vector3.zero, Quaternion.identity);

                // Apply character stats to player components
                // PlayerController playerController = player.GetComponent<PlayerController>();
                // if (playerController != null)
                // {
                //     playerController.characterData = characterData;
                //     playerController.ApplyCharacterStats();
                // }
            }
            
            if (PlayerPrefs.HasKey("CharacterAppearance"))
            {
                characterData.DeserializeAppearance(PlayerPrefs.GetString("CharacterAppearance"));
            }
        }
        else
        {
            // No character created, go back to creation screen
            UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterCreation");
        }
    }
}