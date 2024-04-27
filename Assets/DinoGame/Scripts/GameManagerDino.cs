using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Thirdweb;

public class GameManagerDino : MonoBehaviour
{
    public static GameManagerDino Instance { get; private set; }
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI hiscoreText;
    public TextMeshProUGUI scoreText;

    public Button retryButton;
    private PlayerDino player;
    private SpawnerDino spawner;
    private float score;

    public GameObject ERC20TokenBalanceText;
    private string tokenContractAddress = "0x9f87A223c1A2C37e15A72e4F61937eAF616A1ea7";
    private ThirdwebSDK sdk;
    public GameObject claimingStatusTxt;
    public GameObject claimTokenBtn;
    public GameObject tokenClaimingPanel;

    private bool enabledRetry;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerDino>();
        spawner = FindObjectOfType<SpawnerDino>();
        NewGame();
        sdk = ThirdwebManager.Instance.SDK;
        GetTokenBalance();
        claimingStatusTxt.SetActive(false);
        claimTokenBtn.SetActive(true);
        tokenClaimingPanel.SetActive(false);
        retryButton.gameObject.SetActive(false);
    }

    public async void GetTokenBalance()
    {
        string address = await sdk.Wallet.GetAddress();
        Contract contract = sdk.GetContract(tokenContractAddress);
        var data = await contract.ERC20.BalanceOf(address);
        ERC20TokenBalanceText.GetComponent<TMPro.TextMeshProUGUI>().text = data.displayValue;
        claimingStatusTxt.SetActive(false);
        retryButton.gameObject.SetActive(true);
    }

    public async void ClaimERC20Token() {
        claimingStatusTxt.SetActive(true);
        claimTokenBtn.SetActive(false);
        retryButton.gameObject.SetActive(false);
        Contract contract = sdk.GetContract(tokenContractAddress);
        var data = await contract.ERC20.Claim("10");
        Debug.Log("Tokens were claimed!");
        GetTokenBalance();
    }
    public void NewGame()
    {
        tokenClaimingPanel.SetActive(false);
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        score = 0f;
        gameSpeed = initialGameSpeed;
        enabledRetry = true;
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        UpdateHiScore();
    }
    public void GameOver()
    {
        if (score >= 1000f) {
            tokenClaimingPanel.SetActive(true);
        }
        gameSpeed = 0f;
        enabledRetry = false;
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        UpdateHiScore();
    }
    private void Update()
    {
        if (enabledRetry == true) {
            retryButton.gameObject.SetActive(false);
        }
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime * ScoreMultiplier.Ins.xScore;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }

    private void UpdateHiScore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);
        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }
        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
}
