using System;

namespace Utilities.Singleton
{
    public class LazySingleton
    {
        /// <summary>
        /// 单例模式，适用于非 MonoBehavior 对象
        /// </summary>
        private static readonly Lazy<LazySingleton> Lazy = new Lazy<LazySingleton>(() => new LazySingleton());

        public static LazySingleton Instance => Lazy.Value;

        private LazySingleton() { }
    }
}
