using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    int coins = 0;
    [SerializeField] Text coinsText;
    [SerializeField] AudioSource pickupCoin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectables"))
        {
            Destroy(other.gameObject);
            coins++;
            coinsText.text = "coins:" + coins;
            pickupCoin.Play();
        }
    }
}
