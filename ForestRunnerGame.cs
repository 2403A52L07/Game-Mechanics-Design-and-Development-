using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Attach this ONE script to the Player GameObject.
// Required on Player: CharacterController and Tag = Player.
// Coin objects: Tag = Coin or Emerald, Collider with Is Trigger enabled.
// Obstacles: Tag = Obstacle, Collider with Is Trigger enabled.
[RequireComponent(typeof(CharacterController))]
public class ForestRunnerGame : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float laneDistance = 2.6f;
    [SerializeField] private float laneChangeSpeed = 12f;
    [SerializeField] private float jumpHeight = 2.2f;
    [SerializeField] private float gravity = -25f;

    [Header("Optional UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text coinText;
    [SerializeField] private Text distanceText;
    [SerializeField] private Text bestText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Optional Endless Road")]
    [SerializeField] private GameObject[] roadSegmentPrefabs;
    [SerializeField] private float segmentLength = 30f;
    [SerializeField] private int segmentsAhead = 5;
    [SerializeField] private int initialSegments = 6;

    private CharacterController controller;
    private int currentLane = 1; // 0 = left, 1 = centre, 2 = right
    private int coins;
    private float verticalVelocity;
    private float startZ;
    private float distance;
    private float nextSegmentZ;
    private bool isPlaying = true;

    private const string BestDistanceKey = "ForestRunner_BestDistance";

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        startZ = transform.position.z;
        nextSegmentZ = startZ;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        for (int i = 0; i < initialSegments; i++)
            SpawnRoadSegment();

        UpdateUI();
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        HandleMovement();
        UpdateDistance();
        UpdateUI();
        UpdateRoadSegments();
    }

    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            currentLane = Mathf.Max(0, currentLane - 1);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            currentLane = Mathf.Min(2, currentLane + 1);

        float targetX = (currentLane - 1) * laneDistance;
        float nextX = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime
        );

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 movement = new Vector3(
            (nextX - transform.position.x) / Mathf.Max(Time.deltaTime, 0.0001f),
            verticalVelocity,
            forwardSpeed
        );

        controller.Move(movement * Time.deltaTime);
    }

    private void UpdateDistance()
    {
        distance = Mathf.Max(0f, transform.position.z - startZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlaying)
            return;

        if (other.CompareTag("Coin"))
        {
            coins += 1;
            Destroy(other.gameObject);
            UpdateUI();
        }
        else if (other.CompareTag("Emerald"))
        {
            coins += 5;
            Destroy(other.gameObject);
            UpdateUI();
        }
        else if (other.CompareTag("Obstacle"))
        {
            EndRun();
        }
    }

    private void EndRun()
    {
        isPlaying = false;

        int currentDistance = Mathf.RoundToInt(distance);
        int bestDistance = PlayerPrefs.GetInt(BestDistanceKey, 0);

        if (currentDistance > bestDistance)
        {
            PlayerPrefs.SetInt(BestDistanceKey, currentDistance);
            PlayerPrefs.Save();
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        UpdateUI();
    }

    private void UpdateUI()
    {
        int score = Mathf.RoundToInt(distance) + coins;
        int best = PlayerPrefs.GetInt(BestDistanceKey, 0);

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (coinText != null)
            coinText.text = "Coins: " + coins;

        if (distanceText != null)
            distanceText.text = "Distance: " + Mathf.RoundToInt(distance) + " m";

        if (bestText != null)
            bestText.text = "Best: " + best + " m";
    }

    private void SpawnRoadSegment()
    {
        if (roadSegmentPrefabs == null || roadSegmentPrefabs.Length == 0)
            return;

        GameObject prefab = roadSegmentPrefabs[Random.Range(0, roadSegmentPrefabs.Length)];
        Instantiate(prefab, new Vector3(0f, 0f, nextSegmentZ), Quaternion.identity);
        nextSegmentZ += segmentLength;
    }

    private void UpdateRoadSegments()
    {
        if (roadSegmentPrefabs == null || roadSegmentPrefabs.Length == 0 ||
            segmentLength <= 0f)
            return;

        while (nextSegmentZ < transform.position.z + segmentLength * segmentsAhead)
            SpawnRoadSegment();
    }

    // Connect the Retry button's OnClick event to this method.
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Optional: connect Pause and Resume buttons to these methods.
    public void PauseGame()
    {
        if (!isPlaying)
            return;

        isPlaying = false;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPlaying = true;
    }
}
