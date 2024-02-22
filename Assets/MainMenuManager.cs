using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thirdweb;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public const string ContractAddressDino = "0x638c83a22742c0274c0Fb0BA3cDA91Ad983bAFBb";
    public const string ContractAddressBird = "0x95215414CEB2d5ECcCd490613bb3ad53d87F44Cd";

    private Contract contractDino;
    private Contract contractBird;
    public Text dinoText;
    public Text birdText;
    public GameObject connectWalletPrompt;

    public GameObject dinoButton;
    public GameObject birdButton;

    private void Start()
    {
        connectWalletPrompt.SetActive(true);
        dinoButton.SetActive(false);
        birdButton.SetActive(false);
        dinoText.gameObject.SetActive(false);
        birdText.gameObject.SetActive(false);
    }

    public void CheckNFTStatus()
    {
        CheckStatusforDino();
        CheckStatusforBird();
    }

    private async void CheckStatusforDino()
    {
        contractDino = ThirdwebManager.Instance.SDK.GetContract(ContractAddressDino);
        var results = await contractDino.ERC721.Balance();
        if (int.Parse(results) == 0)
        {
            dinoText.text = "Click below to Claim Key to Play";
        }
        else if (int.Parse(results) >= 1)
        {
            dinoText.text = "Click Button to Play Game";
        }
        connectWalletPrompt.SetActive(false);
        dinoButton.SetActive(true);
        birdButton.SetActive(true);
        dinoText.gameObject.SetActive(true);
        birdText.gameObject.SetActive(true);
    }

    public async void GetNFTBalanceDino()
    {
        contractDino = ThirdwebManager.Instance.SDK.GetContract(ContractAddressDino);
        var results = await contractDino.ERC721.Balance();
        if (int.Parse(results) == 0)
        {
            ClaimNFTDino();
        }
        else if (int.Parse(results) >= 1)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

    public async void ClaimNFTDino()
    {
        try
        {
            contractDino = ThirdwebManager.Instance.SDK.GetContract(ContractAddressDino);
            var results = await contractDino.ERC721.Claim(1);
            SceneManager.LoadSceneAsync(1);
        }
        catch (System.Exception)
        {
            Debug.Log("Error claiming NFT");
        }
    }
    private async void CheckStatusforBird()
    {
        contractBird = ThirdwebManager.Instance.SDK.GetContract(ContractAddressBird);
        var results = await contractBird.ERC721.Balance();
        if (int.Parse(results) == 0)
        {
            birdText.text = "Click below to Claim Key to Play";
        }
        else if (int.Parse(results) >= 1)
        {
            birdText.text = "Click Button to Play Game";
        }
    }

    public async void GetNFTBalanceBird()
    {
        contractBird = ThirdwebManager.Instance.SDK.GetContract(ContractAddressBird);
        var results = await contractBird.ERC721.Balance();
        if (int.Parse(results) == 0)
        {
            ClaimNFTBird();
        }
        else if (int.Parse(results) >= 1)
        {
            SceneManager.LoadSceneAsync(2);
        }
    }

    public async void ClaimNFTBird()
    {
        try
        {
            contractBird = ThirdwebManager.Instance.SDK.GetContract(ContractAddressBird);
            var results = await contractBird.ERC721.Claim(1);
            SceneManager.LoadSceneAsync(2);
        }
        catch (System.Exception)
        {
            Debug.Log("Error claiming NFT");
        }
    }
}
