//#file：./tab/battle.xlsx, sheet：action，output：action.cs

using System.Collections.Generic;

public class Table_action
{
	public struct Data
	{
		public string key;
		public string type;
		public float speed;
		public bool movingUp;
		public float topY;
		public float bottomY;
		public float anglePerSecond;

		public Data(string _key = "", string _type = "", float _speed = 0.0f, bool _movingUp = false, float _topY = 0.0f, float _bottomY = 0.0f, float _anglePerSecond = 0.0f)
		{
			key = _key;
			type = _type;
			speed = _speed;
			movingUp = _movingUp;
			topY = _topY;
			bottomY = _bottomY;
			anglePerSecond = _anglePerSecond;
		}
	}
	public static Dictionary<string, Data> data = new Dictionary<string, Data> {
		{"moveY_down_1", new Data(_key: "moveY_down_1", _type: "moveY", _speed: 1.0f, _movingUp: false)},
		{"moveY_up_1", new Data(_key: "moveY_up_1", _type: "moveY", _speed: 1.0f, _movingUp: true)},
		{"moveY_medium_1", new Data(_key: "moveY_medium_1", _type: "moveY", _speed: 2.0f, _movingUp: false)},
		{"moveY_hard_1", new Data(_key: "moveY_hard_1", _type: "moveY", _speed: 3.0f, _movingUp: false)},
		{"updown_easy_1", new Data(_key: "updown_easy_1", _type: "updown", _speed: 1.0f, _movingUp: false, _topY: 4.0f, _bottomY: -4.0f)},
		{"updown_medium_1", new Data(_key: "updown_medium_1", _type: "updown", _speed: 2.0f, _movingUp: false, _topY: 4.0f, _bottomY: -4.0f)},
		{"updown_hard_1", new Data(_key: "updown_hard_1", _type: "updown", _speed: 3.0f, _movingUp: false, _topY: 4.0f, _bottomY: -4.0f)},
		{"rotate_easy_1", new Data(_key: "rotate_easy_1", _type: "rotate", _anglePerSecond: 45.0f)},
		{"rotate_easy_2", new Data(_key: "rotate_easy_2", _type: "rotate", _anglePerSecond: -45.0f)},
		{"rotate_medium_1", new Data(_key: "rotate_medium_1", _type: "rotate", _anglePerSecond: 180.0f)},
		{"rotate_hard_1", new Data(_key: "rotate_hard_1", _type: "rotate", _anglePerSecond: 270.0f)},
	};
}
