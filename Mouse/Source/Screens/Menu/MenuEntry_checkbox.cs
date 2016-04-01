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
    /// Пункт меню с чекбоксом
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AItem_checkbox : AMenuEntry
    {
        ///--------------------------------------------------------------------------------------
        private Texture2D   m_checked = null;
        private Texture2D   m_unchecked = null;
        private bool        m_value = false;
        private Vector2     m_shift = Vector2.Zero;//смещение в позиции для текста
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AItem_checkbox(AMenuItems itemsParent, string caption, bool value)
            :
            base(itemsParent, caption)
        {
            m_value = value;
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            base.onLoadContent(content);
            m_checked = content.Load<Texture2D>("Common/checkbox_checked");
            m_unchecked = content.Load<Texture2D>("Common/checkbox_unchecked");
            m_shift.X = m_checked.Width;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// объявление делегата получателя события на выбор пункта меню
        /// на входе состояния чекбокса
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void checkedEventHandler(AMenuScreen menu, AMenuEntry item, bool value);
        public event checkedEventHandler checkedEvent;
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка выбора пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onSelect(AMenuScreen menu)
        {
            m_value = !m_value;
            if (checkedEvent != null)
            {
                checkedEvent(menu, this, m_value);
            }
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Дополнительная отрисовка данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDraw(TimeSpan gameTime, ASpriteBatch spriteBatch, Vector2 origin)
        {
            spriteBatch.begin();
            Texture2D sprite = m_value ? m_checked : m_unchecked;
            spriteBatch.Draw(sprite, m_position - m_shift, null, Color.White, 0, origin, m_scale, SpriteEffects.None, 0);
            spriteBatch.end();
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Устанавливает позицию меню, в экране
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override Vector2 position
        {
            get 
            {
                return m_position - m_shift; 
            }
            set 
            {
                m_position = value; 
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Ширина пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override int getWidth(AScreen screen)
        {
            int iWidth = base.getWidth(screen);
            return iWidth + (int)m_shift.X;
        }




    }
    ///--------------------------------------------------------------------------------------
    ///


}