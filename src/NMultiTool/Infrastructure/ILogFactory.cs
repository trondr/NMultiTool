using System;
using Common.Logging;

namespace NMultiTool.Infrastructure
{
    public interface ILogFactory
    {
        ILog GetLogger(Type type);
    }
}