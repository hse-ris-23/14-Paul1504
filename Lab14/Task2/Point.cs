using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4
{
    public class Point<T>
    {
        private object p;
        public T Data { get; set; }
        public bool IsFound { get; set; }

        public Point(T data)
        {
            Data = data;
            IsFound = false;
        } 
    }
}
