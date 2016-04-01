using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mouse.ScreenSystem;
using Mouse.App;

namespace Mouse.GameProgramms.GameObjects.Units
{


     ///=====================================================================================
    ///
    /// <summary>
    /// Тип передвижения, вверх или вниз
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public enum EMoveVertical
    {
        none,
        up,
        down
    }
    ///--------------------------------------------------------------------------------------





     ///=====================================================================================
    ///
    /// <summary>
    /// интелект для хождения сверху в низ
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AActionVertical : AActionAI
    {

        private EMoveVertical m_move = EMoveVertical.none; //тип движения
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление внутренней логики объекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onUpdate(TimeSpan gameTime, AUnitObject unit)
        {
            /*
             * инциализация персонажа
             */
            if (!unit.isPosition())
            {
                if (unit.map != null)
                {
                    Vector2 pos = getNewPosition(unit);
                    Vector2 size = new Vector2(unit.size.X, unit.map.size.Y);
                    List<AObject> list = unit.map.getObjects(new Vector2(pos.X - unit.size.X / 2, 0), size);
                    int i = 0;
                    while (list.Count > 0 && ++i < 10)
                    {
                        pos = getNewPosition(unit);
                        list = unit.map.getObjects(new Vector2(pos.X - unit.size.X / 2, 0), size);
                    }



                    unit.position = pos;
                    
                    
                    moveStart(unit);
                }
            }
        }
        ///--------------------------------------------------------------------------------------






       
         ///=====================================================================================
        ///
        /// <summary>
        /// Установка новой позиции
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 getNewPosition(AUnitObject unit)
        {
            AGameHelper helper = AGameHelper.instance;
            Vector2 mapSize = unit.map.size;
            Vector2 unitSize = unit.size;
            float left = helper.random.Next((int)unitSize.X, (int)(mapSize.X - unitSize.X));
            float top = unitSize.Y; ;
            m_move = EMoveVertical.down;
            if (helper.random.Next(2) == 1)
            {
                top = mapSize.Y - unitSize.Y;
                m_move = EMoveVertical.up;
            }
            return new Vector2(left, top);
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// запуск движения игрового персонажа
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void moveStart(AUnitObject unit)
        {
            AGameHelper helper = AGameHelper.instance;
            float fShift = helper.random.Next(50, 100);
            switch (m_move)
            {
                case EMoveVertical.none:
                    {
                        unit.moveStop();
                        break;
                    }
                case EMoveVertical.down:
                    {
                        unit.moveStart(new Vector2(0, fShift));
                        break;
                    }
                case EMoveVertical.up:
                    {
                        unit.moveStart(new Vector2(0, -fShift));
                        break;
                    }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка столкновения с препятствием
        /// идем в другую сторону
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onBlockageMove(Vector2 newPosition, AUnitObject unit)
        {
            m_move = (m_move == EMoveVertical.up) ? EMoveVertical.down : EMoveVertical.up;
            moveStart(unit);
        }
        ///--------------------------------------------------------------------------------------





    }//AActionVertical
}
