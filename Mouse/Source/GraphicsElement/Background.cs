using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mouse.ScreenSystem;




namespace Mouse.GraphicsElement
{



     ///=====================================================================================
    ///
    /// <summary>
    /// Графический еэлемент, ввиде фоновой, движующейся картинки
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ABackground
    {
        private Texture2D m_backgroundTexture;
        private Vector2 m_sizeTexture;
        private Vector2 m_sizeTextureDiv2;
        private Vector2 m_viewport;
        private float m_rotate_x = 0;
        private float m_rotate_y = 0;
        private float m_speed = 100.0f;

        private float m_rotationAngle = 0f; //угол поворота
        private float m_rotationDirect = 0f;//в сторону поворота
        private float m_rotationSpeed = 500f;//Скорость поворота
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor 1
        /// передаются размеры объекта, в данном случаи размеры экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ABackground(Vector2 size)
        {
            m_viewport = size;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor 2
        /// передаются размеры объекта, в данном случаи размеры экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ABackground(Vector2 size, float speed)
        {
            m_viewport = size;
            m_speed = speed;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка картинок заднего фона игры
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content, string sNameTexture)
        {
            m_backgroundTexture = content.Load<Texture2D>(sNameTexture);
            m_sizeTexture = new Vector2(m_backgroundTexture.Width, m_backgroundTexture.Height);
            m_sizeTextureDiv2 = m_sizeTexture / 2;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка фонового экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void draw(TimeSpan gameTime, ASpriteBatch spriteBatch)
        {
            Vector2 pos = new Vector2(m_rotate_x, m_rotate_y);
            while (pos.Y < m_viewport.Y)
            {
                while (pos.X < m_viewport.X)
                {
                    spriteBatch.Draw(m_backgroundTexture,
                               pos + m_sizeTextureDiv2, null,
                               Color.White,
                               m_rotationAngle, m_sizeTextureDiv2, 1.0f, 0, 0.0f);
                    
                    pos.X += m_sizeTexture.X;
                }
                pos.Y += m_sizeTexture.Y;
                pos.X = m_rotate_x;
            }



            /*
             *  поставим новые координаты
             */
            float shift = (float)gameTime.Milliseconds / m_speed;
            m_rotate_x += shift * 1.3f;
            m_rotate_y += shift;
            if (m_rotate_x > 0)
            {
                m_rotate_x -= m_sizeTexture.X;
            }
            if (m_rotate_y > 0)
            {
                m_rotate_y -= m_sizeTexture.Y;
            }


            /*
             * Сделаем поворот картинки
             */
            if (m_rotationDirect != 0.0f)
            {
                shift = (float)gameTime.Milliseconds / m_rotationSpeed;
                m_rotationAngle += m_rotationDirect * shift;

                const float rmin = (float)Math.PI * 2;
                if (m_rotationAngle > rmin)
                {
                    m_rotationAngle -= rmin;
                }
                else
                    if (m_rotationAngle < 0)
                    {
                        m_rotationAngle += rmin;
                    }

            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Угол поворота картинки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float rotationAngle
        {
            get
            {
                return m_rotationAngle;
            }
            set
            {
                m_rotationAngle = value;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Направление вращения картинки -1 до 1, 0 = остановить вращение
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float rotationDirect
        {
            get
            {
                return m_rotationDirect;
            }
            set
            {
                m_rotationDirect = value;
            }
        }        
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Скорость вращения картинки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public float rotationSpeed
        {
            get
            {
                return m_rotationSpeed;
            }
            set
            {
                m_rotationSpeed = value;
            }
        }


    }//ABackground
}