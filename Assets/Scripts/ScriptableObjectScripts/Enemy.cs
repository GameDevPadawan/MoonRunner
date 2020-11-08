using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "New Enemy", menuName = "NPC/Enemy")]
public class Enemy : ScriptableObject
{
    public float damage;
    public float moveSpeed;
    public float maxHealth;
}
