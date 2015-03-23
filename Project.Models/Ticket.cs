using System;
using System.ComponentModel.DataAnnotations;
using Project.Models.Properties;
using Project.Models.Validators;

namespace Project.Models
{
    public class Ticket
    {
        public Ticket()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public long TicketId { get; set; }

        [StringLength(255, ErrorMessageResourceName = "TicketCountryStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Display(Name = "TicketCountryLabelText", ResourceType = typeof(Resources))]
        public string Country { get; set; }

        [StringLength(255, ErrorMessageResourceName = "TicketCityStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Display(Name = "TicketCityLabelText", ResourceType = typeof(Resources))]
        public string City { get; set; }

        [StringLength(255, ErrorMessageResourceName = "TicketPlaceDescriptionStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextMultilineValidator]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "TicketPlaceDescriptionRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketPlaceDescriptionLabelText", ResourceType = typeof(Resources))]
        public string PlaceDescription { get; set; }

        [StringLength(255, ErrorMessageResourceName = "TicketLookDescriptionStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextMultilineValidator]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "TicketLookDescriptionRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketLookDescriptionLabelText", ResourceType = typeof(Resources))]
        public string LookDescription { get; set; }

        [Range(0, 2, ErrorMessageResourceName = "TicketGenderRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "TicketGenderRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketGenderLabelText", ResourceType = typeof(Resources))]
        public short Gender { get; set; } // 1 - female, 2 - male, 0 - unknown

        [Range(1, 200, ErrorMessageResourceName = "TicketAgeMinRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketAgeMinLabelText", ResourceType = typeof(Resources))]
        public short? AgeMin { get; set; }

        [Range(1, 200, ErrorMessageResourceName = "TicketAgeMaxRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketAgeMaxLabelText", ResourceType = typeof(Resources))]
        public short? AgeMax { get; set; }

        [Range(1000, 2500, ErrorMessageResourceName = "TicketYearRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "TicketYearRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketYearLabelText", ResourceType = typeof(Resources))]
        public short Year { get; set; }

        [Range(1, 4, ErrorMessageResourceName = "TicketSeasonRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "TicketSeasonRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketSeasonLabelText", ResourceType = typeof(Resources))]
        public short Season { get; set; } // 1 - winter, 2 - spring, 3 - summer, 4 - autumn

        [Range(1, 12, ErrorMessageResourceName = "TicketMonthRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketMonthLabelText", ResourceType = typeof(Resources))]
        public short? Month { get; set; }

        [Range(1, 31, ErrorMessageResourceName = "TicketDayRangeValidationError", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TicketDayLabelText", ResourceType = typeof(Resources))]
        public short? Day { get; set; }

        [StringLength(255, ErrorMessageResourceName = "TicketAdditionalNoteStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextMultilineValidator]
        [Display(Name = "TicketAdditionalNoteLabelText", ResourceType = typeof(Resources))]
        public string AdditionalNote { get; set; }

        [StringLength(255, ErrorMessageResourceName = "TicketFirstnameStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Display(Name = "TicketFirstnameLabelText", ResourceType = typeof(Resources))]
        public string Firstname { get; set; }

        [StringLength(255, ErrorMessageResourceName = "TicketLastnameStringLengthValidationError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineValidator]
        [Display(Name = "TicketLastnameLabelText", ResourceType = typeof(Resources))]
        public string Lastname { get; set; }

        [Display(Name = "TicketLanguageLabelText", ResourceType = typeof(Resources))]
        public short Language { get; set; }  // 1 - ru, 2 - en

        [PastDate]
        [StoreRestrictedDate]
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
    }
}