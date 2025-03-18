using System.Collections;
using TMPro;
using UnityEngine;
public enum SpawnOffset
{
  offsetToRight,
  offsetToLeft,
  offsetToUp,
  offsetToDown
}
public class GameStateManager : MonoBehaviour
{
  public static string PlayerTag = "Player";
  public static string EnemyTag = "Enemy";

  private static GameStateManager instance;
  public static GameStateManager Instance { get { return instance; } }
  [Header("Basic setup")]
  public PlayerController Player;
  [SerializeField] private ControllableTank CtankPrototype;
  public Canvas CanvasReference;
  // North south west east in order.
  public Transform[] Guidepoints;

  private float elapsedGameplayTime = 0f;

  [SerializeField] private float CurrentScore = 0f;
  [Header("UI Setup")]
  [SerializeField] private TextMeshProUGUI spawnedUIProto;
  [SerializeField] private TextMeshProUGUI TimeText;
  [SerializeField] private TextMeshProUGUI ScoreText;

  [SerializeField] private float spawnedUIFloatTime = 3f;
  [SerializeField] private float spawnedUILerpToZeroTime = 1f;
  [SerializeField] private float floatYHeight = 2f;

  [Header("Enemy spawning")]
  [SerializeField] private EnemyController EnemyTankPrototype;
  [SerializeField] private float timeBetweenSpawns = 5f;
  [SerializeField] private int amountOfEnemiesToSpawn = 1;

  private bool continueSpawningEnemies = true;
  private float extraRndSpawnVariation = 5f;

  [Header("Difficulty and Such")]
  public int enemyProjectileDamage = -10;
  void Awake()
  {
    if (instance != null)
    {
      Destroy(this.gameObject);
    }
    else
    {
      instance = this;
    }
    StartCoroutine(EnemySpawnClock());
  }
  void Update()
  {
    UpdateAndSetTime();
    SetScoreText();
  }
  void UpdateAndSetTime()
  {
    elapsedGameplayTime += Time.deltaTime;

    float minute = elapsedGameplayTime / 60;
    float seconds = elapsedGameplayTime % 60;

    TimeText.text = $"Time: {Mathf.FloorToInt(minute):00}:{seconds:00.00}";
  }
  void SetScoreText()
  {
    ScoreText.text = $"Score: {Mathf.FloorToInt(CurrentScore):0000000}";
  }
  public void SpawnControllableTank(Vector2 position)
  {
    var temp = Instantiate(CtankPrototype, position, Quaternion.identity);
  }
  public void ScoreEvent(float scoreDelta, Vector3 position)
  {
    CurrentScore += scoreDelta;
    StartCoroutine(spawnUIAndFloat(scoreDelta, position));
  }
  IEnumerator spawnUIAndFloat(float scoreDelta, Vector3 position)
  {
    float count = 0f;
    float progress = 0f;
    TextMeshProUGUI deltaScoreText = Instantiate(spawnedUIProto);
    deltaScoreText.rectTransform.SetParent(CanvasReference.transform);

    Vector3 endPos = position;
    endPos.y += floatYHeight;

    Vector3 setPos = position;
    while (count < spawnedUIFloatTime)
    {
      count += Time.deltaTime;
      progress = count / spawnedUIFloatTime;

      progress = Easings.easeInOutQuad(progress);
      setPos = Vector3.Lerp(position, endPos, progress);

      deltaScoreText.rectTransform.position = Camera.main.WorldToScreenPoint(setPos);
      yield return null;
    }
    // Replace with score countdown
    StartCoroutine(lerpScoreTextToZero(scoreDelta, deltaScoreText, setPos));
  }
  IEnumerator lerpScoreTextToZero(float scoreDelta, TextMeshProUGUI text, Vector3 pos)
  {
    float count = 0f;
    float progress = 0f;

    float setScore = 0f;

    string sign = scoreDelta > 0f ? "+" : "-";
    while (count < spawnedUILerpToZeroTime)
    {
      count += Time.deltaTime;

      progress = count / spawnedUILerpToZeroTime;
      progress = Easings.easeInOutQuad(progress);
      setScore = Mathf.Lerp(scoreDelta, 0f, progress);

      text.rectTransform.position = Camera.main.WorldToScreenPoint(pos);

      text.text = $"{sign}{Mathf.FloorToInt(setScore)}";
      yield return null;
    }
    Destroy(text.gameObject);
  }
  IEnumerator EnemySpawnClock()
  {
    float count = 0f;

    while (count < timeBetweenSpawns)
    {
      count += Time.deltaTime;
      yield return null;
    }

    for (int i = 0; i < amountOfEnemiesToSpawn; i++)
    {
      SpawnEnemyTank();
    }
  }

  private void SpawnEnemyTank()
  {
    float offsetFromPlayerView = Camera.main.orthographicSize;
    int option = Random.Range(0, 4);
    float exVariation = Random.Range(0f, extraRndSpawnVariation);

    Vector2 playerPos = Player.transform.position;
    Vector2 offsetSpawnPos = playerPos;

    switch ((SpawnOffset)option)
    {
      case SpawnOffset.offsetToUp:
        offsetSpawnPos.y += offsetFromPlayerView + exVariation;
        offsetSpawnPos.x = Random.Range(playerPos.x - offsetFromPlayerView, playerPos.x + offsetFromPlayerView);
        break;
      case SpawnOffset.offsetToDown:
        offsetSpawnPos.y -= offsetFromPlayerView + exVariation;
        offsetSpawnPos.x = Random.Range(playerPos.x - offsetFromPlayerView, playerPos.x + offsetFromPlayerView);

        break;
      case SpawnOffset.offsetToLeft:
        offsetSpawnPos.x += offsetFromPlayerView + exVariation;
        offsetSpawnPos.y = Random.Range(playerPos.y - offsetFromPlayerView, playerPos.y + offsetFromPlayerView);

        break;
      case SpawnOffset.offsetToRight:
        offsetSpawnPos.x += offsetFromPlayerView + exVariation;
        offsetSpawnPos.y = Random.Range(playerPos.y - offsetFromPlayerView, playerPos.y + offsetFromPlayerView);

        break;
    }
    EnemyController temp = Instantiate(EnemyTankPrototype, offsetSpawnPos, Quaternion.identity);

    if (continueSpawningEnemies == true)
    {
      StartCoroutine(EnemySpawnClock());
    }
  }
}
