//#file：./tab/shop.xlsx, sheet：shop_ball，output：shop_ball.cs

using System.Collections.Generic;

public class Table_shop_ball
{
	public struct Data
	{
		public int key;
		public int type;
		public string path;
		public bool rotate;

		public Data(int _key = 0, int _type = 0, string _path = "", bool _rotate = false)
		{
			key = _key;
			type = _type;
			path = _path;
			rotate = _rotate;
		}
	}
	public static Dictionary<int, Data> data = new Dictionary<int, Data> {
		{1001, new Data(_key: 1001, _type: 0, _path: "Car3D", _rotate: false)},
		{1002, new Data(_key: 1002, _type: 0, _path: "Car3D", _rotate: false)},
		{1003, new Data(_key: 1003, _type: 0, _path: "Car3D", _rotate: false)},
		{1004, new Data(_key: 1004, _type: 0, _path: "Car3D", _rotate: false)},
		{1005, new Data(_key: 1005, _type: 0, _path: "Car3D", _rotate: false)},
		{1006, new Data(_key: 1006, _type: 0, _path: "Car3D", _rotate: false)},
		{1007, new Data(_key: 1007, _type: 0, _path: "Car3D", _rotate: false)},
		{1008, new Data(_key: 1008, _type: 0, _path: "Car3D", _rotate: false)},
		{1009, new Data(_key: 1009, _type: 0, _path: "Car3D", _rotate: false)},
		{2001, new Data(_key: 2001, _type: 1, _path: "Car3D", _rotate: false)},
		{2002, new Data(_key: 2002, _type: 1, _path: "Car3D", _rotate: false)},
		{2003, new Data(_key: 2003, _type: 1, _path: "Car3D", _rotate: false)},
		{2004, new Data(_key: 2004, _type: 1, _path: "Car3D", _rotate: false)},
		{2005, new Data(_key: 2005, _type: 1, _path: "Car3D", _rotate: false)},
		{2006, new Data(_key: 2006, _type: 1, _path: "Car3D", _rotate: false)},
		{2007, new Data(_key: 2007, _type: 1, _path: "Car3D", _rotate: false)},
		{2008, new Data(_key: 2008, _type: 1, _path: "Car3D", _rotate: false)},
		{2009, new Data(_key: 2009, _type: 1, _path: "Car3D", _rotate: false)},
		{3001, new Data(_key: 3001, _type: 2, _path: "Car3D", _rotate: false)},
		{3002, new Data(_key: 3002, _type: 2, _path: "Car3D", _rotate: false)},
		{3003, new Data(_key: 3003, _type: 2, _path: "Car3D", _rotate: false)},
		{3004, new Data(_key: 3004, _type: 2, _path: "Car3D", _rotate: false)},
		{3005, new Data(_key: 3005, _type: 2, _path: "Car3D", _rotate: false)},
		{3006, new Data(_key: 3006, _type: 2, _path: "Car3D", _rotate: false)},
		{3007, new Data(_key: 3007, _type: 2, _path: "Car3D", _rotate: false)},
		{3008, new Data(_key: 3008, _type: 2, _path: "Car3D", _rotate: false)},
		{3009, new Data(_key: 3009, _type: 2, _path: "Car3D", _rotate: false)},
		{4001, new Data(_key: 4001, _type: 3, _path: "Car3D", _rotate: false)},
		{4002, new Data(_key: 4002, _type: 3, _path: "Car3D", _rotate: false)},
		{4003, new Data(_key: 4003, _type: 3, _path: "Car3D", _rotate: false)},
		{4004, new Data(_key: 4004, _type: 3, _path: "Car3D", _rotate: false)},
		{4005, new Data(_key: 4005, _type: 3, _path: "Car3D", _rotate: false)},
		{4006, new Data(_key: 4006, _type: 3, _path: "Car3D", _rotate: false)},
		{4007, new Data(_key: 4007, _type: 3, _path: "Car3D", _rotate: false)},
		{4008, new Data(_key: 4008, _type: 3, _path: "Car3D", _rotate: false)},
		{4009, new Data(_key: 4009, _type: 3, _path: "Car3D", _rotate: false)},
	};
}
