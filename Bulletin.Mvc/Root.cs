using System.Collections;
using System.Collections.Generic;

namespace Bulletin.Mvc
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Author
    {
        public string link { get; set; }
        public string name { get; set; }
    }

    public class Availability
    {
        public string raw { get; set; }
    }

    public class Bestseller
    {
        public string category { get; set; }
        public string link { get; set; }
    }

    public class Brand
    {
        public string link { get; set; }
        public string name { get; set; }
        public string refinement_display_name { get; set; }
        public string value { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Delivery
    {
        public Price price { get; set; }
        public string tagline { get; set; }
    }

    public class Department
    {
        public string link { get; set; }
        public string name { get; set; }
        public string refinement_display_name { get; set; }
        public string value { get; set; }
    }

    public class GiftGuide
    {
        public string link { get; set; }
    }

    public class OtherFormat
    {
        public string asin { get; set; }
        public string link { get; set; }
        public string title { get; set; }
    }

    public class Page
    {
        public int current_page { get; set; }
        public string next_page_link { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
        public string amazon_url { get; set; }
        public DateTime created_at { get; set; }
        public string page { get; set; }
        public DateTime processed_at { get; set; }
        public double total_time_taken { get; set; }
    }

    public class Pagination
    {
        public List<Page> pages { get; set; }
    }

    public class Price
    {
        public string link { get; set; }
        public string name { get; set; }
        public string refinement_display_name { get; set; }
        public string Value { get; set; }
        public string currency { get; set; }
        public bool is_free { get; set; }
        public string? Raw { get; set; }
        public string symbol { get; set; }
        public bool is_primary { get; set; }
        public string Asin { get; set; }
    }

    public class Price4
    {
        public string currency { get; set; }
        public bool is_primary { get; set; }
        public string name { get; set; }
        public string raw { get; set; }
        public string symbol { get; set; }
        public double value { get; set; }
        public bool? is_rrp { get; set; }
        public string asin { get; set; }
        public string link { get; set; }
    }

    public class Prime
    {
        public string link { get; set; }
        public string name { get; set; }
        public string refinement_display_name { get; set; }
        public string value { get; set; }
    }

    public class Refinements
    {
        public List<Brand> brand { get; set; }
        public List<Department> departments { get; set; }
        public List<Price> price { get; set; }
        public List<Prime> prime { get; set; }
        public List<Review> reviews { get; set; }
    }

    public class RequestInfo
    {
        public int credits_used_this_request { get; set; }
        public bool success { get; set; }
        public int topup_credits_remaining { get; set; }
    }

    public class RequestMetadata
    {
        public DateTime created_at { get; set; }
        public List<Page> pages { get; set; }
        public DateTime processed_at { get; set; }
        public double total_time_taken { get; set; }
    }

    public class RequestParameters
    {
        public string amazon_domain { get; set; }
        public List<string> categories { get; set; }
        public string excluded_sponsored { get; set; }
        public string max_page { get; set; }
        public string page { get; set; }
        public string search_term { get; set; }
        public string sort_by { get; set; }
        public string type { get; set; }
    }

    public class Review
    {
        public string link { get; set; }
        public string name { get; set; }
        public string refinement_display_name { get; set; }
        public string value { get; set; }
    }

    public class Root
    {
        public Pagination pagination { get; set; }
        public Refinements refinements { get; set; }
        public RequestInfo request_info { get; set; }
        public RequestMetadata request_metadata { get; set; }
        public RequestParameters request_parameters { get; set; }
        public List<SearchResult> search_results { get; set; }
    }

    public class SearchResult
    {
        public string? asin { get; set; }
        public List<Author>? authors { get; set; }
        public List<Category>? categories { get; set; }
        public Delivery? delivery { get; set; }
        public string? image { get; set; }
        public bool? is_prime { get; set; }
        public string? link { get; set; }
        public List<OtherFormat>? other_formats { get; set; }
        public string? page { get; set; }
        public int? position { get; set; }
        public int? position_overall { get; set; }
        public Price? Price { get; set; }
        public List<Price>? prices { get; set; }
        public double? rating { get; set; }
        public int? ratings_total { get; set; }
        public string? title { get; set; }
        public bool? kindle_unlimited { get; set; }
        public Availability? availability { get; set; }
        public Bestseller? bestseller { get; set; }
        public GiftGuide? gift_guide { get; set; }
    }
}
