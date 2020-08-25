using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMutiplier = Vector2.zero;
    [SerializeField] private bool infiniteHorizontal = false, infiniteVertical = false;
    private float textureUnitSizeX, textureUnitSizeY;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width/sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height/sprite.pixelsPerUnit;
    }


    void FixedUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position  += new Vector3(deltaMovement.x * parallaxEffectMutiplier.x, deltaMovement.y * parallaxEffectMutiplier.y, deltaMovement.z);
        lastCameraPosition = cameraTransform.position;

        if(infiniteHorizontal)
        {    
            if(Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offSetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offSetPositionX, transform.position.y, transform.position.z);
            }
        }

        if(infiniteVertical)
        {   
            if(Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offSetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offSetPositionY, transform.position.z);
            }
        }

    }

}
