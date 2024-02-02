using System.ComponentModel;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ThisCompanyIsGettingLethal
{
    #region V1
    internal class PackageListing {
        [JsonPropertyName("name"), ReadOnly(true)]
        public string Name { get; set; }
        [JsonPropertyName("full_name"), ReadOnly(true)]
        public string FullName { get; set; }
        [JsonPropertyName("owner"), ReadOnly(true)]
        public string Owner { get; set; }
        [JsonPropertyName("package_url"), ReadOnly(true)]
        public string PackageUrl { get; set; }
        [JsonPropertyName("donation_link"), ReadOnly(true)]
        public string DonationLink { get; set; }
        [JsonPropertyName("date_created"), ReadOnly(true)]
        public string DateCreated { get; set; }
        [JsonPropertyName("date_updated"), ReadOnly(true)]
        public string DateUpdated { get; set; }
        [JsonPropertyName("uuid4"), ReadOnly(true)]
        public string UUID4 { get; set; }
        [JsonPropertyName("rating_score"), ReadOnly(true)]
        public string RatingScore { get; set; }
        [JsonPropertyName("is_pinned"), ReadOnly(true)]
        public string IsPinned { get; set; }
        [JsonPropertyName("is_deprecated"), ReadOnly(true)]
        public string IsDeprecated { get; set; }
        [JsonPropertyName("has_nsfw_content")]
        public bool HasNsfwContent { get; set; }
        [JsonPropertyName("categories"), ReadOnly(true)]
        public string Categories { get; set; }
        [JsonPropertyName("versions"), ReadOnly(true)]
        public string Versions { get; set; }
    }
    #endregion

    #region Experimental
    internal class PackageExperimental {
        [JsonPropertyName("namespace"), ReadOnly(true)]
        public string Namespace { get; set; }

        [JsonPropertyName("name"), MinLength(1), MaxLength(128), Required]
        public string Name { get; set; }

        [JsonPropertyName("full_name"), ReadOnly(true)]
        public string FullName { get; set; }

        [JsonPropertyName("owner"), ReadOnly(true)]
        public string Owner { get; set; }

        [JsonPropertyName("package_url"), ReadOnly(true)]
        public string PackageUrl { get; set; }

        [JsonPropertyName("date_created"), ReadOnly(true)]
        public string DateCreated { get; set; }

        [JsonPropertyName("date_updated"), ReadOnly(true)]
        public string DateUpdated { get; set; }

        [JsonPropertyName("rating_score"), Range(0, int.MaxValue)]
        public int RatingScore { get; set; }

        [JsonPropertyName("is_pinned")]
        public bool IsPinned { get; set; }

        [JsonPropertyName("is_deprecated")]
        public bool IsDeprecated { get; set; }

        [JsonPropertyName("total_downloads"), Range(0, int.MaxValue)]
        public int TotalDownloads { get; set; }

        [JsonPropertyName("latest"), Required]
        public PackageVersionExperimental Latest { get; set; }

        [JsonPropertyName("community_listings"), Required]
        public PackageListingExperimental[] CommunityListings { get; set; }
    }

    internal class PackageVersionExperimental {
        // string namespace;
        [JsonPropertyName("namespace"), ReadOnly(true)]
        public string Namespace { get; set; }

        [JsonPropertyName("name"), MinLength(1), MaxLength(128), Required]
        public string Name { get; set; }

        [JsonPropertyName("version_number"), MinLength(1), MaxLength(16), Required]
        public string VersionNumber { get; set; }

        [JsonPropertyName("full_name"), ReadOnly(true)]
        public string FullName { get; set; }

        [JsonPropertyName("description"), MinLength(1), MaxLength(128), Required]
        public string Description { get; set; }

        [JsonPropertyName("icon"), ReadOnly(true)]
        public string Icon { get; set; }

        [JsonPropertyName("dependencies"), ReadOnly(true)]
        public string[] Dependencies { get; set; }

        [JsonPropertyName("download_url"), ReadOnly(true)]
        public string DownloadUrl { get; set; }

        [JsonPropertyName("downloads"), Range(0, int.MaxValue)]
        public int Downloads { get; set; }

        [JsonPropertyName("date_created"), ReadOnly(true)]
        public string DateCreated { get; set; }

        [JsonPropertyName("website_url"), ReadOnly(true), MinLength(1), MaxLength(1024), Required]
        public string WebsiteUrl { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }

    internal class PackageListingExperimental {
        [JsonPropertyName("has_nsfw_content")]
        public bool HasNsfwContent { get; set; }

        [JsonPropertyName("categories"), ReadOnly(true)]
        public string[] Categories { get; set; }

        [JsonPropertyName("community"), ReadOnly(true)]
        public string Community { get; set; }

        [JsonPropertyName("review_status"), AllowedValues("unreviewed", "approved", "rejected")]
        public string ReviewStatus { get; set; }
    }
    #endregion
}
