using System;

namespace NMultiTool.Library.Module.Commands.ConvertSvgToIco
{
    public class NMultiToolException : Exception
    {
        public NMultiToolException(): base() { }

        public NMultiToolException(string message): base(message) { }
    }
}