﻿using DelitaTrade.Common.Interfaces;

namespace DelitaTrade.Common
{
    public class ExceptionMessages
    {
        private const string ObjectNotExists = "already exists in data base";

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
    }
}
