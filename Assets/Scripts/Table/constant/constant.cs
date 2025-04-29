//#file：./tab/constant.xlsx, sheet：constant，output：constant.cs

using System.Collections.Generic;

public class Table_constant
{
	public struct Data
	{
		public string key;
		public int param1;
		public List<int> param2;

		public Data(string _key = "", int _param1 = 0, List<int> _param2 = null)
		{
			key = _key;
			param1 = _param1;
			param2 = _param2;
		}
	}
	public static Dictionary<string, Data> data = new Dictionary<string, Data> {
		{"normal_ball_cost", new Data(_key: "normal_ball_cost", _param1: 1000)},
		{"advanced_ball_cost", new Data(_key: "advanced_ball_cost", _param1: 2000)},
		{"watch_ad_earn_coin", new Data(_key: "watch_ad_earn_coin", _param1: 500)},
		{"default_ball_id", new Data(_key: "default_ball_id", _param1: 1001)},
		{"default_road_id", new Data(_key: "default_road_id", _param1: 1001)},
		{"chest_room_coin", new Data(_key: "chest_room_coin", _param2: new List<int>(new int[] { 75, 75, 100, 100, 125, 125, 150, 150, 200 }))},
		{"endless_unlock_level", new Data(_key: "endless_unlock_level", _param1: 10)},
		{"normal_level_total_count", new Data(_key: "normal_level_total_count", _param1: 99999999)},
		{"normal_level_ads_coin_rate", new Data(_key: "normal_level_ads_coin_rate", _param1: 2)},
		{"bonus_level_ads_coin_rate", new Data(_key: "bonus_level_ads_coin_rate", _param2: new List<int>(new int[] { 3, 4, 5 }))},
	};
}
