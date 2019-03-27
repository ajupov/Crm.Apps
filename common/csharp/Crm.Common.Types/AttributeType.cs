namespace Crm.Common.Types
{
    public enum AttributeType : byte
    {
        None = 0,
        Bool = 1,
        Byte = 2,
        Short = 3,
        UnsignedShort = 4,
        Integer = 5,
        UnsignedInteger = 6,
        Long = 7,
        UnsignedLong = 8,
        Decimal = 10,
        Single = 11,
        Double = 12,
        //Промежуток
        Date = 20,
        Time = 21,
        DateTime = 22,
        //Промежуток
        Email = 30,
        Phone = 31,
        Address = 32,
        Geolocation = 33,
        Image = 34,
        Документ = 35,
        Link = 36,
        SocialLink = 37,
        ImageLink = 38,
        DocumentLink = 39,
        BankAccount = 40,
        //Промежуток
        //Биометрические данные
        //Промежуток
        Json = 60,
        Xml = 61,
        Html = 63,
        Text = 64
    }
}