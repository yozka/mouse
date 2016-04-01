using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mouse.ScreenSystem;
using Mouse.GameProgramms.Map;

namespace Mouse.GameProgramms.GameObjects.Units
{
    ///--------------------------------------------------------------------------------------







     ///=====================================================================================
    ///
    /// <summary>
    /// Обработка игровой логигки. когда урправляем мышкой, это:
    /// 1. Действие, которое вызывается, когда юнит находит сыр
    ///    и на карту добовляется новый кот
    /// 2. Действие, когда натыкаемся на активного кота
    ///
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    class AActionHeroMouse : AActionAI
    {





         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление внутренней логики объекта, который ищет сыр
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onUpdate(TimeSpan gameTime, AUnitObject unit)
        {
            if (unit.isPosition())
            {
                AMap map = unit.map;
                if (map != null)
                {
                    
                    List<AObject> objects = map.getObjects(unit);
                    foreach (AObject obj in objects)
                    {
                        /*
                         * нас интересуют объекты только СЫЫЫРР
                         */
                        if (obj is AObjCheese && cheesEvent != null)
                        {
                            cheesEvent(unit, obj as AObjCheese);
                        }
                        else
                        /*
                         * обработка что мынаткнулись на котов
                         */
                        if (obj is AUnitCat && catsEvent != null)
                        {
                            catsEvent(unit, obj as AUnitCat);
                        }
                    }
                }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// объявление делегата получателя события что мы наткнулись на сыр
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void cheesEventHandler(AUnitObject unit, AObjCheese cheese);
        public event cheesEventHandler cheesEvent;
        ///--------------------------------------------------------------------------------------





        

         ///=====================================================================================
        ///
        /// <summary>
        /// объявление делегата получателя события что мы наткнулись на кота
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void catsEventHandler(AUnitObject unit, AUnitCat cat);
        public event catsEventHandler catsEvent;

    }//AActionCheeseToCat
}
