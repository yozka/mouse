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
    /// Эффект для анимации исчезновение одной жизни у объекта
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AEffectHeartLife : AParticleEffect
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
            return 3;//effectHeartLife
        }
        ///--------------------------------------------------------------------------------------





        #region Блок 1, создание частиц
        ///--------------------------------------------------------------------------------------
        private TimeSpan m_lifeEffect = TimeSpan.Zero;//время жизни эффекта
        private TimeSpan m_sleep = TimeSpan.Zero; //время между частицами
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// constructor 2 этот эффект является отдельной штуковиной для эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AEffectHeartLife(AParticleManager manager, TimeSpan time)
            :
            base(manager)
        {
            m_lifeEffect = time;
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
            if (m_sleep.TotalMilliseconds < 50)
            {
                return true;
            }
            m_sleep = TimeSpan.Zero;

            
            /*
             * добовляем новые частицы
             */
            AGameHelper helper = AGameHelper.instance;

            for (int i = 0; i < 5; i++)
            {
                AParticle particle = newParticle();

                //положение частицы
                float posX = helper.random.Next(0, (int)size.X);
                float posY = helper.random.Next(0, (int)size.Y / 2);
                particle.position = position + new Vector2(posX, posY) - size / 2;

                //время жизни прозрачность
                particle.lifeTime = TimeSpan.FromMilliseconds(helper.random.Next(100, 1500));
                particle.timeTransition = particle.lifeTime;
                particle.transition = 0;
                particle.transparence = 1;

                //цвет
                Color eColor = Color.Red;
                eColor.R = (byte)helper.random.Next(100, 255);
                particle.color = eColor;

                //размеры
                particle.size = 0;
                particle.sizeMax = (float)helper.random.Next(50, 70) / 100.0f; //размер ччастицы максимальный
            }
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
        private Texture2D m_sprite; //спрайт для отрисовки частицы
        private float m_transitionShow = 0.0f;//дельта увелечение размеров частицы при появлении
        private Vector2 m_sizeDiv2;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// constructor 1 этот эффект является менеджером, присособленцем
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AEffectHeartLife(AParticleManager manager)
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
            m_sprite = content.Load<Texture2D>("Common/Particles/heart");
            m_sizeDiv2 = new Vector2(m_sprite.Width, m_sprite.Height) / 2;
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
            if (particle.transparence == 0.0f)
            {
                return;
            }
            float transitionDelta = (float)(gameTime.TotalMilliseconds /
                                           particle.timeTransition.TotalMilliseconds);
            particle.transition += transitionDelta;
            particle.transparence = 1 - MathHelper.Clamp(particle.transition, 0.0f, 1.0f);

            /* установим размер частицы*/
            if (particle.size < particle.sizeMax)
            {
                particle.size += m_transitionShow;
                particle.size = MathHelper.Clamp(particle.size, 0, particle.sizeMax);
            }

            particle.position.Y -= transitionDelta * 150;



            // отрисовка частицы
            spriteBatch.Draw(m_sprite,
                                         particle.position, null,
                                         particle.color * particle.transparence,
                                         0, m_sizeDiv2, particle.size, 0, 0);
        }

        #endregion


    }
}
