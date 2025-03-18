using UnityEngine;

public class ControllableTank : A_TankController
{
  public static int ControllableTankCount;
  [Header("Controllable Tank Specifics")]

  public Transform Cannon;
  public Transform SelfGuidepoint;
  public CannonProjectile ProjectilePrototype;
  [Range(0.01f, 10f)] public float decayTime = 3f;
  private int selfCountID;
  private int index;

  float angleToMouse;
  float setZAngle;
  protected override void Start()
  {
    base.Start();
    this.tag = GameStateManager.PlayerTag;
    // Because Tank Sprite is facing diff direction.. Whatev
    SpriteRend.flipX = !_flipX;

    ControllableTankCount++;

    index = ControllableTankCount % GameStateManager.Instance.Guidepoints.Length;
    SelfGuidepoint = GameStateManager.Instance.Guidepoints[index];
  }

  protected override void Update()
  {
    base.Update();
    Vector2 delta = SelfGuidepoint.position - transform.position;
    moveDirection = delta.normalized;
    AimCannon();
    shootCannon();
  }
  private void shootCannon()
  {
    if (Input.GetMouseButtonDown(0))
    {
      CannonProjectile temp = Instantiate(ProjectilePrototype, Cannon.transform.position, Cannon.transform.localRotation);
      temp.targetTag = GameStateManager.EnemyTag;
      temp.DamagePoints = GameStateManager.Instance.Player.ProjectileDamage;
    }
  }
  private void AimCannon()
  {
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0f;
    Vector3 delta = mousePos - transform.position;

    Vector3 eulerAngles = Cannon.localRotation.eulerAngles;

    angleToMouse = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;
    setZAngle = Easings.ExponentialDecay(setZAngle, angleToMouse, decayTime, Time.deltaTime);

    eulerAngles.z = setZAngle;
    Cannon.localRotation = Quaternion.Euler(eulerAngles);
  }
}
