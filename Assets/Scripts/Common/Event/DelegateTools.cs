using UnityEngine;
using System.Collections;

namespace DelegateTools
{
    public delegate void VoidDelegate();// 传方法
    public delegate void IntDelegate(int param_int);// 传带参数的方法(int param)
    public delegate void StringDelegate(string param_str);// 传带参数的方法(string param)
    public delegate void IntStringDelegate(int param1, string param2);// 传带参数的方法(int param1, string param2)
}