namespace MyUserName.Bll
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public class Compute
    {
        private static readonly string[] UserNames = new[]
        {
            "Hugh", "Pugh", "Barney", "McGrew", "Cuthbert", "Dibble", "Grub"
        };
        
        public IEnumerable<string> Get()
        {
            var rng = new Random();
            
            var result =  Enumerable.Range(1, 5).Select(index =>
                    UserNames[rng.Next(UserNames.Length)])
                .ToArray();

            return result;
        }
    }
}