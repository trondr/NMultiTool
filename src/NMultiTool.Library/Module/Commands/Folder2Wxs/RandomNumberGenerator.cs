using System;

namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random _randomNumber;

        public RandomNumberGenerator()
        {
            _randomNumber = new Random(DateTime.Now.Millisecond);
        }

        public int GetRandomNumber()
        {
            return _randomNumber.Next(0x7fffffff);
        }
    }
}