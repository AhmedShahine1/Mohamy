﻿using Mohamy.Core.Entity.ConsultingData;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Mohamy.Core.Entity.Files
{
    [DebuggerDisplay("{Name,nq}")]
    public class Images
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        [ForeignKey("path")]
        public string pathId { get; set; }

        public Paths path { get; set; }


    }
}