using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomGenerator2D : MonoBehaviour
{
    [Header("Room Dimensions")]
    public Vector2 roomSize = new Vector2(10f, 8f);
    public float wallThickness = 0.2f;

    [Header("Prefabs")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;

    [Header("Colliders")]
    public bool addColliders = true;
    public bool isTrigger = false;

    [Header("Generated Room")]
    public GameObject generatedRoom;
    public GameObject floorObject;
    public GameObject[] wallObjects = new GameObject[4];

    // Generate a 2D room with floor and walls
    public void GenerateRoom()
    {
        // Clear any existing room first
        ClearRoom();

        // Check if prefabs are assigned
        if (floorPrefab == null || wallPrefab == null)
        {
            Debug.LogError("Floor or Wall prefab is not assigned!");
            return;
        }

        // Create the room parent object
        generatedRoom = new GameObject("GeneratedRoom2D");
        generatedRoom.transform.SetParent(transform);
        generatedRoom.transform.localPosition = Vector3.zero;

        // Create floor
        floorObject = Instantiate(floorPrefab, Vector3.zero, Quaternion.identity);
        floorObject.name = "Floor";
        floorObject.transform.SetParent(generatedRoom.transform);
        floorObject.transform.localScale = new Vector3(roomSize.x, roomSize.y, 1f);

        // Create walls (top, bottom, left, right)
        wallObjects[0] = CreateWall("TopWall", 
            new Vector3(0, roomSize.y/2 + wallThickness/2, 0), 
            new Vector3(roomSize.x + wallThickness*2, wallThickness, 1f));
        
        wallObjects[1] = CreateWall("BottomWall", 
            new Vector3(0, -roomSize.y/2 - wallThickness/2, 0), 
            new Vector3(roomSize.x + wallThickness*2, wallThickness, 1f));
        
        wallObjects[2] = CreateWall("LeftWall", 
            new Vector3(-roomSize.x/2 - wallThickness/2, 0, 0), 
            new Vector3(wallThickness, roomSize.y + wallThickness*2, 1f));
        
        wallObjects[3] = CreateWall("RightWall", 
            new Vector3(roomSize.x/2 + wallThickness/2, 0, 0), 
            new Vector3(wallThickness, roomSize.y + wallThickness*2, 1f));
    }

    private GameObject CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        wall.name = name;
        wall.transform.SetParent(generatedRoom.transform);
        wall.transform.localScale = scale;
        
        // Add collider to walls if not already present
        if (addColliders && wall.GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
            collider.isTrigger = isTrigger;
        }
        
        return wall;
    }

    // Clear the generated room
    public void ClearRoom()
    {
        if (generatedRoom != null)
        {
            if (Application.isPlaying)
            {
                Destroy(generatedRoom);
            }
            else
            {
                DestroyImmediate(generatedRoom);
            }
        }
        floorObject = null;
        wallObjects = new GameObject[4];
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RoomGenerator2D))]
public class RoomGenerator2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomGenerator2D generator = (RoomGenerator2D)target;

        GUILayout.Space(10);
        
        if (GUILayout.Button("Generate Room"))
        {
            generator.GenerateRoom();
        }

        if (GUILayout.Button("Clear Room"))
        {
            generator.ClearRoom();
        }
    }
}
#endif