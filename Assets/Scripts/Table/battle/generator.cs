//#file：./tab/battle.xlsx, sheet：generator，output：generator.cs

using System.Collections.Generic;

public class Table_generator
{
	public struct Data
	{
		public string key;
		public int appearLevel;
		public int maxLevel;
		public int appearType;
		public string type;
		public float distance;
		public string action1;
		public string action2;
		public string coinGenerator;
		public float deltaX;
		public float radius;
		public float width;
		public float height;
		public float gapHeight;
		public float frequencyScale;
		public float amplitudeScale;
		public bool isCos;
		public float posY;
		public int coinCountY;
		public bool noCoin;

		public Data(string _key = "", int _appearLevel = 0, int _maxLevel = 0, int _appearType = 0, string _type = "", float _distance = 0.0f, string _action1 = "", string _action2 = "", string _coinGenerator = "", float _deltaX = 0.0f, float _radius = 0.0f, float _width = 0.0f, float _height = 0.0f, float _gapHeight = 0.0f, float _frequencyScale = 0.0f, float _amplitudeScale = 0.0f, bool _isCos = false, float _posY = 0.0f, int _coinCountY = 0, bool _noCoin = false)
		{
			key = _key;
			appearLevel = _appearLevel;
			maxLevel = _maxLevel;
			appearType = _appearType;
			type = _type;
			distance = _distance;
			action1 = _action1;
			action2 = _action2;
			coinGenerator = _coinGenerator;
			deltaX = _deltaX;
			radius = _radius;
			width = _width;
			height = _height;
			gapHeight = _gapHeight;
			frequencyScale = _frequencyScale;
			amplitudeScale = _amplitudeScale;
			isCos = _isCos;
			posY = _posY;
			coinCountY = _coinCountY;
			noCoin = _noCoin;
		}
	}
	public static Dictionary<string, Data> data = new Dictionary<string, Data> {
		{"updownrect_newbie", new Data(_key: "updownrect_newbie", _appearLevel: 1, _maxLevel: 1, _appearType: 1, _type: "updownrect", _distance: 30.0f, _coinGenerator: "updownrect_newbie_coin", _deltaX: 99999.0f, _width: 30.0f, _gapHeight: 10.0f, _posY: 0.0f, _noCoin: true)},
		{"updownrect_newbie_coin", new Data(_key: "updownrect_newbie_coin", _type: "coin", _deltaX: 1.0f, _frequencyScale: 0.5f, _amplitudeScale: 1.2f, _posY: 0.0f, _coinCountY: 3)},
		{"wave_newbie", new Data(_key: "wave_newbie", _appearLevel: 2, _maxLevel: 2, _appearType: 1, _type: "wave", _distance: 30.0f, _coinGenerator: "wave_newbie_coin", _width: 0.5f, _gapHeight: 8.0f, _frequencyScale: 0.5f, _amplitudeScale: 1.2f, _posY: 0.0f)},
		{"wave_newbie_coin", new Data(_key: "wave_newbie_coin", _type: "coin", _deltaX: 1.0f, _frequencyScale: 0.45f, _amplitudeScale: 1.2f, _posY: 0.0f, _coinCountY: 3)},
		{"long_rotate_rect_1", new Data(_key: "long_rotate_rect_1", _appearLevel: 3, _maxLevel: 4, _appearType: 1, _type: "rect", _distance: 8.0f, _action1: "rotate_easy_1", _coinGenerator: "long_rotate_rect_1_coin", _deltaX: 99999.0f, _width: 10.0f, _height: 0.5f, _posY: 0.0f)},
		{"long_rotate_rect_1_coin", new Data(_key: "long_rotate_rect_1_coin", _type: "coin", _deltaX: 1.0f, _frequencyScale: 0.0f, _amplitudeScale: 0.0f, _posY: -2.0f, _coinCountY: 3)},
		{"long_rotate_rect_2", new Data(_key: "long_rotate_rect_2", _appearLevel: 4, _maxLevel: 4, _appearType: 1, _type: "rect", _distance: 8.0f, _action1: "rotate_easy_2", _coinGenerator: "long_rotate_rect_2_coin", _deltaX: 99999.0f, _width: 10.0f, _height: 0.5f, _posY: 0.0f)},
		{"long_rotate_rect_2_coin", new Data(_key: "long_rotate_rect_2_coin", _type: "coin", _deltaX: 1.0f, _frequencyScale: 0.0f, _amplitudeScale: 0.0f, _posY: 2.0f, _coinCountY: 3)},
		{"wave_easy_1", new Data(_key: "wave_easy_1", _appearLevel: 5, _maxLevel: 5, _appearType: 1, _type: "wave", _distance: 24.0f, _coinGenerator: "wave_easy_1_coin", _width: 0.5f, _gapHeight: 4.0f, _frequencyScale: 0.5f, _amplitudeScale: 1.5f, _posY: 0.0f)},
		{"wave_easy_1_coin", new Data(_key: "wave_easy_1_coin", _type: "coin", _deltaX: 1.0f, _frequencyScale: 0.5f, _amplitudeScale: 1.5f, _posY: 0.0f, _coinCountY: 1)},
		{"tunnel_mid", new Data(_key: "tunnel_mid", _appearLevel: 6, _appearType: 1, _type: "wave", _distance: 8.0f, _coinGenerator: "tunnel_mid_coin", _width: 0.5f, _gapHeight: 4.0f, _frequencyScale: 0.0f, _amplitudeScale: 0.0f, _posY: 0.0f)},
		{"tunnel_mid_coin", new Data(_key: "tunnel_mid_coin", _type: "coin", _deltaX: 1.0f, _frequencyScale: 0.0f, _amplitudeScale: 0.0f, _posY: 0.0f, _coinCountY: 1)},
		{"tunnel_top", new Data(_key: "tunnel_top", _appearLevel: 7, _appearType: 1, _type: "wave", _distance: 8.0f, _coinGenerator: "tunnel_top_coin", _width: 0.5f, _gapHeight: 4.0f, _frequencyScale: 0.0f, _amplitudeScale: 0.0f, _posY: 1.0f)},
		{"tunnel_top_coin", new Data(_key: "tunnel_top_coin", _type: "coin", _deltaX: 1.0f, _frequencyScale: 0.0f, _amplitudeScale: 0.0f, _posY: 1.0f, _coinCountY: 1)},
		{"tunnel_bottom", new Data(_key: "tunnel_bottom", _appearLevel: 7, _appearType: 1, _type: "wave", _distance: 8.0f, _coinGenerator: "tunnel_bottom_coin", _width: 0.5f, _gapHeight: 4.0f, _frequencyScale: 0.0f, _amplitudeScale: 0.0f, _posY: -1.0f)},
		{"tunnel_bottom_coin", new Data(_key: "tunnel_bottom_coin", _type: "coin", _deltaX: 1.0f, _frequencyScale: 0.0f, _amplitudeScale: 0.0f, _posY: -1.0f, _coinCountY: 1)},
		{"circle_mid_1", new Data(_key: "circle_mid_1", _appearLevel: 8, _appearType: 1, _type: "circle", _distance: 12.0f, _coinGenerator: "circle_mid_1_coin", _deltaX: 4.0f, _radius: 0.6f, _posY: 0.0f)},
		{"circle_mid_1_coin", new Data(_key: "circle_mid_1_coin", _type: "coin", _deltaX: 0.6f, _frequencyScale: 0.75f, _amplitudeScale: 2.0f, _isCos: true, _posY: 0.0f, _coinCountY: 1)},
		{"circle_random_1", new Data(_key: "circle_random_1", _appearLevel: 9, _appearType: 1, _type: "circle", _distance: 12.0f, _deltaX: 2.0f, _radius: 0.5f, _posY: -999.0f)},
		{"circle_random_up_1", new Data(_key: "circle_random_up_1", _appearLevel: 10, _appearType: 1, _type: "circle", _distance: 12.0f, _action1: "moveY_up_1", _deltaX: 3.0f, _radius: 0.5f, _posY: -999.0f)},
		{"circle_random_down_1", new Data(_key: "circle_random_down_1", _appearLevel: 10, _appearType: 1, _type: "circle", _distance: 12.0f, _action1: "moveY_down_1", _deltaX: 3.0f, _radius: 0.5f, _posY: -999.0f)},
		{"rect_random_1", new Data(_key: "rect_random_1", _appearLevel: 11, _appearType: 1, _type: "rect", _distance: 12.0f, _deltaX: 3.0f, _width: 1.0f, _height: 3.0f, _posY: -999.0f)},
		{"rect_random_up_1", new Data(_key: "rect_random_up_1", _appearLevel: 12, _appearType: 1, _type: "rect", _distance: 12.0f, _action1: "moveY_up_1", _deltaX: 4.0f, _width: 1.0f, _height: 3.0f, _posY: -999.0f)},
		{"rect_random_down_1", new Data(_key: "rect_random_down_1", _appearLevel: 12, _appearType: 1, _type: "rect", _distance: 12.0f, _action1: "moveY_down_1", _deltaX: 4.0f, _width: 1.0f, _height: 3.0f, _posY: -999.0f)},
		{"updownrect_1", new Data(_key: "updownrect_1", _appearLevel: 13, _appearType: 1, _type: "updownrect", _distance: 18.0f, _deltaX: 6.0f, _width: 1.5f, _gapHeight: 4.0f, _posY: -999.0f)},
		{"updowncircle_1", new Data(_key: "updowncircle_1", _appearLevel: 14, _appearType: 1, _type: "updowncircle", _distance: 18.0f, _deltaX: 6.0f, _radius: 2.0f, _gapHeight: 4.0f, _posY: -999.0f)},
		{"updowncircle_2", new Data(_key: "updowncircle_2", _appearLevel: 15, _appearType: 1, _type: "updowncircle", _distance: 31.0f, _deltaX: 8.0f, _radius: 4.0f, _gapHeight: 4.0f, _posY: -999.0f)},
		{"circlewave_1", new Data(_key: "circlewave_1", _appearLevel: 16, _appearType: 1, _type: "circlewave", _distance: 31.0f, _deltaX: 8.0f, _radius: 6.0f, _gapHeight: 0.0f, _posY: 0.0f)},
		{"coin_wave_1", new Data(_key: "coin_wave_1", _appearLevel: 1, _appearType: 3, _type: "coin", _distance: 12.0f, _deltaX: 1.0f, _frequencyScale: 0.5f, _amplitudeScale: 1.2f, _posY: 0.0f, _coinCountY: 2)},
		{"coin_wave_2", new Data(_key: "coin_wave_2", _appearLevel: 1, _appearType: 3, _type: "coin", _distance: 12.0f, _deltaX: 1.0f, _frequencyScale: 0.5f, _amplitudeScale: -1.2f, _posY: 0.0f, _coinCountY: 2)},
		{"coin_bonus_1", new Data(_key: "coin_bonus_1", _appearLevel: 1, _appearType: 4, _type: "coin", _distance: 12.0f, _deltaX: 1.0f, _frequencyScale: 0.5f, _amplitudeScale: 0.0f, _posY: 0.0f, _coinCountY: 5)},
		{"coin_bonus_2", new Data(_key: "coin_bonus_2", _appearLevel: 2, _appearType: 4, _type: "coin", _distance: 12.0f, _deltaX: 1.0f, _frequencyScale: 0.5f, _amplitudeScale: 1.0f, _posY: 0.0f, _coinCountY: 5)},
		{"coin_bonus_3", new Data(_key: "coin_bonus_3", _appearLevel: 3, _appearType: 4, _type: "coin", _distance: 12.0f, _deltaX: 1.0f, _frequencyScale: 0.5f, _amplitudeScale: 1.2f, _posY: 0.0f, _coinCountY: 5)},
		{"coin_bonus_4", new Data(_key: "coin_bonus_4", _appearLevel: 4, _appearType: 4, _type: "coin", _distance: 12.0f, _deltaX: 1.0f, _frequencyScale: 0.5f, _amplitudeScale: 2.0f, _posY: 0.0f, _coinCountY: 5)},
		{"circle_medium_1", new Data(_key: "circle_medium_1", _type: "circle", _action1: "moveY_medium_1", _deltaX: 2.4f, _radius: 2.5f)},
		{"circle_hard_1", new Data(_key: "circle_hard_1", _type: "circle", _action1: "moveY_hard_1", _deltaX: 1.8f, _radius: 3.0f)},
		{"circle_bonus_1", new Data(_key: "circle_bonus_1", _type: "circle", _action1: "moveY_down_1", _deltaX: 6.0f, _radius: 2.0f)},
		{"rect_easy_1", new Data(_key: "rect_easy_1", _appearType: 1, _type: "rect", _distance: 3.0f, _action1: "moveY_down_1", _deltaX: 5.0f, _width: 1.5f, _height: 2.0f)},
		{"rect_medium_1", new Data(_key: "rect_medium_1", _type: "rect", _action1: "rotate_medium_1", _deltaX: 2.4f, _width: 1.5f, _height: 3.0f)},
		{"rect_hard_1", new Data(_key: "rect_hard_1", _type: "rect", _action1: "rotate_hard_1", _deltaX: 1.8f, _width: 1.5f, _height: 4.0f)},
		{"rect_bonus_1", new Data(_key: "rect_bonus_1", _type: "rect", _action1: "moveY_down_1", _deltaX: 6.0f, _width: 1.0f, _height: 3.0f)},
		{"updownRect_medium_1", new Data(_key: "updownRect_medium_1", _type: "updownrect", _deltaX: 6.0f, _width: 1.5f, _gapHeight: 3.0f)},
		{"updownRect_hard_1", new Data(_key: "updownRect_hard_1", _type: "updownrect", _deltaX: 4.5f, _width: 1.5f, _gapHeight: 2.0f)},
		{"updownRect_bonus_1", new Data(_key: "updownRect_bonus_1", _type: "updownrect", _deltaX: 9.0f, _width: 1.5f, _gapHeight: 5.0f)},
		{"wave_medium_1", new Data(_key: "wave_medium_1", _type: "wave", _width: 0.5f, _gapHeight: 3.8f, _frequencyScale: 0.8f, _amplitudeScale: 1.8f)},
		{"wave_hard_1", new Data(_key: "wave_hard_1", _type: "wave", _width: 0.5f, _gapHeight: 3.5f, _frequencyScale: 1.0f, _amplitudeScale: 2.2f)},
	};
}
