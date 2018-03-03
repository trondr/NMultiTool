using System;
using NCmdLiner;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public class Folder2WxsCommandProvider : IFolder2WxsCommandProvider
    {
        private readonly IFolder2Wxs _folder2Wxs;

        public Folder2WxsCommandProvider(IFolder2Wxs folder2Wxs)
        {
            _folder2Wxs = folder2Wxs;
        }

        public Result<int> Folder2Wxs(HarvestInfo harvestInfo)
        {
            try
            {
                _folder2Wxs.Harvest(harvestInfo);
                return Result.Ok(0);
            }
            catch (Exception e)
            {
                return Result.Fail<int>(new NMultiToolException($"Failed to harvest folder '{harvestInfo.Path.Value}'. {e.Message}",e));
            }            
        }
    }
}