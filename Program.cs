using System;

namespace Covid19_UnderNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            StatusSource.Instance.Calculate();
        }
    }
}
