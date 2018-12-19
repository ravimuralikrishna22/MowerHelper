using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace MowerHelper.Models.BLL.MessageStation
{
    public class Ms_UserCollection : CollectionBase
    {
        public int Add(Ms_user value)
        {
            return List.Add(value);
        }
    }
}