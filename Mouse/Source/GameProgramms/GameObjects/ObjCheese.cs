using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mouse.ScreenSystem;
using Mouse.App;

namespace Mouse.GameProgramms.GameObjects
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Сыр
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AObjCheese : AObject
    {
        ///--------------------------------------------------------------------------------------
        private const float c_rotation = 1000;//количество милисекунд для ротации сыра
        private const float c_show = 800;//количество милисекундр для вывода сыра
        private const float c_hide = 300;//скрытие сыра
        private const float c_rotationLeft = -0.3f;//поворот сыра на лево
        private const float c_rotationRight = 0.3f;//поворот сыра на право

        ///--------------------------------------------------------------------------------------
        private Texture2D   m_shadow; //тень юнита  
        private Texture2D   m_sprite; //спрайт для отрисовки
        private Vector2     m_sizeDiv2 = Vector2.Zero; //размер спрайта урезаные в двое
        private float       m_zoom = 0.0f;//маштаб сыра, 
        
        private float       m_rotation = 0.0f;//поворот сыраж
        private float       m_rotationSide = 1.0f;//сторона вкотоую идет ротация

        private bool        m_delete = false;//признак того что идет удаление сыра
        ///--------------------------------------------------------------------------------------



     


         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка контента картинки сыра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onLoadContent(ContentManager content)
        {
            base.onLoadContent(content);
            m_shadow = content.Load<Texture2D>("Common/Units/shadow");
            m_sprite = content.Load<Texture2D>("Common/cheese");
            size = new Vector2(m_sprite.Width, m_sprite.Height);
            m_sizeDiv2 = size / 2;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка тени
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onBeforeDraw(TimeSpan gameTime, ASpriteBatch spriteBatch)
        {
            spriteBatch.begin();
            spriteBatch.Draw(m_shadow, position - new Vector2(50, -5), Color.White);
            spriteBatch.end();
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка юнита
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDraw(TimeSpan gameTime, ASpriteBatch spriteBatch)
        {

            /* сделаем поворот сыра*/
            float transRotation = (float)(gameTime.TotalMilliseconds / c_rotation);
            m_rotation += transRotation * m_rotationSide;
            if (m_rotation < c_rotationLeft)
            {
                m_rotation = c_rotationLeft;
                m_rotationSide = -m_rotationSide;
            }
            if (m_rotation > c_rotationRight)
            {
                m_rotation = c_rotationRight;
                m_rotationSide = -m_rotationSide;
            }



            if (m_delete && !active)
            {
                float transShow = (float)(gameTime.TotalMilliseconds / c_hide);
                m_zoom -= transShow;
                m_zoom = MathHelper.Clamp(m_zoom, 0, 1);
            }
            else    /* увеличем размер сыра*/
            if (m_zoom < 1)
            {
                float transShow = (float)(gameTime.TotalMilliseconds / c_show);
                m_zoom += transShow;
                m_zoom = MathHelper.Clamp(m_zoom, 0, 1);
            }

            spriteBatch.begin();
            spriteBatch.Draw(m_sprite,
                                        position, null,
                                        Color.White,
                                        m_rotation, m_sizeDiv2, m_zoom, 0, 0.0f);

            spriteBatch.end();

           
            //spriteBatch.Draw(m_sprite, position - size / 2, Color.White);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Если сыр ниразу не ставили, то установим ему позицию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime)
        {
            base.onUpdate(gameTime);
            if (!isPosition())
            {
                newPosition();
            }

            //смотрим, если сыр не активен и у него стоит пометка на удаление, то удаляем
            if (m_delete && !active && m_zoom == 0.0f)
            {
                map = null;
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
        public void newPosition()
        {
            AGameHelper helper = AGameHelper.instance;
            if (map != null)
            {
                 Vector2 mapSize = map.size;
                 Vector2 unitSize = size;
                 float posX = helper.random.Next((int)unitSize.X, (int)(mapSize.X - unitSize.X));
                 float posY = helper.random.Next((int)unitSize.Y, (int)(mapSize.Y - unitSize.Y));

                 position = new Vector2(posX, posY);
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Удаление из списка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void remove()
        {
            if (!m_delete)
            {
                m_delete = true;
                active = false;
            }
        }

    }//AObjCheese
}
