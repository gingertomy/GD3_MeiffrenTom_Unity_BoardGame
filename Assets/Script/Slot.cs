using System;
using System.Collections;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private int slotIndex = -1;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private Dice dice;
    [SerializeField] private float high;

    [SerializeField] private int moveTime;


    public Action<bool> isMoving;

    void OnEnable()
    {
        dice.OnDiceThrow += MoveSlot;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void MoveSlot(int result)
    {
        StartCoroutine(MovePlayer(result));
    }

    private IEnumerator MovePlayer(int result)
    {
        isMoving?.Invoke(false);

        slotIndex = Mathf.Clamp(
            slotIndex + result, 0, slots.Length - 1
        );
        

        yield return StartCoroutine(MoveToSlot(slotIndex));

        int effect = 0;

        if (slots[slotIndex].CompareTag("Bonus"))
        {
            effect = 2;
            Debug.Log("Bonus");
        }
        else if (slots[slotIndex].CompareTag("Malus"))
        {
            effect = -2;
            Debug.Log("Malus");
        }

        if (effect != 0)
        {
            yield return new WaitForSeconds(0.5f);

            slotIndex = Mathf.Clamp(slotIndex + effect, 0, slots.Length - 1);
            
            yield return StartCoroutine(MoveToSlot(slotIndex));
        }

        isMoving?.Invoke(true);
    }

    private IEnumerator MoveToSlot(int index)
    {
        Vector3 targetPosition =
            slots[index].transform.position + Vector3.up * high;

        while (Vector3.Distance(
            player.transform.position,
            targetPosition) > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                targetPosition,
                moveTime * Time.deltaTime
            );

            yield return null;
        }

        player.transform.position = targetPosition;
    }
}