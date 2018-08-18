using System;
using System.Collections.Generic;
using System.Text;

namespace Thorium.Shared
{
    public enum FinalAction
    {
        TurnIn,
        Abandon,
        Fail,
    }

    public class ExecutionResult
    {
        public FinalAction FinalAction { get; }
        public string AdditionalInformation { get; }
        public ExecutionResult(FinalAction action, string info)
        {
            FinalAction = action;
            AdditionalInformation = info;
        }
    }
}
