using UnityEngine;

[CreateAssetMenu(fileName = "PersistentDataPlayer1", menuName = "Scriptable Objects/PersistentDataPlayer1")]
public class PersistentDataPlayer1 : ScriptableObject
{
    [SerializeField] private int actualPlayer;
    private int[] positionPlayer;

}
