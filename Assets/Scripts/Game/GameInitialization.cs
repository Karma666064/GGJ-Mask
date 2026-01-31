using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialization : MonoBehaviour
{
    GameState gs;

    //[SerializeField] GameObject card;
    [SerializeField] Transform hand;
    [SerializeField] Transform pool;

    [SerializeField] int maxCardInHand = 9;

    [SerializeField] float time = 1f;

    private void Awake()
    {
        gs = GetComponent<GameState>();
    }

    private void Start()
    {
        foreach (var card in gs.deck)
        {
            GameObject cardObject = Instantiate(card, pool);
        }

        StartCoroutine(InstantiateCardInHand());
    }

    IEnumerator InstantiateCardInHand ()
    {
        foreach (var card in gs.hand)
        {
            yield return new WaitForSeconds(time);
            GameObject cardObject = Instantiate(card, pool);
        }
    }
}
