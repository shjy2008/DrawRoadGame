/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ShopPanel
{
    public class ShopPanel : BasePanel
    {
        public static ShopPanel instance;

        [SerializeField] GridView gridView = default;

        ShopPanel()
        {
            instance = this;
        }

        void Start()
        {
            GenerateCells(50);

            //gridView.JumpTo(0);

        }

        void SelectCell()
        {
            if (gridView.DataCount == 0)
            {
                return;
            }
        }

        void GenerateCells(int dataCount)
        {
            var items = Enumerable.Range(0, dataCount)
                .Select(i => new ItemData(i))
                .ToArray();

            gridView.UpdateContents(items);
            SelectCell();
        }

        public void OnCloseBtnClk()
        {
            ClosePanel();
        }
    }
}
