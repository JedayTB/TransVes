using UnityEngine;

public class Pickup : ScoreObject
{

  protected virtual void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == GameStateManager.PlayerTag)
    {
      GameStateManager.Instance.SpawnControllableTank(transform.position);
      ScoreEvent();
      Destroy(this.gameObject);
    }
  }
}
