using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.SceneManagement;

public class BirdTokens : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject HasNotClaimedState;
    public GameObject ClaimingState;
    private int scoresToClaim;
    public GameObject ClaimButton;
    [SerializeField] public TMPro.TextMeshProUGUI scoresEarnedText;
    private const string DROP_ERC20_CONTRACT = "0x0C71eA1115746f3A41eEc52D2aE8B99f87ACE65c";
    private void Start()
    {
        HasNotClaimedState.SetActive(true);
        ClaimingState.SetActive(false);
    }
    private void Update()
    {
        // scoresEarnedText.text = "Token Earned: " + gameManager.score.ToString();
        scoresToClaim = gameManager.score;
    }
    // Start is called before the first frame update
    public async void GetTokenBlance()
    {
        try
        {
            var address = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
            Contract contract = ThirdwebManager.Instance.SDK.GetContract(DROP_ERC20_CONTRACT);
            var data = await contract.ERC20.BalanceOf(address);
            scoresEarnedText.text = "Token Earned: " + data.displayValue;
        }
        catch (System.Exception)
        {
            Debug.Log("Error getting token balance");
        }
    }

    // public void ResetBlance()
    // {
    //     scoresEarnedText.text = "Token Earned: 0";
    // }

    public async void MintERC20()
    {
        try
        {
            Debug.Log("Minting ERC20");
            Contract contract = ThirdwebManager.Instance.SDK.GetContract(DROP_ERC20_CONTRACT);
            HasNotClaimedState.SetActive(false);
            ClaimingState.SetActive(true);
            var results = await contract.ERC20.Claim(scoresToClaim.ToString());
            Debug.Log("ERC20 minted");
            GetTokenBlance();
            ClaimingState.SetActive(false);
            HasNotClaimedState.SetActive(true);
            ClaimButton.SetActive(false);
        }
        catch (System.Exception)
        {
            Debug.Log("Error minting ERC20");
        }

    }
}
