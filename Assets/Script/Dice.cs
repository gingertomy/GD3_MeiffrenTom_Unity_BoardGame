using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    public Action<int> OnDiceThrow;
    
   
    [SerializeField] private Slot slot;
    private bool CanThrowDice = true;

    void OnEnable()
    {
        slot.isMoving += CheckDice;
    }

    void OnDisable()
    {
        slot.isMoving -= CheckDice;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanThrowDice)
        {
            ThrowDice();
        }
    }

    public void ThrowDice()
    {
        if (!CanThrowDice)
        {
            Debug.Log("Deja en mouvement");
            return;
        }

        CanThrowDice = false;

        int result = Random.Range(1, 7);

        Debug.Log("NOUVEAU LANCER : " + result);

        OnDiceThrow?.Invoke(result);

      
    }

    private void CheckDice(bool canThrowDice)
    {
        CanThrowDice = canThrowDice;
    }
}