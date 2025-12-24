using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockSpawner : MonoBehaviour
{
    public static BlockSpawner instance;
    private float spawnHeight = 3.5f;
    private float spawnDepth = 10.1f;
    private float spawnWidth = 4.5f;
    [SerializeField] Block prefabBlock;
    [SerializeField] private SO_BlockType[] blockTypes;
    [SerializeField] private bool debugSpeedUp = false;
    [Space(10)]
    [Header("Settings")]
    [SerializeField]private float lerpAmount = 0.1f;
    private Vector3 pos;
    [SerializeField] private float spawnMarge = 0.5f;
    // Start is called before the first frame update
    void Awake(){
        instance = this;
    }

    void Update(){
        pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, spawnDepth));
        transform.position = new Vector2(Mathf.Lerp(transform.position.x, Math.Clamp(pos.x, -4.5f, 4.5f), lerpAmount), spawnHeight);
        // RefreshRate refreshRate;
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     Screen.SetResolution(1080, 1920, FullScreenMode.Windowed, Screen.currentResolution.refreshRateRatio);
        //     debugSpeedUp = !debugSpeedUp;
        // }
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     Screen.SetResolution(500, 1000, FullScreenMode.ExclusiveFullScreen, 25);
        //     debugSpeedUp = !debugSpeedUp;
        // }
    }
    public int maxBlockIndex = 3; // Set this to the maximum index you want to allow
    private int minBlockIndex = 2;

    public void OnPress(InputAction.CallbackContext context){
        if (!context.canceled && !debugSpeedUp){
            return;
        }

        // Create a list to hold the weighted indices
        List<int> weightedIndices = new List<int>();

        // Populate the list with indices, each index appears (maxBlockIndex - index + 1) times
        for (int i = 0; i <= maxBlockIndex; i++)
        {
            for (int j = 0; j < Math.Max(maxBlockIndex - (i * 2), 1); j++)
            {
                weightedIndices.Add(i);
            }
        }

        // Select a random index from the weighted list
        int blockIndex = weightedIndices[UnityEngine.Random.Range(0, weightedIndices.Count)];

        SO_BlockType blockType = blockTypes[blockIndex];
        SpawnBlock(blockType, transform.position);
    }
    public void SpawnBlock(SO_BlockType blockType, Vector2 position, bool isTransformed = false){
        if (isTransformed){
            foreach(SO_BlockType newBlockType in blockTypes){
                if(newBlockType.index == blockType.index + 1){
                    blockType = newBlockType;
                    maxBlockIndex = Math.Max(minBlockIndex, newBlockType.index - 2);
                    break;
                }
            }
        }
        Block block = Instantiate(prefabBlock, position, Quaternion.identity);
        block.SetBlockType(blockType);
        print(blockType.index);
    }
}