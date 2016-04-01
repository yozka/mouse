using System;
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
    public enum EMoveHorizontal
    {
        none,
        left,
        right
    }
    ///--------------------------------------------------------------------------------------





     ///=====================================================================================
    ///
    /// <summary>
    /// интелект для хождения сверху в низ
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AActionHorizontal : AActionAI
    {

        private EMoveHorizontal m_move = EMoveHorizontal.none; //тип движения
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
                AGameHelper helper = AGameHelper.instance;

                if (unit.map != null)
                {
                    Vector2 mapSize = unit.map.size;
                    Vector2 unitSize = unit.size;
                    float left = unitSize.X;
                    float top = helper.random.Next((int)unitSize.Y, (int)(mapSize.Y - unitSize.Y)); ;
                    m_move = EMoveHorizontal.right;
                    if (helper.random.Next(2) == 1)
                    {
                        left = mapSize.X - unitSize.X;
                        m_move = EMoveHorizontal.left;
                    }

                    unit.position = new Vector2(left, top);
                    moveStart(unit);
                }
            }
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
                case EMoveHorizontal.none:
                    {
                        unit.moveStop();
                        break;
                    }
                case EMoveHorizontal.right:
                    {
                        unit.moveStart(new Vector2(fShift, 0));
                        break;
                    }
                case EMoveHorizontal.left:
                    {
                        unit.moveStart(new Vector2(-fShift, 0));
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
            m_move = (m_move == EMoveHorizontal.left) ? EMoveHorizontal.right : EMoveHorizontal.left;
            moveStart(unit);
        }
        ///--------------------------------------------------------------------------------------





    }//AActionHorizontal
}
