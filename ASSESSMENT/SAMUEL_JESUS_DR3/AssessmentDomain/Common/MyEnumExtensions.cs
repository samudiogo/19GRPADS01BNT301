﻿using System;
using System.ComponentModel;

namespace AssessmentDomain.Common
{
    public static class MyEnumExtensions
    {
        public static string GetDescription<T>(this T val) where T : Enum
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
                .GetType()
                .GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}