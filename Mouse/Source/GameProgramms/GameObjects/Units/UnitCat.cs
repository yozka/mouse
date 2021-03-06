﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using Mouse.GameProgramms.GameObjects;
using Mouse.App;


namespace Mouse.GameProgramms.GameObjects.Units
{


     ///=====================================================================================
    ///
    /// <summary>
    /// Юнит типа "кошка"
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AUnitCat : AUnitObject
    {
        private SoundEffect m_sound1 = null;
        ///--------------------------------------------------------------------------------------




        
        
         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AUnitCat()
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
        public AUnitCat(AActionAI actionAI)
            :
            base(actionAI)
        {
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента для юнита типа кошка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            base.onLoadContent(content);
            sprite = content.Load<Texture2D>("Common/Units/Cat");
            //m_sound1 = content.Load<SoundEffect>("Common/Sound/cats_1");
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Возвращаем звук, который возникат при создании/появление кошки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override SoundEffect sound_new
        {
            get 
            {
                return m_sound1;
            }
        }



    }//AUnitCat
}
