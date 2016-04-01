using System;
using System.Collections.Generic;





/// <typeparam name="T">Singleton class</typeparam>
public class ASingleton<T> where T : class, new()
{
    /// Защищённый конструктор необходим для того, чтобы предотвратить создание экземпляра класса Singleton. 
    /// Он будет вызван из закрытого конструктора наследственного класса.
    protected ASingleton() 
    { 
    
    }

    private static T _instance = null;
    public static T instance
    {
        get 
        { 
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

}