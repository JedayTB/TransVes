using UnityEngine;

public class ScoreObject : MonoBehaviour
{
  [Header("Score Setup")]
  [SerializeField] private float ScoreDelta = 500f;

  protected virtual void ScoreEvent()
  {
    GameStateManager.Instance.ScoreEvent(ScoreDelta, transform.position);
  }
}
