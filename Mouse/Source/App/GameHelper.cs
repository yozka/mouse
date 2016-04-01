using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mouse.App
{


    public class AGameHelper : ASingleton<AGameHelper>
    {
        public Random random = new Random();
        
        
        /// Вызовет защищенный конструктор класса Singleton
        public AGameHelper() 
        {
            random.Next();
        

        }

    }

}
