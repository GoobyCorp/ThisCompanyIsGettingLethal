using System.ComponentModel;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ThisCompanyIsGettingLethal
{
    //internal class JsonMessages
    //{

    //}

    internal class PackageExperimental {
        [JsonPropertyName("namespace"), ReadOnly(true)]
        public string _namespace { get; set; }

        [JsonPropertyName("name"), MinLength(1), MaxLength(128), Required]
        public string name { get; set; }

        [JsonPropertyName("full_name"), ReadOnly(true)]
        public string full_name { get; set; }

        [JsonPropertyName("owner"), ReadOnly(true)]
        public string owner { get; set; }

        [JsonPropertyName("package_url"), ReadOnly(true)]
        public string package_url { get; set; }

        [JsonPropertyName("date_created"), ReadOnly(true)]
        public string date_created { get; set; }

        [JsonPropertyName("date_updated"), ReadOnly(true)]
        public string date_updated { get; set; }

        [JsonPropertyName("rating_score"), Range(0, int.MaxValue)]
        public int rating_score { get; set; }

        [JsonPropertyName("is_pinned")]
        public bool is_pinned { get; set; }

        [JsonPropertyName("is_deprecated")]
        public bool is_deprecated { get; set; }

        [JsonPropertyName("total_downloads"), Range(0, int.MaxValue)]
        public int total_downloads { get; set; }

        [JsonPropertyName("latest"), Required]
        public PackageVersionExperimental latest { get; set; }

        [JsonPropertyName("community_listings"), Required]
        public PackageListingExperimental[] community_listings { get; set; }
    }

    internal class PackageVersionExperimental {
        // string namespace;
        [JsonPropertyName("namespace"), ReadOnly(true)]
        public string _namespace { get; set; }

        [JsonPropertyName("name"), MinLength(1), MaxLength(128), Required]
        public string name { get; set; }

        [JsonPropertyName("version_number"), MinLength(1), MaxLength(16), Required]
        public string version_number { get; set; }

        [JsonPropertyName("full_name"), ReadOnly(true)]
        public string full_name { get; set; }

        [JsonPropertyName("description"), MinLength(1), MaxLength(128), Required]
        public string description { get; set; }

        [JsonPropertyName("icon"), ReadOnly(true)]
        public string icon { get; set; }

        [JsonPropertyName("dependencies"), ReadOnly(true)]
        public string[] dependencies { get; set; }

        [JsonPropertyName("download_url"), ReadOnly(true)]
        public string download_url { get; set; }

        [JsonPropertyName("downloads"), Range(0, int.MaxValue)]
        public int downloads { get; set; }

        [JsonPropertyName("date_created"), ReadOnly(true)]
        public string date_created { get; set; }

        [JsonPropertyName("website_url"), ReadOnly(true), MinLength(1), MaxLength(1024), Required]
        public string website_url { get; set; }

        [JsonPropertyName("is_active")]
        public bool is_active { get; set; }
    }

    internal class PackageListingExperimental {
        [JsonPropertyName("has_nsfw_content")]
        public bool has_nsfw_content { get; set; }

        [JsonPropertyName("categories"), ReadOnly(true)]
        public string[] categories { get; set; }

        [JsonPropertyName("community"), ReadOnly(true)]
        public string community { get; set; }

        [JsonPropertyName("review_status"), AllowedValues("unreviewed", "approved", "rejected")]
        public string review_status { get; set; }
    }
}
