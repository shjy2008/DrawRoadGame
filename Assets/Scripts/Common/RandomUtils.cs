using System;
using System.Collections.Generic;

public class RandomUtils
{
    public RandomUtils()
    {
    }

    public static List<T> ListChoice<T>(List<T> l, int count)
    {
        List<T> tempList = new List<T>(l);
        List<T> newList = new List<T>();
        for (int i = 0; i < count; ++i)
        {
            int index = UnityEngine.Random.Range(0, tempList.Count);
            newList.Add(tempList[index]);
            tempList.RemoveAt(index);
        }
        return newList;
    }

    public static List<T> GetRandomFromList<T>(List<T> l, int count)
    {
        List<T> retList = new List<T>();
        for (int i = 0; i < count; ++i)
        {
            retList.Add(ListChoiceOne<T>(l));
        }
        return retList;
    }

    public static T ListChoiceOne<T>(List<T> l)
    {
        List<T> retList = ListChoice<T>(l, 1);
        return retList[0];
    }

    public static List<T> GetShuffledList<T>(List<T> l)
    {
        return ListChoice<T>(l, l.Count);
    }

    public static void ListShuffle<T>(List<T> l)
    {
        for (int i = 1; i < l.Count; ++i)
        {
            int j = UnityEngine.Random.Range(0, i);
            T temp = l[i];
            l[i] = l[j];
            l[j] = temp;
        }
    }
}
