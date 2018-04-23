using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CallbackException
/// </summary>
public class CallbackException : Exception
{
    public CallbackException(string message)
        : base(message)
    {
    }
}