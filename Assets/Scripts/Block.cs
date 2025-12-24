using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void BlockDestroyed(Block block);
public class Block : MonoBehaviour
{
    public bool hasCollided = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private SO_BlockType blockType;
    Block collidedBlock;
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start(){

        // spriteRenderer.sprite = blockType.sprite;
        // transform.localScale = new Vector3(blockType.scale, blockType.scale, 1);
    }



    private void OnCollisionEnter2D(Collision2D collision){
        collidedBlock = collision.gameObject.GetComponent<Block>();
        if (collidedBlock == null){
            return;
        }

        if (collidedBlock.GetBlockType().index == blockType.index && !hasCollided && !collidedBlock.hasCollided)
        {
            // Both blocks are the same and neither has collided yet
            hasCollided = true;
            collidedBlock.hasCollided = true;

            BlockSpawner.instance.SpawnBlock(blockType, transform.position, true);

            Destroy(collidedBlock.gameObject);
            Destroy(gameObject);
        }

    //    if (collidedBlock.GetBlockType().index == blockType.index){
    //        Destroy(collidedBlock.gameObject);
    //        Debug.Log("Destroyed");
    //        Destroy(gameObject);
    //    }
    }
    public void SetBlockType(SO_BlockType newBlockType){
        blockType = newBlockType;
        spriteRenderer.sprite = blockType.sprite;
        transform.localScale = new Vector3(blockType.scale, blockType.scale, 1);
    }
    public SO_BlockType GetBlockType(){
        return blockType;
    }
}
