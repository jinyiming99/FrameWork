using System.Collections.Generic;

using UnityEngine;

namespace GameFrameWork.UI
{
    public class PositionContainer
    {
        List<Vector2> _list = new List<Vector2>();
        private int _itemWidth;
        private int _itemHight;
        private int _widthSpacing;
        private int _hightSpacing;
        private int _column;
        
        private int _containerWidth;
        public int ContainerWidth => _containerWidth;
        private int _containerHight;
        public int ContainerHight => _containerHight;
        
        
        public PositionContainer(int itemWidth,int itemHight,int widthSpacing,int hightSpacing ,int column)
        {
            _itemWidth = itemWidth;
            _itemHight = itemHight;
            _widthSpacing = widthSpacing;
            _hightSpacing = hightSpacing;
            _column = column;
        }

        public Vector2 GetPos(int index)
        {
            if (index < 0 || index >= _list.Count)
            {
                return Vector2.zero;
            }
            return _list[index];
        }
        
        public void Calculate(int count,bool isHorizontal)
        {
            _list.Clear();
            int row = count / _column;
            if (count % _column != 0)
            {
                row += 1;
            }
            for (int i = 0; i < count; ++i)
            {
                if (isHorizontal)
                    _list.Add(new Vector2((i % _column) * (_itemWidth + _widthSpacing), (-i / _column) * (_itemHight + _hightSpacing)));
                else
                    _list.Add(new Vector2((i / _column) * (_itemWidth + _widthSpacing), (-i % _column) * (_itemHight + _hightSpacing)));
            }
            _containerWidth = isHorizontal ? _column * (_itemWidth + _widthSpacing) - _widthSpacing : row * (_itemWidth + _widthSpacing) - _widthSpacing;
            _containerHight = isHorizontal ? row * (_itemHight + _hightSpacing) - _hightSpacing : _column * (_itemHight + _hightSpacing) - _hightSpacing;
        }
    }
}