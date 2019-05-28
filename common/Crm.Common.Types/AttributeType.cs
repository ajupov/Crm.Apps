namespace Crm.Common.Types
{
    public enum AttributeType : byte
    {
        None = 0,
        Tag = 1,
        Bool = 2,
        Byte = 3,
        Short = 4,
        UnsignedShort = 5,
        Integer = 6,
        UnsignedInteger = 7,
        Long = 8,
        UnsignedLong = 9,
        Decimal = 10,
        Single = 11,
        Double = 12,
        Date = 20,
        Time = 21,
        DateTime = 22,
        Email = 30,
        Phone = 31,
        Address = 32,
        Geolocation = 33,
        Image = 34,
        Document = 35,
        Link = 36,
        SocialLink = 37,
        ImageLink = 38,
        DocumentLink = 39,
        BankAccount = 40,
        Json = 60,
        Xml = 61,
        Html = 63,
        Text = 64
    }
}