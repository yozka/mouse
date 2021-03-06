﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mouse.ScreenSystem;


namespace Mouse.Screens
{



     ///=====================================================================================
    ///
    /// <summary>
    /// Коллекция записей в меню
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AMenuItems
    {
        ///--------------------------------------------------------------------------------------
        private AScreen m_menu = null;
        private List<AMenuEntry>    m_entries = new List<AMenuEntry>();
        private Color m_itemBackgroundColor = Color.Yellow * 0.8f;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMenuItems(AScreen menu)
        {
            m_menu = menu;
        }
        ///--------------------------------------------------------------------------------------






        
         ///=====================================================================================
        ///
        /// <summary>
        /// Цвет фона над пунктом меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Color itemBackgroundColor
        {
            get
            {
                return m_itemBackgroundColor;
            }
            set
            {
                m_itemBackgroundColor = value;
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            foreach (AMenuEntry entry in m_entries)
            {
                entry.loadContent(content);
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Получает список пунктов меню, так что производные классы могут добавить
        /// Или изменить содержимое меню.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public IList<AMenuEntry> entries
        {
            get { return m_entries; }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// количество пунктов меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int count
        {
            get { return m_entries.Count; }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление пункта меню
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public AMenuEntry addItem(string caption)
        {
            AMenuEntry item = new AMenuEntry(this, caption);
            add(item);
            return item;
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление пункта меню выход из игры
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public AMenuEntry addItemExitGame()
        {
            AMenuEntry item = new AItem_exitGame(this);
            add(item);
            return item;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление пункта меню "выход из меню"
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public AMenuEntry addItemExitMenu(string caption)
        {
            AMenuEntry item = new AItem_exitMenu(this, caption);
            add(item);
            return item;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление пункта меню c переключателем
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public AItem_checkbox addItemCheckbox(string caption, bool value)
        {
            AItem_checkbox item = new AItem_checkbox(this, caption, value);
            add(item);
            return item;
        }
        ///--------------------------------------------------------------------------------------
        ///







         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление пункта меню с добавлением экрана и названием
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public AMenuEntry addItem(string caption, AScreen screen)
        {
            AMenuEntry item = new AItem_addScreen(this, caption, screen);
            add(item);
            return item;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Добавление пункта меню с добавлением экрана
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public AMenuEntry addItem(AScreen screen)
        {
            AMenuEntry item = new AItem_addScreen(this, screen.getCaption(), screen);
            add(item);
            return item;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Возвращает индекс элемента меню в позиции данной мышкой или тачпадом.
        /// </summary>
        /// <param name="state">тачпад - мышка с координатами</param>
        /// <returns>Индекс пункта меню, если нажали не туда то -1</returns>
        ///  
        ///--------------------------------------------------------------------------------------
        ///
        public AMenuEntry getMenuEntryAt(MouseState state)
        {
            Point pos = new Point(state.X, state.Y);
            foreach (AMenuEntry entry in m_entries)
            {
                float width = entry.getWidth(m_menu);
                float height = entry.getHeight(m_menu);
                Rectangle rect = new Rectangle((int)entry.position.X, (int)(entry.position.Y - (height * 0.5f)),
                                               (int)width, (int)height);
                if (rect.Contains(pos))
                {
                    return entry;
                }
            }
            return null;
        }
        ///--------------------------------------------------------------------------------------








 
         ///=====================================================================================
        ///
        /// <summary>
        /// Передаем метод обновления для всех вложенных меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime, AMenuEntry selected)
        {
            // обновим все пункты меню
            bool isActive = m_menu.isActive;
            foreach (AMenuEntry entry in m_entries)
            {
                bool isSelected = isActive && (entry == selected);
                entry.update(isSelected, gameTime);
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка всех пунктов меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void draw(TimeSpan gameTime, ASpriteBatch spriteBatch, AMenuEntry selected)
        {

            bool isActive = m_menu.isActive;
            foreach (AMenuEntry entry in m_entries)
            {
                bool isSelected = isActive && (entry == selected);
                entry.draw(m_menu, isSelected, gameTime, spriteBatch);
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// добавление пункта меню в список
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void add(AMenuEntry entry)
        {
            if (m_menu.isContent)
            {
                entry.loadContent(m_menu.content);
            }
            m_entries.Add(entry);

        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// удаление все пунктов меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void clear()
        {
            m_entries.Clear();
        }


    }//AMenuScreen
}