using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    protected IEnumerator ShakeImage(Transform transform, float duration, float amount)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float offsetX = Random.Range(-amount, amount);
            float offsetY = Random.Range(-amount, amount);

            transform.localPosition = new Vector3(
                originalPosition.x + offsetX,
                originalPosition.y + offsetY,
                originalPosition.z
            );

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        transform.localPosition = originalPosition;
    }

    // Flash an image to red and then back to its original color
    protected IEnumerator FlashRed(Color originalColor, SpriteRenderer spriteRenderer, float duration)
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor;
    }

    // protected IEnumerator FlashRed(Image image, float duration)
    // {
    //     Color originalColor = image.color;
    //     image.color = Color.red;

    //     yield return new WaitForSeconds(duration);

    //     image.color = originalColor;
    // }

    public virtual void ShakeAndFlashRed(Color objColor,GameObject obj=null, float shakeDuration = .8f, float shakeAmount = .5f){}
    
}
