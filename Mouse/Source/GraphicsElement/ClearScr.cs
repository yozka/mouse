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
    /// Графический еэлемент, который закрашивает экран в квадраты
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AClearSrc
    {
        private readonly int c_size = 30;//размер квадратов


        private Texture2D m_backgroundTexture;
        private Vector2 m_viewport;

        private Vector2 m_center = Vector2.Zero;
        
        private int m_begin = 0;
        private int m_end = 0;

        private TimeSpan m_speed = TimeSpan.FromSeconds(2);
        private TimeSpan m_time = TimeSpan.Zero;
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor 1
        /// передаются размеры объекта, в данном случаи размеры экрана
        /// и позицию где будет центр стирания экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AClearSrc(Vector2 size, Vector2 position)
        {
            m_viewport = size;
            m_center = position;
            calc();
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
        public AClearSrc(Vector2 size, Vector2 position, TimeSpan speed)
        {
            m_viewport = size;
            m_speed = speed;
            m_center = position;
            calc();
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// подсчет позиций
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void calc()
        {
            Vector2 dd = m_viewport - m_center;
            m_end = (int)Math.Max(dd.X + dd.Y, m_center.X + m_center.Y);
            m_end = (int)Math.Max(m_end, m_center.X + dd.Y);
            m_end = (int)Math.Max(m_end, m_center.Y + dd.X);
            m_end = (int)Math.Max(m_end, m_center.X + m_viewport.Y);
            m_end = (int)Math.Max(m_end, m_center.Y + m_viewport.X);
            m_end = (int)(m_end / c_size) + 1;
            m_begin = m_end;

            m_speed = TimeSpan.FromMilliseconds(m_speed.TotalMilliseconds / m_begin);
            m_time = m_speed;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            m_backgroundTexture = content.Load<Texture2D>("Common/blank");
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
            spriteBatch.begin();
            for (int i = m_begin; i < m_end; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    int x,y;
                    x = (int)m_center.X + j * c_size;
                    y = (int)m_center.Y + (i - j) * c_size;
                    spriteBatch.Draw(m_backgroundTexture, new Rectangle(x, y, c_size, c_size), Color.Black);

                    y = (int)m_center.Y - (i - j) * c_size;
                    spriteBatch.Draw(m_backgroundTexture, new Rectangle(x, y, c_size, c_size), Color.Black);

                    x = (int)m_center.X - j * c_size;
                    spriteBatch.Draw(m_backgroundTexture, new Rectangle(x, y, c_size, c_size), Color.Black);

                    y = (int)m_center.Y + (i - j) * c_size;
                    spriteBatch.Draw(m_backgroundTexture, new Rectangle(x, y, c_size, c_size), Color.Black);

                }
            }
            spriteBatch.end();

            
            





        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// посчитаем новую позицию для очистки экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool updateTo(TimeSpan gameTime)
        {
            /*
             *  поставим новые координаты
            */
            m_time -= gameTime;
            if (m_time.TotalMilliseconds < 0)
            5
                m_time += m_speed;

                m_begin--;
            }
            return m_begin < 0 ? false : true;
        }



    }//ABackground
}