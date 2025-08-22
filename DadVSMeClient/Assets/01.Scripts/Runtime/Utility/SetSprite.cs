using H00N.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe
{
    public struct SetSprite
    {
        public SetSprite(Image image, string resourceKey)
        {
            SetSpriteAsync(image, resourceKey);
        }

        public SetSprite(SpriteRenderer spriteRenderer, string resourceKey)
        {
            SetSpriteAsync(spriteRenderer, resourceKey);
        }

        private async void SetSpriteAsync(Image image, string resourceKey)
        {
            if(string.IsNullOrEmpty(resourceKey))
            {
                image.sprite = null;
                image.color = new Color(1f, 1f, 1f, 0f);
                return;
            }

            Sprite resource = await ResourceManager.LoadResourceAsync<Sprite>(resourceKey);

            // Resource를 로딩하는 중에 파괴되었을 수 있다.
            if(image == null)
                return;

            image.gameObject.SetActive(resource != null);
            image.sprite = resource;
            image.color = new Color(1f, 1f, 1f, 1f);
        }

        private async void SetSpriteAsync(SpriteRenderer spriteRenderer, string resourceKey)
        {
            if(string.IsNullOrEmpty(resourceKey))
            {
                spriteRenderer.sprite = null;
                spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                return;
            }

            Sprite resource = await ResourceManager.LoadResourceAsync<Sprite>(resourceKey);

            // Resource를 로딩하는 중에 파괴되었을 수 있다.
            if(spriteRenderer == null)
                return;

            spriteRenderer.gameObject.SetActive(resource != null);
            spriteRenderer.sprite = resource;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}