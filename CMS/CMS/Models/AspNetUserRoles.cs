//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class AspNetUserRoles
    {
        [DisplayName("User ID")]
        public string UserId { get; set; }

        [DisplayName("ID of role")]
        public string RoleId { get; set; }

        [DisplayName("Note")]
        public string aaa { get; set; }
    
        public virtual AspNetRoles AspNetRoles { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
