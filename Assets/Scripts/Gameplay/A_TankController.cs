using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class A_TankController : ScoreObject
{
  [Header("Basic Setup")]

  private float damageVisTime = 0.75f;
  private float fadeoutHealthSliderTime = 1.5f;

  private int baseHealth;
  [SerializeField] private int HealthPoints = 100;
  public SpriteRenderer SpriteRend;
  public Rigidbody2D rb;

  [SerializeField] Slider HpSliderProto;
  [SerializeField] Transform HpSliderPosition;

  private Slider HealthSlider;
  [SerializeField]
  private Image healthFillArea;
  [SerializeField] private Gradient HealthGradient;

  Coroutine fadeOutHealthSliderCOROUTINE;
  Coroutine damageVisualizationCOROUTINE;

  [Header("Sprite Settings")]
  protected bool _flipX;
  [Header("Movement")]
  public float AccelerationStrength = 4f;
  public float MaxSpeed = 7f;
  [Range(0f, 3f)] public float LinearDrag = 1.5f;
  protected Vector2 currentVelocity;
  public Vector2 moveDirection;

  // Start is called before the first frame update
  protected virtual void Start()
  {
    baseHealth = HealthPoints;
    SpriteRend = GetComponentInChildren<SpriteRenderer>();
    rb = GetComponent<Rigidbody2D>();
    rb.drag = LinearDrag;
    rb.gravityScale = 0f;

    HealthSlider = Instantiate(HpSliderProto);
    HealthSlider.transform.SetParent(GameStateManager.Instance.CanvasReference.transform);

    HealthSlider.minValue = 0f;
    HealthSlider.maxValue = baseHealth;
    // Jank, but should give me the fill image of the fill rect
    healthFillArea = HealthSlider.fillRect.GetComponentInChildren<Image>();
    // Repurpose this.
    damageVisualizationCOROUTINE = StartCoroutine(damageVisualization(1, 100, 2f));
  }

  IEnumerator damageVisualization(int hpBefore, int hpAfter, float damageVisTime)
  {
    float count = 0f;
    float progress = 0f;
    float colourProgress = 0f;

    float baseHP = HealthSlider.value;

    float hpBeforeV = (float)hpBefore / baseHealth;
    float hpAfterV = (float)hpAfter / baseHealth;

    while (count < damageVisTime)
    {
      count += Time.deltaTime;
      progress = count / damageVisTime;

      progress = Easings.easeInOutBounce(progress);

      colourProgress = Mathf.Lerp(hpBeforeV, hpAfterV, progress);
      HealthSlider.value = Mathf.Lerp(hpBefore, hpAfter, progress);
      healthFillArea.color = HealthGradient.Evaluate(colourProgress);
      yield return null;
    }
    if (fadeOutHealthSliderCOROUTINE != null)
    {
      StopCoroutine(fadeOutHealthSliderCOROUTINE);
    }

    fadeOutHealthSliderCOROUTINE = StartCoroutine(fadeoutHealthSlider((fadeoutHealthSliderTime)));
    damageVisualizationCOROUTINE = null;
  }
  IEnumerator fadeoutHealthSlider(float time)
  {
    float count = 0f;
    float progress = 0f;
    // Health Slider "target graphic" is set to the exterior sprite.
    // So we Must Change the interior sprite as well "healthFillArea"
    Color BaseColor = healthFillArea.color;
    Color fadeOutColor = healthFillArea.color;
    fadeOutColor.a = 0f;
    while (count < time)
    {
      count += Time.deltaTime;
      progress = count / time;
      progress = Easings.easeInOutQuad(progress);

      HealthSlider.image.color = Color.Lerp(BaseColor, fadeOutColor, progress);
      healthFillArea.color = Color.Lerp(BaseColor, fadeOutColor, progress);
      yield return null;
    }
    fadeOutHealthSliderCOROUTINE = null;
  }
  public void DeltaHealth(int healthDelta)
  {
    int beforeHP = HealthPoints;
    HealthPoints += healthDelta;
    if (damageVisualizationCOROUTINE != null)
    {
      StopCoroutine(damageVisualizationCOROUTINE);
    }
    damageVisualizationCOROUTINE = StartCoroutine(damageVisualization(beforeHP, HealthPoints, damageVisTime));
    if (HealthPoints <= 0) OnAllHealthLost();
  }
  protected virtual void OnAllHealthLost()
  {
    Destroy(HealthSlider.gameObject);
    Destroy(this.gameObject);
  }
  // Update is called once per frame
  protected virtual void Update()
  {
    _flipX = moveDirection.x >= 0f ? false : true;
    SpriteRend.flipX = _flipX;

    HealthSlider.transform.position = Camera.main.WorldToScreenPoint(HpSliderPosition.position);
  }
  protected virtual void FixedUpdate()
  {
    rb.AddForce(moveDirection * AccelerationStrength, ForceMode2D.Force);

    currentVelocity = rb.velocity;

    currentVelocity.y = Mathf.Clamp(currentVelocity.y, -MaxSpeed, MaxSpeed);
    currentVelocity.x = Mathf.Clamp(currentVelocity.x, -MaxSpeed, MaxSpeed);

  }
}
