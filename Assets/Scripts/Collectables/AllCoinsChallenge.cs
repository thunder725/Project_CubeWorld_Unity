using UnityEngine;
using TMPro;
using System.Collections;

public class AllCoinsChallenge : MonoBehaviour
{
    public LayerMask collectableLayerMask;

    public Transform bigStarSpawnPoint;
    public GameObject bigStarPrefab;

    public TextMeshProUGUI coinCounter;

    [SerializeField] Animator animator;

    int totalNumberOfCoins;
    int remainingNumberOfCoins;

    private void Awake()
    {
        GetAllCoins();

        UpdateText();
    }

    void GetAllCoins()
    {
        totalNumberOfCoins = FindObjectsOfType<CoinRotation>().Length;

        remainingNumberOfCoins = totalNumberOfCoins;
    }

    public void CoinCollected()
    {
        remainingNumberOfCoins--;

        if (remainingNumberOfCoins == 0)
        {
            SpawnBigStar();
        }

        UpdateText();
    }

    void SpawnBigStar()
    {
        Instantiate(bigStarPrefab, bigStarSpawnPoint.position, Quaternion.identity);

        GeneralEventManager.PauseGameplay.Invoke();
    }



    void UpdateText()
    {
        coinCounter.text = remainingNumberOfCoins + "/" + totalNumberOfCoins;
    }


}
