// ReSharper disable VirtualMemberCallInConstructor
namespace Tripsters.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using Tripsters.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Friends = new HashSet<ApplicationUser>();
            this.Badges = new HashSet<Badge>();
            this.Trips = new HashSet<UserTrip>();
        }

        [Range(16, 99)]
        public int Age { get; set; }

        public string HomeTownId { get; set; }

        public Town HomeTown { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<ApplicationUser> Friends { get; set; }

        public ICollection<Badge> Badges { get; set; }

        public ICollection<UserTrip> Trips { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
