using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Mouse.App;
using Mouse.ScreenSystem;


namespace Mouse.Particle
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Эффект ввиде падающих звезд, тянующийся а объектом
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AEffectTrail : AParticleEffect 
    {




         ///=====================================================================================
        ///
        /// <summary>
        /// тип используемого эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override int type()
        {
            return 1;//effectTrail
        }
        ///--------------------------------------------------------------------------------------





   #region Блок 1, создание частиц
        ///--------------------------------------------------------------------------------------
        private TimeSpan m_lifeEffect = TimeSpan.Zero;//время жизни эффекта
        private Color m_baseColor1 = Color.Black;//Цвет частиц
        private Color m_baseColor2 = Color.Black;//Цвет частиц
        private TimeSpan m_sleep = TimeSpan.Zero; //время между частицами
        ///--------------------------------------------------------------------------------------

        





         ///=====================================================================================
        ///
        /// <summary>
        /// constructor 2 этот эффект является отдельной штуковиной для эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AEffectTrail(AParticleManager manager, TimeSpan time, Color baseColor1, Color baseColor2)
            :
            base(manager)
        {
            m_lifeEffect = time;
            m_baseColor1 = baseColor1;
            m_baseColor2 = baseColor2;
        }
        ///--------------------------------------------------------------------------------------



 


         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление внутренней логики эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override bool onCreation(TimeSpan time, Vector2 position, Vector2 size)
        {
            m_lifeEffect -= time;
            if (m_lifeEffect.TotalMilliseconds < 0)
            {
                return false;
            }
            m_sleep += time;
            if (m_sleep.TotalMilliseconds < 60)
            {
                return true;
            }
            m_sleep = TimeSpan.Zero;

            /*
             * добовляем новые частицы
             */
            AGameHelper helper = AGameHelper.instance;


            AParticle particle = newParticle();

            //положение частицы
            float posX = helper.random.Next(0, (int)size.X);
            float posY = helper.random.Next(0, (int)size.Y);
            particle.position = position + new Vector2(posX, posY) - (size / 2);
            
            //время жизни прозрачность
            particle.lifeTime = TimeSpan.FromMilliseconds(helper.random.Next(1000, 3000));
            particle.timeTransition = particle.lifeTime;
            particle.transition = 0;
            particle.transparence = 1;
            
            //цвет
            particle.color = Color.Lerp(m_baseColor1, m_baseColor2, (float)helper.random.Next(1, 100) / 100.0f);

            //размеры
            particle.size = 0;
            
            //ротация
            particle.rotation = 0;
            particle.rotationStyle = 1;
            if (helper.random.Next(2) == 1)
            {
                particle.rotationStyle = -1;
            }

            //вид частицы, какой исопльзоват спрайт для отрисовки
            particle.styleView = helper.random.Next(2);
            return true;
        }
        ///--------------------------------------------------------------------------------------
   #endregion






   #region Блок 2, управление частицами
        /*
         * **************************************************************************************
         *
         *  ниже идет реализация для присособленца
         *  запускается на уровне менеджера частиц
         */



        ///--------------------------------------------------------------------------------------
        private Texture2D m_sprite1; //спрайт для отрисовки частицы
        private Texture2D m_sprite2; //спрайт для отрисовки частицы
        private float m_transitionShow = 0.0f;//дельта увелечение размеров частицы при появлении
        private Vector2 m_sizeSprite;
        private const float c_sizePoint = 1.3f;//размер частицы
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// constructor 1 этот эффект является менеджером, присособленцем
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AEffectTrail(AParticleManager manager)
            :
            base(manager)
        {
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Виртуальный метод загрузки графического контента эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onLoadContent(ContentManager content)
        {
            m_sprite1 = content.Load<Texture2D>("Common/Particles/star");
            m_sprite2 = content.Load<Texture2D>("Common/Particles/star_fav");

            m_sizeSprite = new Vector2(m_sprite1.Width, m_sprite1.Height)/2;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// выполнение эффекта для общей группы
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime)
        {
            m_transitionShow = (float)(gameTime.TotalMilliseconds / 200.0f); 
                                          

        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// выполнение эффекта для конкретного объекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdateParticle(TimeSpan gameTime, AParticle particle)
        {
            particle.lifeTime -= gameTime;
            if (particle.lifeTime.TotalMilliseconds < 0)
            {
                //удалим частицу
                removeParticle(particle);
            }

            
            float transitionDelta = (float)(gameTime.TotalMilliseconds /
                                           particle.timeTransition.TotalMilliseconds);
            particle.transition += transitionDelta;
            particle.transparence = 1-MathHelper.Clamp(particle.transition, 0, 1);

            /* установим размер частицы*/
            if (particle.size < c_sizePoint)
            {
                particle.size += m_transitionShow;
                particle.size = MathHelper.Clamp(particle.size, 0, c_sizePoint);
            }

            particle.rotation += m_transitionShow * particle.rotationStyle;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onDrawParticle(TimeSpan gameTime, ASpriteBatch spriteBatch, AParticle particle)
        {
            //spriteBatch.Draw(m_sprite, particle.position, particle.color * particle.transparence);
            Texture2D sprite = particle.styleView == 0 ? m_sprite1 : m_sprite2; 
            
            float size = particle.size;
            /*spriteBatch.Draw(sprite,
                                         particle.position + m_sizeSprite * (c_sizePoint - size), null,
                                         particle.color * particle.transparence,
                                         particle.rotation, Vector2.Zero, size, 0, 0);
             */
            spriteBatch.Draw(sprite,
                                         particle.position, null,
                                         particle.color * particle.transparence,
                                         particle.rotation, Vector2.Zero, size, 0, 0);
        }

   #endregion


    }
}
