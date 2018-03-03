using System.Text.RegularExpressions;
using NCmdLiner;
using NMultiTool.Library.Module.Commands.ConvertSvgToIco;

namespace NMultiTool.Library.Module.Commands.Folder2Wxs
{
    public class YesOrNoOrVar
    {
        private YesOrNoOrVar(string yesOrNo)
        {
            this.Value = yesOrNo;
        }

        public string Value { get; }

        public static implicit operator YesOrNoOrVar(string yesOrNo)
        {
            var result = Create(yesOrNo)
                .OnFailure(exception => throw exception);
            return result.Value;
        }

        public static Result<YesOrNoOrVar> Create(string yesOrNoOrVar)
        {
            var yesOrNoOrVarLower = yesOrNoOrVar?.ToLower();
            switch (yesOrNoOrVarLower)
            {
                case "yes":
                case "no":
                    return Result.Ok(new YesOrNoOrVar(yesOrNoOrVarLower));
                default:
                {
                    if(!string.IsNullOrWhiteSpace(yesOrNoOrVar) && Regex.IsMatch(yesOrNoOrVar, @"^var\.Win64$"))
                        return Result.Ok(new YesOrNoOrVar($"$({yesOrNoOrVar})"));
                    return Result.Fail<YesOrNoOrVar>(new NMultiToolException($"Invalid yes or no value: '{yesOrNoOrVar}'"));
                }
            }
        }
    }
}