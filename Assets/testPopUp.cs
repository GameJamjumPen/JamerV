using UnityEngine;
using TMPro;

public class testPopUp : MonoBehaviour
{

    public Transform showPos;
    public int damage;
    public  GameObject textPopUp;
    public Color color1;
    public void ShowDamage(int damage, Transform transformPos, GameObject textPopUp , Color color)
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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            ShowDamage(damage , showPos , textPopUp, color1);
        }
    }
}
