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
    /// Вспомогательный класс представляет собой отдельный пункт меню в MenuScreen. 
    /// По умолчанию просто рисует строку текста, но она может быть настроена для отображения меню
    /// записи по-разному.
    /// Это также обеспечивает случае, когда пункт меню будет выбран
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AMenuEntry
    {
        ///--------------------------------------------------------------------------------------
        protected Vector2     m_position;
        protected float       m_scale;
        protected float       m_selectionFade;
        protected string      m_text;

        private Texture2D     m_blankTexture;
        private AMenuItems    m_itemsParent;

        /// <summary>
        /// признак, загружен контент юнита или нет
        /// </summary>
        private bool        m_loadContent = false;
        ///--------------------------------------------------------------------------------------




        
         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor без инциализации привязки экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMenuEntry(AMenuItems itemsParent, string text)
        {
            m_text = text;
            m_itemsParent = itemsParent;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента объекта, идет проверка на флаг, что контент уже был когдато загружен
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            if (!m_loadContent)
            {
                onLoadContent(content);
                m_loadContent = true;
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента всех потомков
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onLoadContent(ContentManager content)
        {
            m_blankTexture = content.Load<Texture2D>("Common/itemsMenu");
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка нажатия выбора пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual void onSelect(AMenuScreen menu)
        {
            if (selectEvent != null)
            {
                selectEvent(menu, this);
            }

        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// объявление делегата получателя события на выбор пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void selectEventHandler(AMenuScreen menu, AMenuEntry item);
        public event selectEventHandler selectEvent;
        ///--------------------------------------------------------------------------------------













        ///=====================================================================================
        ///
        /// <summary>
        /// Получает или задает текст этого пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public string text
        {
            get { return m_text; }
            set { m_text = value; }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Устанавливает позицию меню, в экране
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual Vector2 position
        {
            get { return m_position; }
            set { m_position = value; }
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление логики пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(bool isSelected, TimeSpan gameTime)
        {
            //плавно подсвечиваем выбранный пункт меню
            float fadeSpeed = (float)gameTime.TotalSeconds * 4;

            if (isSelected)
                m_selectionFade = Math.Min(m_selectionFade + fadeSpeed, 1);
            else
                m_selectionFade = Math.Max(m_selectionFade - fadeSpeed, 0);

            m_scale = 0.7f + 0.1f * m_selectionFade;
        }
        ///--------------------------------------------------------------------------------------





 
         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка одного пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void draw(AScreen menu, bool isSelected, TimeSpan gameTime, ASpriteBatch spriteBatch)
        {
            Color color = Color.Lerp(Color.White, new Color(179, 106, 3), m_selectionFade);

            SpriteFont font = menu.screenManager.spriteFonts.menuSpriteFont;

            Vector2 origin = new Vector2((font.MeasureString(m_text).X * (m_scale - 0.7f)) / 4f, font.LineSpacing / 2f);

            int iHeight =  getHeight(menu);
            int iWidth = getWidth(menu);
            int iWdt = iWidth / 4;
            spriteBatch.begin();
            spriteBatch.Draw(m_blankTexture, new Rectangle((int)position.X - iWdt, (int)position.Y - iHeight / 2, iWidth + iWdt, iHeight), m_itemsParent.itemBackgroundColor);
            spriteBatch.end();

            spriteBatch.begin();
            spriteBatch.DrawString(font, m_text, m_position, Color.Black, 0, origin, m_scale, SpriteEffects.None, 0);
            spriteBatch.end();


            spriteBatch.begin();
            spriteBatch.DrawString(font, m_text, m_position, color, 0, origin + new Vector2(2f,2f), m_scale, SpriteEffects.None, 0);
            spriteBatch.end();

            onDraw(gameTime, spriteBatch, origin);

        }
        ///--------------------------------------------------------------------------------------




               
        
         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка одного пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual void onDraw(TimeSpan gameTime, ASpriteBatch spriteBatch, Vector2 origin)
        {
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Возвращение высоты пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int getHeight(AScreen screen)
        {
            return (int)(screen.screenManager.spriteFonts.menuSpriteFont.LineSpacing * 0.7f);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Ширина пункта меню
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public virtual int getWidth(AScreen screen)
        {
            return (int)(screen.screenManager.spriteFonts.menuSpriteFont.MeasureString(m_text).X);
        }





    }//AMenuEntry
    //===========================================================================================





}