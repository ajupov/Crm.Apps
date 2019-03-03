using System;
using System.Collections.Generic;
using System.Text;

namespace Crm.Common.Types
{
    public class Request<T>
    {
        public T Body { get; set; }
    }
}
