using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Project.Models.Properties;
using Project.Models.Validators;

namespace Project.Models
{
    public class User
    {
        public User()
        {
            Role = 2;
            IsWarned = false;
            IsBanned = false;
            CreatedAt = DateTime.UtcNow;
        }

        public long UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserFacebookIdRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UserFacebookIdLabelText", ResourceType = typeof(Resources))]
        public long FacebookId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserVkontakteIdRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UserVkontakteIdLabelText", ResourceType = typeof(Resources))]
        public long VkontakteId { get; set; }

        [StringLength(50, ErrorMessageResourceName = "UserFirstnameStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserFirstnameRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UserFirstnameLabelText", ResourceType = typeof(Resources))]
        public string Firstname { get; set; }

        [StringLength(50, ErrorMessageResourceName = "UserLastnameStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserLastnameRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UserLastnameLabelText", ResourceType = typeof(Resources))]
        public string Lastname { get; set; }

        [StringLength(50, ErrorMessageResourceName = "UserUsernameStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Display(Name = "UserUsernameLabelText", ResourceType = typeof(Resources))]
        public string Username { get; set; }

        [Range(0, 2, ErrorMessageResourceName = "UserGenderRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserGenderRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UserGenderLabelText", ResourceType = typeof(Resources))]
        public short Gender { get; set; } // 1 - female, 2 - male, 0 - unknown

        [StringLength(50, ErrorMessageResourceName = "UserEmailStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [EmailValidator]
        [Display(Name = "UserEmailLabelText", ResourceType = typeof(Resources))]
        public string Email { get; set; }

        [StringLength(50, ErrorMessageResourceName = "UserPasswordStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [PasswordValidator]
        [Display(Name = "UserPasswordLabelText", ResourceType = typeof(Resources))]
        public string Password { get; set; }

        [StringLength(255, ErrorMessageResourceName = "UserLinkStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserLinkRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UserLinkLabelText", ResourceType = typeof(Resources))]
        public string Link { get; set; }

        [StringLength(255, ErrorMessageResourceName = "UserPhotoLinkStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UserPhotoLinkLabelText", ResourceType = typeof(Resources))]
        public string PhotoLink { get; set; }

        [Range(1, 2, ErrorMessageResourceName = "UserRoleRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "UserRoleRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UserRoleLabelText", ResourceType = typeof(Resources))]
        public short Role { get; set; } // 1 - admin, 2 - user

        public bool IsOnline { get; set; }

        [Display(Name = "UserIsWarnedLabelText", ResourceType = typeof(Resources))]
        public bool IsWarned { get; set; }

        [Display(Name = "UserIsBannedLabelText", ResourceType = typeof(Resources))]
        public bool IsBanned { get; set; }

        [PastDate]
        [StoreRestrictedDate]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}