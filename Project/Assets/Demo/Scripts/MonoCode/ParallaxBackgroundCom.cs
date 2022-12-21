using UnityEngine;

namespace Demo.Misc
{
    /// <summary>
    /// 背景视察组件
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParallaxBackgroundCom : MonoBehaviour
    {
        [SerializeField]
        [Header("视差速率")]
        private Vector2 parallaxEffectMultiplier;

        [SerializeField] 
        [Header("水平方向无限")]
        private bool infiniteHorizontal;

        [SerializeField]
        [Header("竖直方向无限")]
        private bool infiniteVertical;

        private Transform cameraTransform;
        private Vector3 lastCameraPosition;
        private float textureUnitSizeX;
        private float textureUnitSizeY;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            lastCameraPosition = cameraTransform.position;
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Texture2D texture = sprite.texture;
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
            textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        }

        private void LateUpdate()
        {
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
            lastCameraPosition = cameraTransform.position;

            if (infiniteHorizontal)
            {
                if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
                {
                    float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                    transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
                }
            }

            if (infiniteVertical)
            {
                if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
                {
                    float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                    transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
                }
            }
        }
    }
}
