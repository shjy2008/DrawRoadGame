//#file：./tab/battle.xlsx, sheet：normal_level，output：normal_level.cs

using System.Collections.Generic;

public class Table_normal_level
{
	public struct Data
	{
		public List<int> level;
		public int step;
		public float speedScale;

		public Data(List<int> _level = null, int _step = 0, float _speedScale = 0.0f)
		{
			level = _level;
			step = _step;
			speedScale = _speedScale;
		}
	}
	public static Dictionary<List<int>, Data> data = new Dictionary<List<int>, Data> {
		{new List<int>(new int[] { 1, 1 }), new Data(_level: new List<int>(new int[] { 1, 1 }), _step: 1, _speedScale: 1.0f)},
		{new List<int>(new int[] { 2, 2 }), new Data(_level: new List<int>(new int[] { 2, 2 }), _step: 1, _speedScale: 1.0f)},
		{new List<int>(new int[] { 3, 3 }), new Data(_level: new List<int>(new int[] { 3, 3 }), _step: 2, _speedScale: 1.0f)},
		{new List<int>(new int[] { 4, 4 }), new Data(_level: new List<int>(new int[] { 4, 4 }), _step: 4, _speedScale: 1.0f)},
		{new List<int>(new int[] { 5, 5 }), new Data(_level: new List<int>(new int[] { 5, 5 }), _step: 2, _speedScale: 1.0f)},
		{new List<int>(new int[] { 6, 10 }), new Data(_level: new List<int>(new int[] { 6, 10 }), _step: 6, _speedScale: 1.1f)},
		{new List<int>(new int[] { 11, 15 }), new Data(_level: new List<int>(new int[] { 11, 15 }), _step: 6, _speedScale: 1.1f)},
		{new List<int>(new int[] { 16, 20 }), new Data(_level: new List<int>(new int[] { 16, 20 }), _step: 6, _speedScale: 1.2f)},
		{new List<int>(new int[] { 21, 25 }), new Data(_level: new List<int>(new int[] { 21, 25 }), _step: 6, _speedScale: 1.2f)},
		{new List<int>(new int[] { 26, 30 }), new Data(_level: new List<int>(new int[] { 26, 30 }), _step: 6, _speedScale: 1.3f)},
		{new List<int>(new int[] { 31, 99999999 }), new Data(_level: new List<int>(new int[] { 31, 99999999 }), _step: 6, _speedScale: 1.3f)},
	};
}
