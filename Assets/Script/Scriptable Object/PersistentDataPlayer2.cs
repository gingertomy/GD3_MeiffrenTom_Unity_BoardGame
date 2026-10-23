using UnityEngine;

[CreateAssetMenu(fileName = "PersistentDataPlayer2", menuName = "Scriptable Objects/PersistentDataPlayer2")]
public class PersistentDataPlayer2 : ScriptableObject
{
    [SerializeField] private int actualPlayer;
    private int[] positionPlayer;
}
