using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpUI : MonoBehaviour
{
    public GameObject textPopUp;

    public void ShowDamage(int damage, Transform transformPos, Color color)
    {
        // Instantiate a damage text object at the specified gameObjectPos
        GameObject _popUp = Instantiate(textPopUp, transformPos.position, Quaternion.identity , transform);

        //Debug.Log("INSTANTIATE BROO");
        TextMeshProUGUI damageText = _popUp.GetComponent<TextMeshProUGUI>();
        Animator damageAnim = _popUp.GetComponent<Animator>();
        if (damageText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on instantiated object!");
            return;
        }

        // Set the damage amount and color
        damageText.text = $"{damage}";
        damageText.outlineColor = color;// Red color with full alpha

        // Start the fade-out animation
        // if(damageAnim.){
        //     Destroy(_popUp);
        // }
        float animTime = damageAnim.GetCurrentAnimatorStateInfo(0).length;
        Destroy(_popUp , animTime);

    }
}
