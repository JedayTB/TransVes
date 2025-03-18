using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : A_TankController
{
  public int ProjectileDamage = -10;
  // Start is called before the first frame update
  protected override void Start()
  {
    base.Start();
  }

  // Update is called once per frame
  protected override void Update()
  {

    float Verticle = Input.GetAxisRaw("Vertical");
    float Horziontal = Input.GetAxisRaw("Horizontal");
    moveDirection.x = Horziontal;
    moveDirection.y = Verticle;

    moveDirection.Normalize();

    base.Update();
  }
  protected override void FixedUpdate()
  {
    base.FixedUpdate();
  }
}
