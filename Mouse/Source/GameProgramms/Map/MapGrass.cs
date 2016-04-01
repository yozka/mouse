using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mouse.GameProgramms.GameObjects;
using Mouse.ScreenSystem;

namespace Mouse.GameProgramms.Map
{
    ///--------------------------------------------------------------------------------------








     ///=====================================================================================
    ///
    /// <summary>
    /// Карта первого уровня, с травкой
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    class AMapGrass : AMap
    {
            //текстуры для отрисовки карты
        private Texture2D m_textureFloor;
        private Vector2 m_sizeFloor;
        private float m_rotate_x = -10;
        private float m_rotate_y = -10;

        private Texture2D m_textureBottom;
        private Vector2 m_sizeBottom;
        private float m_shiftBottom = 0;//смещение с учетом акселерометра
       
       ///--------------------------------------------------------------------------------------






        

         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AMapGrass(Vector2 sizeMap)
           : base(sizeMap)
        {

        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
       ///
       /// <summary>
       /// Загрузка контента для карты
       /// </summary>
       /// 
       ///--------------------------------------------------------------------------------------
       protected override void onLoadContent(ContentManager content)
       {
           base.onLoadContent(content);
           m_textureFloor = content.Load<Texture2D>("Common/Grass");
           m_sizeFloor = new Vector2(m_textureFloor.Width, m_textureFloor.Height);

           m_textureBottom = content.Load<Texture2D>("Common/MapBottom");
           m_sizeBottom = new Vector2(m_textureBottom.Width, m_textureBottom.Height);
       }
       ///--------------------------------------------------------------------------------------
        




        
         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка карты, и фона карты
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
       public override void onDraw(TimeSpan gameTime, ASpriteBatch spriteBatch)
       {
           spriteBatch.begin();
           Vector2 pos = new Vector2(m_rotate_x, m_rotate_y);
           while (pos.Y < size.Y)
           {
               while (pos.X < size.X)
               {
                   spriteBatch.Draw(m_textureFloor, pos, Color.White);
                   pos.X += m_sizeFloor.X;
               }
               pos.Y += m_sizeFloor.Y;
               pos.X = m_rotate_x;
           }
           spriteBatch.end();

           base.onDraw(gameTime, spriteBatch);
       }
       ///--------------------------------------------------------------------------------------






        ///=====================================================================================
       ///
       /// <summary>
       /// Отрисовка Нижней части карты
       /// </summary>
       /// 
       ///--------------------------------------------------------------------------------------
       public override void onAfterDraw(TimeSpan gameTime, ASpriteBatch spriteBatch)
       {
           spriteBatch.begin();
           Vector2 pos = new Vector2(m_shiftBottom, size.Y - m_sizeBottom.Y);
           while (pos.X < size.X)
           {
               spriteBatch.Draw(m_textureBottom, pos, Color.White);
               pos.X += m_sizeBottom.X;
           }
           spriteBatch.end();
           base.onDraw(gameTime, spriteBatch);
       }
       ///--------------------------------------------------------------------------------------





        ///=====================================================================================
       ///
       /// <summary>
       /// Обработка акселерометра для эффекта положения карты
       /// </summary>
       /// 
       ///--------------------------------------------------------------------------------------
       public override void onAcceleration(Vector2 value)
       {
           m_shiftBottom = (value.X * -60) - 60;

       }

    }//AMapGrass
}
