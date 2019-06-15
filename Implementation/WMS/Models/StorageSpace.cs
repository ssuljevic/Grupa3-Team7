﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Models
{
    public class StorageSpace
    {
        public string Id  { get; set; }
        public double Capacity { get; set; }
        public double UsedUp { get { return 0; } }
        public ICollection<Item> Items { get; set; }

        [ForeignKey("Firm")]
        public IdentityUser IdentityUser{ get; set; }
    }
}