using System.Collections.Generic;
using Cnf;
using System;
using MemoryPack;

namespace aaa
{
    [MemoryPack.MemoryPackable]
    public partial class Animal
    {
        public int Age { get; set; }
        public string Name { get; set; }
    
        public List<ItemInfo> repairs;
        
        [MemoryPackInclude]
        internal int cccc;

        public Animal()
        {
        }
    }
    
    [MemoryPack.MemoryPackable]
    public partial class Person : Animal
    {
        public int aaa;
    } 
    
    [MemoryPack.MemoryPackable]
    public partial class Women : Person
    {
        public int bbbb;
    } 
}
