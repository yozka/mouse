﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Mouse.ScreenSystem;

namespace Mouse.Screens
{


    ///=====================================================================================
    ///
    /// <summary>
    /// Главный экран меню это первое, что отображается, когда игра запускается.
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AMainMenuScreen : AMenuScreen
    {
        ///-------------------------------------------------------------------------------------
        private Song m_mediaMenu = null;
        
        ///-------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор основного меню
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public AMainMenuScreen()
            : base("Mouse menu")
        {
        }
        ///-------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка музыки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            base.onLoadContent(content);
            m_mediaMenu = content.Load<Song>("Common/Music/menu");
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// При выходе из главного меню, выходим из игры ваще.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onCancel()
        {
            screenManager.Game.Exit();
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// активация фокуса у экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onFocusActivation(AScreen focus)
        {
            screenManager.music.playRepeat(m_mediaMenu);
        }
        ///--------------------------------------------------------------------------------------








    }//AMainMenuScreen
}