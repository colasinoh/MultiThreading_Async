using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;


namespace CountryServiceFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            ICountry countryFactory = new ServiceFactory();

            IService service = countryFactory.CreateService(CountrySpecific.Other);
            Console.WriteLine(service.CalcDiscount());

            //test Other Countries Service
            var mockServiceOther = new Mock<Country>();
            mockServiceOther.Setup(m => m.CalcDiscount()).Returns(() => "Here goes the general calculation for other countries...");
            Console.WriteLine("Mock Country: " + mockServiceOther.Object.CalcDiscount());

            IService serviceJapan = countryFactory.CreateService(CountrySpecific.Japan);
            Console.WriteLine(serviceJapan.CalcDiscount());

            //test Japan Service
            var mockServiceJapan = new Mock<Country>();
            mockServiceJapan.Setup(m => m.CalcDiscount()).Returns(() => "Here goes the calculation specific to Japan...");
            Console.WriteLine("Mock Japan: " + mockServiceJapan.Object.CalcDiscount());

            IService serviceSouthKorea = countryFactory.CreateService(CountrySpecific.SouthKorea);
            Console.WriteLine(serviceSouthKorea.CalcDiscount());

            //test South Korea Service
            var mockServiceKorea = new Mock<Country>();
            mockServiceKorea.Setup(m => m.CalcDiscount()).Returns(() => "Here goes the calculation specific to South Korea...");
            Console.WriteLine("Mock Korea: " + mockServiceKorea.Object.CalcDiscount());

            //implement multiple interface in Service mocks
            //var serviceMock = new Mock<IService>();
            //var disposableService = serviceMock.As<IDisposable>();
            //disposableService.Setup(dS => dS.Dispose());

            Console.ReadKey();
        }
    }

    public enum CountrySpecific
    {
        Japan,
        SouthKorea,
        Other
    }

    public interface IService
    {
        string CalcDiscount();
    }

    public class CountryJapan : IService
    {
        public virtual string CalcDiscount()
        {
            return "Here goes the calculation specific to Japan...";
        }
    }

    public class CountrySouthKorea : IService
    {
        public virtual string CalcDiscount()
        {
            return "Here goes the calculation specific to South Korea...";
        }
    }

    public class Country : IService
    {
        public virtual string CalcDiscount()
        {
            return "Here goes the general calculation for other countries...";
        }
    }

    interface ICountry
    {
        IService CreateService(CountrySpecific country);
    }

    public class ServiceFactory : ICountry
    {
        public IService CreateService(CountrySpecific country)
        {
            switch (country)
            {
                case CountrySpecific.Japan:
                    return new CountryJapan();
                case CountrySpecific.SouthKorea:
                    return new CountrySouthKorea();
                default:
                    return new Country();
            }
        }
    }
}
