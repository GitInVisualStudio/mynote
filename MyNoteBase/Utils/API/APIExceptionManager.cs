using MyNoteBase.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNoteBase.Utils.API
{
    public class APIExceptionManager
    {
        private static Type[] ExceptionTypes = new Type[]
        {
            typeof(MissingParametersException),
            typeof(WrongCredentialsException)
        };

        public static APIException FromID(int id)
        {
            foreach (Type t in ExceptionTypes)
                if ((int)t.GetField("CODE").GetRawConstantValue() == id)
                    return (APIException)t.GetConstructor(new Type[] { }).Invoke(new object[] { });

            return null;
        }
    }
}
