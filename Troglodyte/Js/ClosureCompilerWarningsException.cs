using System;
using System.Text;
using com.google.javascript.jscomp;

namespace Troglodyte.Js
{
    public class ClosureCompilerWarningsException : Exception
    {
        private readonly JSError[] _errors;

        public ClosureCompilerWarningsException(JSError[] errors)
        {
            _errors = errors;
        }

        public override string Message
        {
            get
            {
                var sb = new StringBuilder("Closure Compiler Warnings: ");
                foreach (var error in _errors)
                {
                    sb.AppendLine(String.Format("{0}, line {1}, char {2}: {3}", error.sourceName, error.lineNumber, error.getCharno(), error.description));
                }
                return sb.ToString();
            }
        }
    }
}