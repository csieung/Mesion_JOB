using UnityEngine;

[CreateAssetMenu(fileName = "NewPet", menuName = "Pet")]
public class Pet : ScriptableObject
{
    public string petName;
    public int level;
    public int experience;
    public float affection; // 호감도
    public Sprite petSprite;
    public float roamRadius; // 펫이 돌아다닐 반경
    public float roamDelay;  // 각 이동 간의 지연 시간
}
