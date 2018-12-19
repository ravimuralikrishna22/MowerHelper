using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MowerHelper.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RetainHttpsAttribute : Attribute
    {
    }
}
