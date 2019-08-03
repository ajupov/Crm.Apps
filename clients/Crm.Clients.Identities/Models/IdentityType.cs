using System.Linq;

namespace Crm.Clients.Identities.Models
{
    public enum IdentityType : byte
    {
        // 
        None,

        // На основе логина и пароля
        LoginAndPassword,

        // На основе Email и пароля
        EmailAndPassword,

        // На основе номера телефона и пароля
        PhoneAndPassword,

        // Промежуток

        // На основе смарт-карты
        SmartCard,

        // На основе USB-токена
        UsbToken,

        //Промежуток

        // На основе кровеносных сосудов руки
        BloodVessel,

        // На основе папиллярные узоров пальца
        PapillaryFingerPattern,

        // По радужной оболочке глаза
        Iris,

        // По сетчатке глаза
        Eyeretina,

        // По геометрии руки
        HandGeometry,

        // По геометрии лица
        FaceGeometry,

        // По голосу
        Voice,

        // По рукописному почерку
        Handwriting,

        // Промежуток

        // Через Вконтакте
        // Vkontakte
        Vkontakte,

        // Через Одноклассники
        // Odnoklassniki
        Odnoklassniki,

        // Через Instagram
        // Instagram
        Instagram
    }

    
}