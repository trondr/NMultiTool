﻿using System.IO;
using Common.Logging;
using Common.Logging.Simple;
using NMultiTool.Library.Infrastructure;
using NMultiTool.Library.Module.Commands.ConvertAllSvgToIco;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;
using NUnit.Framework;

namespace NMultiTool.Tests.Module.Commands.ConvertSvgToIco
{
    [TestFixture()]
    public class LibraryImageMagicProviderTests
    {
        [Test()]
        public void CreateIconFromPngFilesTest()
        {
            var target = new ImageMagicProvider(new ConsoleOutLogger(GetType().Name,LogLevel.All,true,true,true,"yyyy-MM-dd-HH-mm-ss" ));
            ToDo.Implement(ToDoPriority.Critical, "trondr","Replace hardcoding of svg test file.");
            var iconInfo = new IconInfo(@"C:\Dev\github-trondr\NMultiTool\src\NMultiTool.Tests\Module\Commands\ConvertSvgToIco\address.svg", new []{16,24,32,48,64,72,128,256});
            File.Delete(iconInfo.IconFile.FullName);
            Assert.IsFalse(File.Exists(iconInfo.IconFile.FullName),"Icon file exists: " + iconInfo.IconFile.FullName);
            target.CreateIconFromPngFilesResized(iconInfo); 
            Assert.IsTrue(File.Exists(iconInfo.IconFile.FullName),"Icon file does not exists: " + iconInfo.IconFile.FullName);
            File.Delete(iconInfo.IconFile.FullName);
        }
    }
}