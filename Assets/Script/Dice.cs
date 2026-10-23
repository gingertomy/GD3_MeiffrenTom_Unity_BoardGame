using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{

    // Message envoyé à Slot à chaque lancer
    // Il a le résultat du dé (1 à 6)
    public Action<int> OnDiceThrow;

    // Référence au script Slot, pour s'abonner à son événement.
    [SerializeField] private Slot slot;
    // true = on peut lancer le dé, false = le dé est verrouillé.
    private bool CanThrowDice = true;

    void OnEnable()
    {
        // Abonnement à l'événement de Slot :
        // à chaque fois que Slot envoie isMoving, CheckDice est appelé.
        slot.isMoving += CheckDice;
    }

    void OnDisable()
    {
        slot.isMoving -= CheckDice;
    }

    void Update()
    {
        // Chaque frame  si Espace est pressé er que le dé est libre, on lance le dé
        if (Input.GetKeyDown(KeyCode.Space) && CanThrowDice)
        {
            ThrowDice();
        }
    }
    // Lance le dé et envoie le résultat à Slot
    public void ThrowDice()
    {
        // Sécurité : si le dé est verrouillé, rien ne se passe
        if (!CanThrowDice)
        {
            Debug.Log("Deja en mouvement");
            return;
        }
        // Verouiller le dé pour empêcher un second lancer.
        CanThrowDice = false;

        // Chiffre aléatoire de 1 à 6 (le 7 est exclu)
        int result = Random.Range(1, 7);

        Debug.Log("NOUVEAU LANCER : " + result);

        // envoi le résultat à Slot.
        OnDiceThrow?.Invoke(result);

      
    }

    // Appelé par Slot par l'événement isMoving.
    // false = le pion bouge (dé verrouillé)
    // true  = le pion a fini (dé libre)
    private void CheckDice(bool canThrowDice)
    {
        CanThrowDice = canThrowDice;
    }
}