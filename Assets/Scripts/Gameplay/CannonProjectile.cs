using UnityEngine;

public class CannonProjectile : MonoBehaviour
{
  public float lifetime = 2f;
  public float projectilespeed = 15f;
  public int DamagePoints = -10;

  public string targetTag;
  void Start()
  {
    Destroy(this.gameObject, lifetime);
  }

  void Update()
  {
    transform.position += projectilespeed * Time.deltaTime * transform.up;
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag(targetTag))
    {
      var tankController = other.gameObject.GetComponent<A_TankController>();
      tankController.DeltaHealth(DamagePoints);
      Destroy(this.gameObject);
    }
  }
}
