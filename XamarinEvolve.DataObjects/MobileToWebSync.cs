﻿using System;

namespace XamarinEvolve.DataObjects
{
    public class MobileToWebSync : BaseDataObject
    {
        public string UserId { get; set; }
        public string TempCode { get; set; }
        public DateTime Expires { get; set; }
    }
}
