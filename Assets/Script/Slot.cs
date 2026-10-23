using System;
using System.Collections;
using UnityEngine;

public class Slot : MonoBehaviour
{
    // Index de la case actuelle du pion
    // -1 = le pion est avant la première case
    private int slotIndex = -1;
    // L'objet qu'on déplace sur le plateau.
    [SerializeField] private GameObject player;
    // Toutes les cases du plateau, dans l'ordre
    [SerializeField] private GameObject[] slots;
    // Référence au script Dice, pour s'abonner à son événement
    [SerializeField] private Dice dice;
    // Hauteur du pion au-dessus de la case (pour qu'il ne soit pas dans le sol)
    [SerializeField] private float high;
    // Vitesse de déplacement du pion
    [SerializeField] private int moveTime;

    // Événement envoyé à Dice :
    // false = le pion démarre, le dé est verrouillé
    // true  = le pion a fini, on peut tirer le dé
    public Action<bool> isMoving;

    void OnEnable()
    {
        // Il s'abonne à l'événement du dé :
        // à chaque lancer, MoveSlot sera appelé avec le résultat
        dice.OnDiceThrow += MoveSlot;

    }
    // Appelée automatiquement par Dice quand le dé est lancé
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void MoveSlot(int result)
    {
        // Le déplacement se fait sur plusieurs frames, utilisation d'une coroutine
        StartCoroutine(MovePlayer(result));
    }

    // Gère un tour complet : déplacement, effet de case, fin du tour
    private IEnumerator MovePlayer(int result)
    {
        // prévient Dice que le pion bouge (le dé se verrouille).
        isMoving?.Invoke(false);

        // Nouvel index = ancien index + résultat du dé.
        // Mathf.Clamp empêche de sortir du tableau (entre 0 et la dernière case)
        slotIndex = Mathf.Clamp(
            slotIndex + result, 0, slots.Length - 1
        );

        // Deplacment du pion et attend qu'il soit arrivé
        yield return StartCoroutine(MoveToSlot(slotIndex));

        // Effet de la case d'arrivée (0 = aucun effet)
        int effect = 0;

        // Lecture de la case pour savoir si c'est une case spéciale
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

        // S'il y a un effet, déplacement une seconde fois 
        if (effect != 0)
        {
            // pause pour que le joueur voie l'arrivée sur la case
            yield return new WaitForSeconds(0.5f);

            // Application de l'effet, limité aux bornes du tableau
            slotIndex = Mathf.Clamp(slotIndex + effect, 0, slots.Length - 1);
            // Second déplacement
            yield return StartCoroutine(MoveToSlot(slotIndex));
        }
        // Tour terminé : message à dice, le dé est de nouveau utilisable
        isMoving?.Invoke(true);
    }

    // Déplace le pion smoothly jusqu'à la case demandée
    private IEnumerator MoveToSlot(int index)
    {
        // Position cible = position de la case + un décalage vers le haut pour ne pas rentrer dans le sol
        Vector3 targetPosition =
            slots[index].transform.position + Vector3.up * high;

        // Tant que le pion n'est pas assez proche de la cible 
        while (Vector3.Distance(
            player.transform.position,
            targetPosition) > 0.01f)
        {
            // il avance vers la cible.
            // Time.deltaTime rend la vitesse indépendante des FPS
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                targetPosition,
                moveTime * Time.deltaTime
            );
            // attente de la frame suivante avant de continuer la boucle.
            yield return null;
        }
        // Placement du pion exactement sur la cible
        // pour corriger l'erreur d'arrondi de la boucle.
        player.transform.position = targetPosition;
    }
}