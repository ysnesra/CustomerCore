﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CustomerCore.Context
{
    public partial class Districts
    {
        public Districts()
        {
            Address = new HashSet<Address>();
        }

        public int Id { get; set; }
        public int TownId { get; set; }
        public string District { get; set; }

        public virtual Towns Town { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}