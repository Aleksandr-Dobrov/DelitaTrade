﻿using DelitaTrade.Common.Interfaces;

namespace DelitaTrade.Common
{
    public class ExceptionMessages
    {
        private const string ObjectNotExists = "already exists in data base";

        public const string ProtocolIsApproved = "Return protocol is approved and cannot be changed";

        public const string ProtocolIsChanged = "Return protocol is changed and cannot be approved";

        public static string IsExists(IExceptionName obj)
        {
            return $"{obj.Name} {ObjectNotExists}";
        }

        public static string NotFound(string obj)
        {
            return $"{obj} not found";
        }

        public static string ServiceNotAvailable(string obj)
        {
            return $"Service {obj} not available";
        }

        public static string NotAuthenticate(IExceptionName obj)
        {
            return $"User {obj.Name} not authenticate";
        }
        public static string NotAuthenticate()
        {
            return $"You not authenticate";
        }

        public static string InvalidEntry(IExceptionName obj)
        {
            return $"{nameof(InvalidEntry)}: {obj} {Environment.NewLine}Entry id must be set to default";
        }

        public static string NotInitialized(string obj)
        {
            return $"Component: {obj} must be first initialized";
        }

        public static string IncomeNotGreatestAmount()
        {
            return "Income can not be greatest that amount";
        }
    }
}
