//#file：./tab/shop.xlsx, sheet：shop_road，output：shop_road.cs

using System.Collections.Generic;

public class Table_shop_road
{
	public struct Data
	{
		public int key;
		public int achieveType;
		public string path;
		public string path_circle;
		public int imageType;
		public int param;
		public string desc;

		public Data(int _key = 0, int _achieveType = 0, string _path = "", string _path_circle = "", int _imageType = 0, int _param = 0, string _desc = "")
		{
			key = _key;
			achieveType = _achieveType;
			path = _path;
			path_circle = _path_circle;
			imageType = _imageType;
			param = _param;
			desc = _desc;
		}
	}
	public static Dictionary<int, Data> data = new Dictionary<int, Data> {
		{1001, new Data(_key: 1001, _achieveType: 0, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 0, _desc: "Pass level {0}")},
		{1002, new Data(_key: 1002, _achieveType: 0, _path: "Road_2", _path_circle: "Road_2_circle", _imageType: 0, _param: 10, _desc: "Pass level {0}")},
		{1003, new Data(_key: 1003, _achieveType: 0, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 30, _desc: "Pass level {0}")},
		{1004, new Data(_key: 1004, _achieveType: 0, _path: "Road_2", _path_circle: "Road_2_circle", _imageType: 0, _param: 50, _desc: "Pass level {0}")},
		{1005, new Data(_key: 1005, _achieveType: 0, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 80, _desc: "Pass level {0}")},
		{1006, new Data(_key: 1006, _achieveType: 0, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 100, _desc: "Pass level {0}")},
		{2001, new Data(_key: 2001, _achieveType: 1, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 100, _desc: "Score {0} or more in an endless game")},
		{2002, new Data(_key: 2002, _achieveType: 1, _path: "Road_2", _path_circle: "Road_2_circle", _imageType: 0, _param: 500, _desc: "Score {0} or more in an endless game")},
		{2003, new Data(_key: 2003, _achieveType: 1, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 2000, _desc: "Score {0} or more in an endless game")},
		{3001, new Data(_key: 3001, _achieveType: 2, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 2, _desc: "Login for {0} days")},
		{3002, new Data(_key: 3002, _achieveType: 2, _path: "Road_2", _path_circle: "Road_2_circle", _imageType: 0, _param: 3, _desc: "Login for {0} days")},
		{3003, new Data(_key: 3003, _achieveType: 2, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 5, _desc: "Login for {0} days")},
		{3004, new Data(_key: 3004, _achieveType: 2, _path: "Road_3", _path_circle: "Road_3_circle", _imageType: 0, _param: 7, _desc: "Login for {0} days")},
	};
}
