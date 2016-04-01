﻿
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mouse.GameProgramms.GameObjects;

namespace Mouse.GameProgramms.GameObjects.Units
{




     ///=====================================================================================
    ///
    /// <summary>
    /// Юнит типа "мышь"
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AUnitMouse : AUnitObject
    {
        ///--------------------------------------------------------------------------------------



        
         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AUnitMouse()
        {
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 2
        /// с привязкой обработчика интелекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AUnitMouse(AActionAI actionAI)
            :
            base(actionAI)
        {
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента для юнита типа мышь
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            base.onLoadContent(content);
            sprite = content.Load<Texture2D>("Common/Units/Mouse");
        }
        ///--------------------------------------------------------------------------------------
        



    }//AUnitMouse
}