using UnityEngine;
using TMPro;
using System.Collections;
using UnityEditor;

public class AllCoinsChallenge : MonoBehaviour
{
    public TextMeshProUGUI coinCounter;

    [SerializeField] Animator animator;

    public GameObject cameraObject, bigStarObject;

    int totalNumberOfCoins;
    int remainingNumberOfCoins;

    private void Awake()
    {
        GetAllCoins();

        UpdateText();

        animator = GetComponent<Animator>();

        cameraObject.SetActive(false);
        bigStarObject.SetActive(false);


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

    public void SpawnBigStar()
    {
        GeneralEventManager.PauseGameplay.Invoke();

        cameraObject.SetActive(true);
        bigStarObject.SetActive(true);

        cameraObject.GetComponent<Camera>().targetDisplay = 0;

        animator.Play("CoinChallenge_ShowStar");
    }

    public void BigStarWasSpawned()
    {
        GeneralEventManager.ResumeGameplay.Invoke();

        cameraObject.SetActive(false);
        animator.Play("New State");
    }



    void UpdateText()
    {
        coinCounter.text = remainingNumberOfCoins + "/" + totalNumberOfCoins;
    }

    public void SubscribeToEvents()
    {
        GeneralEventManager.ResumeGameplay += Nothing;
        GeneralEventManager.PauseGameplay += Nothing;
    }

    void Nothing()
    {

    }


}

#if UNITY_EDITOR
[CustomEditor(typeof(AllCoinsChallenge))]
public class AllCoinsChallengeEditor : Editor
{
    AllCoinsChallenge _instance;

    private void Awake()
    {
        _instance = FindObjectOfType<AllCoinsChallenge>();
        _instance.SubscribeToEvents();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Spawn Big Star"))
        {
            _instance.SpawnBigStar();
        }
    }
}
#endif