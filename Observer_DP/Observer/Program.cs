using System;

namespace Observer
{
    /// <summary>
    ///
    /// An Observer is an object that wishes
    /// to be informed about events happening
    /// in the system.
    /// The entity generating the event
    /// is an observable.
    /// 
    /// </summary>

    public class FallsIllEventArgs
    {
        public string Address;
    }

    public class Person
    {
        public void CatchCold()
        {
            FallsIll?.Invoke(this, new FallsIllEventArgs {Address = "123 Sesame street"});
        }
        // The Observer pattern is basicly baked in 
        // the .Net infrastructure using the ideea of events
        public event EventHandler<FallsIllEventArgs> FallsIll; 
    }
    public class Program
    {
        static void Main(string[] args)
        {
            var person = new Person();
            person.FallsIll += CallDoctor; // CallDoctor Method is subscribeing to the event
            person.CatchCold();
            person.FallsIll -= CallDoctor;
        }

        private static void CallDoctor(object sender, FallsIllEventArgs e)
        {
            Console.WriteLine($"A doctor has been called at address: {e.Address}");
        }
    }
}
