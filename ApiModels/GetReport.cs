namespace netCoreApi.ApiModels
{
    public class GetReport
    {
        public string? CountryName { get; set; }
        public int? AddresCount { get; set; } 
        public DateTime LstAddressUpdated{ get; set; }
    }
}
