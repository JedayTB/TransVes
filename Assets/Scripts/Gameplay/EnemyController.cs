using System.Collections;
using UnityEngine;

public class EnemyController : A_TankController
{
  [Header("Cannon Setup")]
  public Transform Cannon;
  public CannonProjectile projectileProto;
  public float decayTime = 3f;
  public float timeBetweenFiringCannon = 3f;
  private bool continueFiringCannon = true;
  float angleToPlayer;
  float setZAngle;
  protected override void Start()
  {
    base.Start();
    StartCoroutine(CannonFireClock());
  }
  protected override void Update()
  {
    base.Update();
    // Because Tank Sprite is facing diff direction.. Whatev
    SpriteRend.flipX = !_flipX;
    Vector2 delta = GameStateManager.Instance.Player.transform.position - transform.position;
    moveDirection = delta.normalized;
    OrientCannon();
  }
  IEnumerator CannonFireClock()
  {
    float count = 0f;

    while (count < timeBetweenFiringCannon)
    {
      count += Time.deltaTime;
      yield return null;
    }
    FireCannon();
  }
  private void FireCannon()
  {
    CannonProjectile temp = Instantiate(projectileProto, Cannon.transform.position, Cannon.transform.localRotation);

    temp.targetTag = GameStateManager.PlayerTag;
    temp.DamagePoints = GameStateManager.Instance.enemyProjectileDamage;

    if (continueFiringCannon == true) StartCoroutine(CannonFireClock());
  }
  private void OrientCannon()
  {
    Vector3 delta = GameStateManager.Instance.Player.transform.position - transform.position;

    Vector3 eulerAngles = Cannon.localRotation.eulerAngles;

    angleToPlayer = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;
    setZAngle = Easings.ExponentialDecay(setZAngle, angleToPlayer, decayTime, Time.deltaTime);

    eulerAngles.z = setZAngle;
    Cannon.localRotation = Quaternion.Euler(eulerAngles);

  }
}
