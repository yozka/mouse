using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mouse.ScreenSystem;


namespace Mouse.Screens
{





     ///=====================================================================================
    ///
    /// <summary>
    /// Пункт меню, который закрывает игру
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AItem_exitGame : AMenuEntry
    {
        public AItem_exitGame(AMenuItems itemsParent)
            :
            base(itemsParent, "Exit")
        {
        }
        public override void onSelect(AMenuScreen menu)
        {
            menu.screenManager.Game.Exit();
        }
    }
    ///--------------------------------------------------------------------------------------\







    ///=====================================================================================
    ///
    /// <summary>
    /// Пункт меню закрытие текущего меню
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AItem_exitMenu : AMenuEntry
    {
        public AItem_exitMenu(AMenuItems itemsParent, string caption)
            :
            base(itemsParent, caption)
        {
        }
        public override void onSelect(AMenuScreen menu)
        {
            menu.exitScreen();
        }
    }
    ///--------------------------------------------------------------------------------------






    ///=====================================================================================
    ///
    /// <summary>
    /// Пункт меню который вызывает экран
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AItem_addScreen : AMenuEntry
    {
        private AScreen m_screen;
        public AItem_addScreen(AMenuItems itemsParent, string caption, AScreen screen)
            :
            base(itemsParent, caption)
        {
            m_screen = screen;
        }
        public override void onSelect(AMenuScreen menu)
        {
            menu.screenManager.camera.smoothResetCamera();
            menu.screenManager.addScreen(m_screen);
        }
    }
    ///--------------------------------------------------------------------------------------










}