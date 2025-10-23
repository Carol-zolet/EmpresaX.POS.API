using System;

namespace EmpresaX.POS.API.Services.Exceptions
{
    public class CaixaJaFechadoException : Exception
    {
        public CaixaJaFechadoException(string message) : base(message) { }
    }
}


