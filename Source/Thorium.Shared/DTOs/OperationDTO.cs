﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Thorium.Shared.DTOs
{
    public class OperationDTO
    {
        public string Id { get; set; }
        public string OperationType { get; set; }
        public object OperationData { get; set; }
    }
}
