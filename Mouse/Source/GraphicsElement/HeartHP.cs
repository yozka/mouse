using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Mouse.Particle;
using Mouse.ScreenSystem;


namespace Mouse.GraphicsElement
{



     ///=====================================================================================
    ///
    /// <summary>
    /// Графический еэлемент, ввиде сердечек
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AHeartHP
    {
        private const float c_flash = 600.0f; //количество время для стука частоты сердца
        private const float c_zoomMin = 0.9f;//минимальная амплитуда сердца
        private const float c_zoomMax = 1.3f;//максильная амплитуда сердца
        private const float c_spacing = 5.0f;//растояние между сердцами
        ///--------------------------------------------------------------------------------------
        
        private Texture2D m_texture;
        private Vector2 m_size;
        private Vector2 m_sizeDiv2;
        private Vector2 m_position;
        private AParticleEffect m_effect = null;

        private float m_zoom = 1.0f;//увелечение серда
        private float m_zoomSide = 1.0f;//сторона в которую идет увелечение сердца
        ///--------------------------------------------------------------------------------------
        private int m_hp = 0;
        





         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor 1
        /// передаются позиция графического объекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AHeartHP(Vector2 position, AParticleEffect effect)
        {
            m_position = position;
            m_effect = effect;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// проверка, живой или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isLive
        {
            get
            {
                return m_hp > 0;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Знаение уровня жизней
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int hp
        {
            get
            {
                return m_hp;
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Новая игра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void newGame()
        {
            m_hp = 3;
            m_zoom = 1.0f;
            m_zoomSide = 1.0f;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Уменьшение жизней
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int lowHP()
        {
            m_effect.onCreation(TimeSpan.Zero, m_position + new Vector2(m_hp * (m_size.X + c_spacing), 0), m_size);
            
            m_hp--;
            return m_hp;
        }
        ///--------------------------------------------------------------------------------------


      

      


         ///=====================================================================================
        ///
        /// <summary>
        /// Загрузка картинок
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            m_texture = content.Load<Texture2D>("Common/heart");
            m_size = new Vector2(m_texture.Width, m_texture.Height);
            m_sizeDiv2 = m_size / 2;
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
            if (m_hp > 0)
            {
                spriteBatch.begin();
                Vector2 pos = m_position;
                for (int i = 1; i < m_hp; i++)
                {
                    spriteBatch.Draw(m_texture, pos, Color.Red * 0.8f);
                    pos.X += m_size.X + c_spacing;
                }

                /* последние сердце сделаем пульсирующим
                 */
                float trans = (float)(gameTime.TotalMilliseconds / c_flash);
                m_zoom += trans * m_zoomSide;
                if (m_zoom < c_zoomMin)
                {
                    m_zoom = c_zoomMin;
                    m_zoomSide = -m_zoomSide;
                }
                if (m_zoom > c_zoomMax)
                {
                    m_zoom = c_zoomMax;
                    m_zoomSide = -m_zoomSide;
                }

                spriteBatch.Draw(m_texture,
                                            pos + m_sizeDiv2, 
                                            null,
                                            Color.White,
                                            0, //rotation
                                            m_sizeDiv2, m_zoom, 0, 0);

                spriteBatch.end();
            }//m_hp > 0
         
          

        }//draw





    }//ABackground
}