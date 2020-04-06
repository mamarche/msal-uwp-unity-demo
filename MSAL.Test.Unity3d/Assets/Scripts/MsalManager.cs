using MSAL.Client;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MsalManager : MonoBehaviour
{
    [SerializeField]
    private string clientId;
    [SerializeField]
    private TextMeshPro resultText;

    private LoginManager client;
    private string[] scopes = new string[] { "user.read" };

    // Start is called before the first frame update
    void Start()
    {
        client = new LoginManager(clientId, scopes);
    }

    private void Update()
    {
        if (Input.GetKeyDown( KeyCode.L))
        {
            OnSignIn();
        }
    }

    public async void OnSignIn()
    {
        var res = await client.AcquireToken();
        if (res.Status == LoginStatus.Success)
        {
            resultText.text = $"Token: {res.AccessToken}";
        }
        else
        {
            resultText.text = "ERROR: " + res.Error;
        }
    }
}
