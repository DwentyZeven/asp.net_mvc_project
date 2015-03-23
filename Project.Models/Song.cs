using System;
using System.ComponentModel.DataAnnotations;
using Project.Models.Properties;
using Project.Models.Validators;

namespace Project.Models
{
    public class Song
    {
        public Song()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public long SongId { get; set; }

        [StringLength(50, ErrorMessageResourceName = "SongTitleStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "SongTitleRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "SongTitleLabelText", ResourceType = typeof(Resources))]
        public string Title { get; set; }

        [StringLength(50, ErrorMessageResourceName = "SongSingerStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "SongSingerRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "SongSingerLabelText", ResourceType = typeof(Resources))]
        public string Singer { get; set; }

        [StringLength(255, MinimumLength = 10, ErrorMessageResourceName = "SongQuoteStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextMultilineValidator]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "SongQuoteRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "SongQuoteLabelText", ResourceType = typeof(Resources))]
        public string Quote { get; set; }

        [StringLength(255, MinimumLength = 10, ErrorMessageResourceName = "SongTranslationStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextMultilineValidator]
        [Display(Name = "SongTranslationLabelText", ResourceType = typeof(Resources))]
        public string Translation { get; set; }

        [Display(Name = "SongLanguageLabelText", ResourceType = typeof(Resources))]
        public short Language { get; set; }  // 1 - ru, 2 - en

        [PastDate]
        [StoreRestrictedDate]
        public DateTime CreatedOn { get; set; }

        public virtual User User { get; set; }
    }
}