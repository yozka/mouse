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
    /// Эффект для анимации исчезновения одного сердечка
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AEffectHeartHP : AParticleEffect
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
            return 2;//effectHeartHP
        }
        ///--------------------------------------------------------------------------------------





        #region Блок 1, создание частиц



         ///=====================================================================================
        ///
        /// <summary>
        /// Обновление внутренней логики эффекта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override bool onCreation(TimeSpan time, Vector2 position, Vector2 size)
        {
            /*
             * добовляем новые частицы
             */
            AGameHelper helper = AGameHelper.instance;


            for (int i = 0; i < 30; i++)
            {
                AParticle particle = newParticle();

                //положение частицы
                float posX = helper.random.Next(0, (int)size.X);
                float posY = helper.random.Next(0, (int)size.Y);
                particle.position = position + new Vector2(posX, posY) - new Vector2(size.X, size.Y / 2);

                //время жизни прозрачность
                particle.lifeTime = TimeSpan.FromMilliseconds(helper.random.Next(500, 2000));
                particle.timeTransition = particle.lifeTime;
                particle.transition = 0;
                particle.transparence = 1;

                //цвет
                Color eColor = Color.Red;
                eColor.R = (byte)helper.random.Next(100, 255);
                particle.color = eColor;
                
                //размеры
                particle.size = 0;

                //ротация
                particle.rotation = 0;
                particle.rotationStyle = 1;
                if (helper.random.Next(2) == 1)
                {
                    particle.rotationStyle = -1;
                }
            }
            return false;
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
        private const float c_sizePoint = 1.0f;//размер частицы
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// constructor 1 этот эффект является менеджером, присособленцем
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AEffectHeartHP(AParticleManager manager)
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
            particle.transparence = 1 - MathHelper.Clamp(particle.transition, 0, 1);

            /* установим размер частицы*/
            if (particle.size < c_sizePoint)
            {
                particle.size += m_transitionShow;
                particle.size = MathHelper.Clamp(particle.size, 0, c_sizePoint);
            }

            particle.rotation += m_transitionShow * particle.rotationStyle;
            particle.position.Y += transitionDelta * 300;


            
            // отрисовка частицы
            float size = particle.size;
            spriteBatch.Draw(m_sprite,
                                         particle.position + m_sizeDiv2 * (c_sizePoint - size), null,
                                         particle.color * particle.transparence,
                                         particle.rotation, Vector2.Zero, size, 0, 0);
        }

        #endregion


    }
}
