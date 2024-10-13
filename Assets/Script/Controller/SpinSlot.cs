using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSpinSlot : MonoBehaviour
{
    public List<Image> reel1Symbols;  // 3 symbols for Reel 1
    public List<Image> reel2Symbols;  // 3 symbols for Reel 2
    public List<Image> reel3Symbols;  // 3 symbols for Reel 3

    public List<Sprite> symbolSprites;  // Available sprites for the reels
    public Button spinButton;  // Reference to the Spin Button

    private bool isSpinning = false;  // To prevent overlapping spins
    private float spinDuration = 2.0f;  // Duration for each reel spin

    void Start()
    {
        // Assign the Spin Button click event to start the spin
        spinButton.onClick.AddListener(() => StartCoroutine(SpinReels()));
    }

    // Spins all three reels one by one with a delay between them
    IEnumerator SpinReels()
    {
        if (isSpinning) yield break;  // Prevent multiple simultaneous spins

        isSpinning = true;
        spinButton.interactable = false;  // Disable the button while spinning

        // Spin each reel with a small staggered delay
        StartCoroutine(SpinSingleReel(reel1Symbols));
        yield return new WaitForSeconds(0.2f);  // Delay between each reel spin
        StartCoroutine(SpinSingleReel(reel2Symbols));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(SpinSingleReel(reel3Symbols));

        // Wait for all spins to finish
        yield return new WaitForSeconds(spinDuration + 0.5f);

        // Check for a win condition
        CheckWin();

        spinButton.interactable = true;  // Enable the button after spin
        isSpinning = false;
    }

    // Spins a single reel by shifting symbols smoothly
    IEnumerator SpinSingleReel(List<Image> reel)
    {
        float elapsedTime = 0f;
        float singleSpinTime = 0.1f;  // Time for each symbol shift

        while (elapsedTime < spinDuration)
        {   
            if(elapsedTime < 0.5 && singleSpinTime > 0.01f){
                singleSpinTime*=0.8f;
            }
            else if(elapsedTime >1.5){
                singleSpinTime*=1.2f;
            }
            // Shift symbols down and wrap the last one to the top
            ShiftSymbols(reel);

            yield return new WaitForSeconds(singleSpinTime);  // Delay between shifts
            elapsedTime += singleSpinTime;
        }
    }

    // Helper function to shift symbols and wrap the last one to the top
    private void ShiftSymbols(List<Image> reel)
    {
        int index = Random.Range(0,symbolSprites.Count);
        reel[2].sprite = reel[1].sprite;
        reel[1].sprite = reel[0].sprite;
        reel[0].sprite = symbolSprites[index];
    }

    // Check if the middle symbols of all three reels match (win condition)
    private void CheckWin()
    {
        if (reel1Symbols[1].sprite == reel2Symbols[1].sprite &&
            reel2Symbols[1].sprite == reel3Symbols[1].sprite)
        {
            Debug.Log("You Win!");
        }
        else
        {
            Debug.Log("Try Again!");
        }
    }
}
