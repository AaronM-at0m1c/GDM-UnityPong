using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.PackageManager;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class GameManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject LPaddle;
    [SerializeField] private NetworkObject RPaddle;
    [SerializeField] private GameObject ball;

    [SerializeField] private TextMeshProUGUI leftScoreText;
    [SerializeField] private TextMeshProUGUI rightScoreText;
    [SerializeField] private TextMeshProUGUI winMessageText;
    [SerializeField] private Button startButton;

    private NetworkVariable<int> leftScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> rightScore = new NetworkVariable<int>(0);
    private NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false);
    private const int pointsToWin = 5;

// Initialize game setup and listen for "start game" button to be pressed
    public override void OnNetworkSpawn()
    {
        leftScore.OnValueChanged += (oldVal, newVal) => UpdateScoreUI();
        rightScore.OnValueChanged += (oldVal, newVal) => UpdateScoreUI();
        gameOver.OnValueChanged += (oldVal, newVal) => UpdateWinUI();

        winMessageText.gameObject.SetActive(false);
        UpdateScoreUI();

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            startButton.gameObject.SetActive(IsHost);

        }
    }

    // When start game button is pressed, start the game
    public void StartGame()
    {
        if (!IsServer) return;

        leftScore.Value = 0;
        rightScore.Value = 0;
        gameOver.Value = false;
        Debug.Log("Button Clicked");

        startButton.gameObject.SetActive(false);

        ResetBall(Vector2.right);
    }

    // Function to update right player score and check if they have enough points to win
    public void RightPlayerScored()
    {
        if (!IsServer || gameOver.Value) return;

        rightScore.Value++;
        CheckWinCondition();

        if (!gameOver.Value)
            ResetBall(Vector2.left);
    }

        // Function to update left player score and check if they have enough points to win
    public void LeftPlayerScored()
    {
        if (!IsServer || gameOver.Value) return;

        leftScore.Value++;
        CheckWinCondition();

        if (!gameOver.Value)
            ResetBall(Vector2.right);
    }

    void Start()
    {
        if (IsServer)
        {
            StartCoroutine(AssignOwnershipAfterSpawn());
        }
    }

    // Assign ownership of paddles
    private IEnumerator AssignOwnershipAfterSpawn()
    {
        yield return new WaitForSeconds(1f);

        if (LPaddle != null && LPaddle.IsSpawned)
        {
            LPaddle.ChangeOwnership(NetworkManager.ServerClientId);
            Debug.Log("Assigned LPaddle to host");
        }
        else
        {
            Debug.Log("Error, LPaddle not spawned");    
        }
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        private void OnClientConnected(ulong clientId)
    {
        if (!IsServer) return;

        if (clientId == NetworkManager.ServerClientId) return;
        Debug.Log($"Client {clientId} connected, assigning RPaddle");

        StartCoroutine(AssignRPaddleToClient(clientId));
    }

    private IEnumerator AssignRPaddleToClient(ulong clientId)
    {
        yield return new WaitForSeconds(0.5f);

        if (RPaddle != null && RPaddle.IsSpawned)
        {
            RPaddle.ChangeOwnership(clientId);
            Debug.Log("Assigned RPaddle to client"); 
            }
            else
            {
                Debug.Log("Error, RPaddle not assigned.");
        }
                
    }

    // Function to update the score UI on the canvas
    private void UpdateScoreUI()
    {
        leftScoreText.text = leftScore.Value.ToString();
        rightScoreText.text = rightScore.Value.ToString();
    }

    // Function to update the win UI on the canvas    
    private void UpdateWinUI()
    {
        if (gameOver.Value)
        {
            winMessageText.gameObject.SetActive(true);
            if (leftScore.Value >= pointsToWin)
                winMessageText.text = "Left Player Wins!";
            else
                winMessageText.text = "Right Player Wins!";
        }
        else
        {
            winMessageText.gameObject.SetActive(false);
        }

    }

    // Reset ball function to be called after a player scores
    private void ResetBall(Vector2 serveDirection)
    {
        if (ball == null) return;
        ball.transform.position = Vector3.zero;
        BallMovement bm = ball.GetComponent<BallMovement>();
        if (bm != null)
            bm.Launch(serveDirection);
    }

    // Check if player score = 5
    private void CheckWinCondition()
    {
        if (leftScore.Value >= pointsToWin || rightScore.Value >= pointsToWin)
        {
            gameOver.Value = true;

            if (ball != null)
                ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
