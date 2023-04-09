namespace CustomerCore.ViewModels.AddressModels
{
    public class AddressModel
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public byte? CountryId { get; set; }
        public short? CityId { get; set; }
        public int? TownId { get; set; }
        public int? DistrictId { get; set; }
        public string PostalCode { get; set; }
        public string AddressText { get; set; }

    }
}
