using UnityEngine;

public class CameraController : MonoBehaviour
{
  [Range(0.1f, 5f)]
  public float minDecaySpeed = 1f;

  [Range(5, 15f)]
  public float maxDecaySpeed = 5f;
  public float maxDistanceFromCam = 5f;
  private float DecaySpeed = 5f;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  void FixedUpdate()
  {
    Vector3 plPos = GameStateManager.Instance.Player.transform.position;
    plPos.z = -10f;

    Vector3 delta = plPos - transform.position;
    float progress = Mathf.Clamp01(delta.magnitude / maxDistanceFromCam);
    DecaySpeed = Mathf.Lerp(minDecaySpeed, maxDecaySpeed, progress);

    transform.position = Easings.VexExpoDecay(transform.position, plPos, DecaySpeed, Time.fixedDeltaTime);
  }
}
