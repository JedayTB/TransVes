using UnityEngine;

public class Easings
{

  /// <summary>
  /// 
  /// Exponential decay is constant.
  /// A good range is 1 - 25, slow to fast
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  /// <param name="decay">"speed" of decay</param>
  /// <param name="deltaTime"></param>
  /// <returns></returns>
  public static float ExponentialDecay(float a, float b, float decay, float deltaTime)
  {
    return b + (a - b) * Mathf.Exp(-decay * deltaTime);
  }
  public static Vector3 VexExpoDecay(Vector3 a, Vector3 b, float decay, float deltaTime)
  {
    float vx = ExponentialDecay(a.x, b.x, decay, deltaTime);
    float vy = ExponentialDecay(a.y, b.y, decay, deltaTime);
    float vz = ExponentialDecay(a.z, b.z, decay, deltaTime);

    return new Vector3(vx, vy, vz);
  }
  public static float easeInOutQuad(float x)
  {
    return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
  }

  public static float easeInOutBounce(float x)
  {
    return x < 0.5
    ? (1 - easeOutBounce(1 - 2 * x)) / 2
    : (1 + easeOutBounce(2 * x - 1)) / 2;
  }
  public static float easeOutBounce(float x)
  {
    const float n1 = 7.5625f;
    const float d1 = 2.75f;

    if (x < 1 / d1)
    {
      return n1 * x * x;
    }
    else if (x < 2 / d1)
    {
      return n1 * (x -= 1.5f / d1) * x + 0.75f;
    }
    else if (x < 2.5 / d1)
    {
      return n1 * (x -= 2.25f / d1) * x + 0.9375f;
    }
    else
    {
      return n1 * (x -= 2.625f / d1) * x + 0.984375f;
    }
  }
}
