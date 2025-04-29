using System;
using UnityEngine;

public class UiUtil
{
    public UiUtil()
    {
    }

// 三次贝塞尔曲线
// 参数：初始点，中间第一个点，中间第二个点，终止点，比例（0-1）
	public static Vector2 CubicBezierCurve(Vector2 startPos, Vector2 centerPos1, Vector2 centerPos2, Vector2 endPos, float percent)
	{
		float formula(float p0, float p1, float p2, float p3, float t)
		{
			float p = Mathf.Pow(1 - t, 3) * p0 + 3 * t * Mathf.Pow(1 - t, 2) * p1 + 3 * Mathf.Pow(t, 2) * (1 - t) * p2 + Mathf.Pow(t, 3) * p3;
			return p;
		}
		float posX = formula(startPos.x, centerPos1.x, centerPos2.x, endPos.x, percent);
		float posY = formula(startPos.y, centerPos1.y, centerPos2.y, endPos.y, percent);
		return new Vector2(posX, posY);
	}

	public static Vector2 QuadraticBezierCurve(Vector2 startPos, Vector2 centerPos, Vector2 endPos, float percent)
	{
		float formula(float p0, float p1, float p2, float t)
		{
			float p = Mathf.Pow(1 - t, 2) * p0 + 2 * t * (1 - t) * p1 + Mathf.Pow(t, 2) * p2;
			return p;
		}
		float posX = formula(startPos.x, centerPos.x, endPos.x, percent);
		float posY = formula(startPos.y, centerPos.y, endPos.y, percent);
		return new Vector2(posX, posY);
	}

    // Length of an animation
	public static float GetAnimLength(GameObject gameObject, string animName)
	{
		AnimationClip[] clips = gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips;
		foreach (var item in clips)
		{
			if (item.name == animName)
			{
				return item.length;
			}
		}
		return 0.0f;
	}
}
