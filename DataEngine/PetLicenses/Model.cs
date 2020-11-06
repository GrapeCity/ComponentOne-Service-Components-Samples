using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PetLicenses
{
    public class PetLicense
    {
        [JsonProperty("License Issue Date")]
        public DateTime IssueDate { get; set; }
        [JsonProperty("License Number")]
        public string LicenseNumber { get; set; }
        [JsonProperty("Animal's Name")]
        public string AnimalName { get; set; }
        public string Species { get; set; }
        [JsonProperty("Primary Breed")]
        public string PrimaryBreed { get; set; }
        [JsonProperty("Secondary Breed")]
        public string SecondaryBreed { get; set; }
        [JsonProperty("ZIP Code")]
        [JsonConverter(typeof(ZipCodeConverter))]
        public int? ZipCode { get; set; }
    }

    public class Location
    {
        public int Zip { get; set; }
        public string City { get; set; }
        public string County { get; set; }
    }

    public class ZipCodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override Object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string zip = (string)reader.Value;
            return (zip != null && zip.Length >= 5) ? Convert.ToInt32(zip.Substring(0, 5)) : (int?)null;
        }
    }
}