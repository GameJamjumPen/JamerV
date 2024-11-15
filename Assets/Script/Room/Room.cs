using UnityEngine;
public abstract class Room : MonoBehaviour
{
    public bool isOwned;
    public Transform buildPos;
    public GameObject OwnedRoom;
    public bool Treasure = false;
    public abstract void OnPlayerAttack();
    public virtual void DisplayRoom(){
        if(isOwned){
            Instantiate(OwnedRoom ,buildPos.position ,Quaternion.identity , this.transform);
        }
    }
}