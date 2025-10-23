using System;

namespace EmpresaX.POS.API.Services.Exceptions
{
    public class CaixaNaoEncontradoException : Exception
    {
        public CaixaNaoEncontradoException(string message) : base(message) { }
    }
}


