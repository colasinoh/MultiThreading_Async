using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryServiceFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            ICountry countryFactory = new ServiceFactory();

            IService service = countryFactory.CreateService(CountrySpecific.Japan);
            service.CalcDiscount();

            IService serviceJapan = countryFactory.CreateService(CountrySpecific.SouthKorea);
            serviceJapan.CalcDiscount();

            IService serviceSouthKorea = countryFactory.CreateService(CountrySpecific.Other);
            serviceSouthKorea.CalcDiscount();

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
        void CalcDiscount();
    }

    public class CountryJapan : IService
    {
        public void CalcDiscount()
        {
            Console.WriteLine("Here goes the calculation specific to Japan...");
        }
    }

    public class CountrySouthKorea : IService
    {
        public void CalcDiscount()
        {
            Console.WriteLine("Here goes the calculation specific to South Korea...");
        }
    }

    public class Country : IService
    {
        public void CalcDiscount()
        {
            Console.WriteLine("Here goes the general calculation for other countries...");
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
