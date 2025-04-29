using System;
using UnityEngine;

public struct CircleArea
{
    public Vector2 o;
    public float r;

    public CircleArea(Vector2 _o, float _r)
    {
        o = _o;
        r = _r;
    }
}

//rectangle
class RectParams
{
	public float w;
	public float h;
	public float x;
	public float y;
	public float radian;

    public RectParams(float _w, float _h, float _x, float _y, float _radian)
    {
		w = _w;
		h = _h;
		x = _x;
		y = _y;
		radian = _radian;
    }
};

class RectParamLine
{
	public Vector2 pos1;
	public Vector2 pos2;
	public float width;

    public RectParamLine(Vector2 _pos1, Vector2 _pos2, float _width)
    {
		pos1 = _pos1;
		pos2 = _pos2;
		width = _width;
    }
}

class Rectangle
{
	public void SetParams(RectParams rect)
	{
		_w = rect.w;
		_h = rect.h;
		_x = rect.x;
		_y = rect.y;
		_radian = rect.radian;
		CalcPointsByParams();
	}

    public void SetParamLine(RectParamLine rectline)
	{
		Vector2 pos1 = rectline.pos1;
		Vector2 pos2 = rectline.pos2;
        Vector2 posMid = (pos1 + pos2) / 2.0f;
		Vector2 diff = pos2 - pos1;
		RectParams param = new RectParams(diff.magnitude, rectline.width, posMid.x, posMid.y, Mathf.Atan2(diff.y, diff.x));
		SetParams(param);
	}

	public void CalcPointsByParams()
	{
		float x1 = -1 * _w / 2.0f;
		float y1 = _h / 2.0f;

		float x2 = -1 * _w / 2.0f;
		float y2 = -1 * _h / 2.0f;

		float x3 = _w / 2.0f;
		float y3 = -1 * _h / 2.0f;

		float x4 = _w / 2.0f;
		float y4 = _h / 2.0f;

		//rotate first
		_x1 = x1 * Mathf.Cos(_radian) - y1 * Mathf.Sin(_radian);
		_y1 = x1 * Mathf.Sin(_radian) + y1 * Mathf.Cos(_radian);

		_x2 = x2 * Mathf.Cos(_radian) - y2 * Mathf.Sin(_radian);
		_y2 = x2 * Mathf.Sin(_radian) + y2 * Mathf.Cos(_radian);

		_x3 = x3 * Mathf.Cos(_radian) - y3 * Mathf.Sin(_radian);
		_y3 = x3 * Mathf.Sin(_radian) + y3 * Mathf.Cos(_radian);

		_x4 = x4 * Mathf.Cos(_radian) - y4 * Mathf.Sin(_radian);
		_y4 = x4 * Mathf.Sin(_radian) + y4 * Mathf.Cos(_radian);

		//translate 
		_x1 += _x;
		_x2 += _x;
		_x3 += _x;
		_x4 += _x;

		_y1 += _y;
		_y2 += _y;
		_y3 += _y;
		_y4 += _y;
	}

	public bool IsPointInRect(float x, float y)
	{
		return MathKit.IsCircleCrossRect(x, y, 0, _x, _y, _w, _h, _radian);
	}

	public float _w;
	public float _h;
	public float _x;
	public float _y;
	public float _radian;

	//left top
	public float _x1;
	public float _y1;
	//left bottom
	public float _x2;
	public float _y2;
	//right bottom
	public float _x3;
	public float _y3;
	//right top
	public float _x4;
	public float _y4;
};

public class MathKit
{
    public MathKit()
    {
    }

    public static bool IsCircleCrossCircle(Vector2 center1, float r1, Vector2 center2, float r2)
    {
		Vector2 diff = center1 - center2;
		return diff.magnitude < (r1 + r2);
    }

    public static bool IsPointInCircle(Vector2 point, Vector2 circleCenter, float r)
    {
		return IsCircleCrossCircle(point, 0, circleCenter, r);
    }

    public static bool IsCircleCrossLineSeg(float x, float y, float r, float x1, float y1, float x2, float y2)
    {
		float vx1 = x - x1;
		float vy1 = y - y1;
		float vx2 = x2 - x1;
		float vy2 = y2 - y1;

		// len = v2.length()  
		float len = Mathf.Sqrt(vx2 * vx2 + vy2 * vy2);

		// v2.normalize()  
		vx2 /= len;
		vy2 /= len;

		// u = v1.dot(v2)  
		// u is the vector projection length of vector v1 onto vector v2.  
		float u = vx1 * vx2 + vy1 * vy2;

		// determine the nearest point on the lineseg  
		float x0 = 0;
		float y0 = 0;
		if (u <= 0)
		{
			// p is on the left of p1, so p1 is the nearest point on lineseg  
			x0 = x1;
			y0 = y1;
		}
		else if (u >= len)
		{
			// p is on the right of p2, so p2 is the nearest point on lineseg  
			x0 = x2;
			y0 = y2;
		}
		else
		{
			// p0 = p1 + v2 * u  
			// note that v2 is already normalized.  
			x0 = x1 + vx2 * u;
			y0 = y1 + vy2 * u;
		}

		return (x - x0) * (x - x0) + (y - y0) * (y - y0) <= r * r;
	}

	public static bool IsCircleCrossLineSeg(float cx, float cy, float radius, Vector2 pos1, Vector2 pos2, float lineWidth)
	{
		Vector2 posMid = (pos1 + pos2) / 2.0f;
		Vector2 diff = pos2 - pos1;
		return IsCircleCrossRect(cx, cy, radius, posMid.x, posMid.y, diff.magnitude, lineWidth, Mathf.Atan2(diff.y, diff.x));
	}

	public static bool IsLineSegCrossLineSeg(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
	{
		if (!(Mathf.Min(x1, x2) <= Mathf.Max(x3, x4) && Mathf.Min(y3, y4) <= Mathf.Max(y1, y2) && Mathf.Min(x3, x4) <= Mathf.Max(x1, x2) && Mathf.Min(y1, y2) <= Mathf.Max(y3, y4)))
			return false;

		float u, v, w, z;//分别记录两个向量
		u = (x3 - x1) * (y2 - y1) - (x2 - x1) * (y3 - y1);
		v = (x4 - x1) * (y2 - y1) - (x2 - x1) * (y4 - y1);
		w = (x1 - x3) * (y4 - y3) - (x4 - x3) * (y1 - y3);
		z = (x2 - x3) * (y4 - y3) - (x4 - x3) * (y2 - y3);
		return (u * v <= 0.00001f && w * z <= 0.00001f);
	}

    public static bool IsLineSegCrossLineSeg(Vector2 line1_pos1, Vector2 line1_pos2, float line1Width, Vector2 line2_pos1, Vector2 line2_pos2, float line2Width)
    {
        Rectangle rect1 = new Rectangle();
		rect1.SetParamLine(new RectParamLine(line1_pos1, line1_pos2, line1Width));
		Rectangle rect2 = new Rectangle();
		rect2.SetParamLine(new RectParamLine(line2_pos1, line2_pos2, line2Width));

		// check AABB first
		float rect1_minX = Mathf.Min(rect1._x1, rect1._x2, rect1._x3, rect1._x4);
		float rect1_maxX = Mathf.Max(rect1._x1, rect1._x2, rect1._x3, rect1._x4);
		float rect1_minY = Mathf.Min(rect1._y1, rect1._y2, rect1._y3, rect1._y4);
		float rect1_maxY = Mathf.Max(rect1._y1, rect1._y2, rect1._y3, rect1._y4);
		float rect2_minX = Mathf.Min(rect2._x1, rect2._x2, rect2._x3, rect2._x4);
		float rect2_maxX = Mathf.Max(rect2._x1, rect2._x2, rect2._x3, rect2._x4);
		float rect2_minY = Mathf.Min(rect2._y1, rect2._y2, rect2._y3, rect2._y4);
		float rect2_maxY = Mathf.Max(rect2._y1, rect2._y2, rect2._y3, rect2._y4);
		bool aabbCrossX = rect1_minX < rect2_maxX && rect1_maxX > rect2_minX;
		bool aabbCrossY = rect1_minY < rect2_maxY && rect1_maxY > rect2_minY;
		if (!(aabbCrossX && aabbCrossY))
			return false;

		// line cross line
		float[] rect1_x_list = new float[] { rect1._x1, rect1._x2, rect1._x3, rect1._x4 };
		float[] rect1_y_list = new float[] { rect1._y1, rect1._y2, rect1._y3, rect1._y4 };
		float[] rect2_x_list = new float[] { rect2._x1, rect2._x2, rect2._x3, rect2._x4 };
		float[] rect2_y_list = new float[] { rect2._y1, rect2._y2, rect2._y3, rect2._y4 };
		for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 4; ++j)
			{
				int i_prev = i - 1;
				if (i_prev < 0)
					i_prev = 3;
				int j_prev = j - 1;
                if (j_prev < 0)
                    j_prev = 3;
				if (IsLineSegCrossLineSeg(rect1_x_list[i], rect1_y_list[i], rect1_x_list[i_prev], rect1_y_list[i_prev],
                    rect2_x_list[j], rect2_y_list[j], rect2_x_list[j_prev], rect2_y_list[j_prev]))
                {
					return true;
                }
            }
        }

        // point in rect
        for (int i = 0; i < 4; ++i)
        {
			if (IsPointInRect(rect1_x_list[i], rect1_y_list[i], rect2._x, rect2._y, rect2._w, rect2._h, rect2._radian))
				return true;
			if (IsPointInRect(rect2_x_list[i], rect2_y_list[i], rect1._x, rect1._y, rect1._w, rect1._h, rect1._radian))
				return true;
		}
		return false;
	}

    public static void Rotate_vec(float x0, float y0, float radian, out float x, out float y)
	{
		x = x0 * Mathf.Cos(radian) - y0 * Mathf.Sin(radian);
		y = x0 * Mathf.Sin(radian) + y0 * Mathf.Cos(radian);
	}

	public static bool IsCircleCrossRect(float cx, float cy, float radius, float rx, float ry, float w, float h, float rot_y)
	{
		float x, y;
		Rotate_vec(cx - rx, cy - ry, -rot_y, out x, out y);
		x = Mathf.Abs(x);
		y = Mathf.Abs(y);
		float p1_x = w / 2.0f;
		float p1_y = h / 2.0f;
		float vec_x = Mathf.Max(x - p1_x, 0);
		float vec_y = Mathf.Max(y - p1_y, 0);
		return vec_x * vec_x + vec_y * vec_y <= radius * radius;
	}

	public static bool IsPointInRect(float x, float y, float rect_cx, float rect_cy, float w, float h, float rect_rot_y)
	{
		return IsCircleCrossRect(x, y, 0, rect_cx, rect_cy, w, h, rect_rot_y);
	}
}
