using System;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        public AccountingModel()
        {
            price = 0;
            nightsCount = 1;
            discount = 0;
            total = 0;
        }
        
        private double price;
        public double Price
        {
            get => price;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException();
                }

                price = value;
                Notify(nameof(Price));
                total = RecalculateTotal(value, NightsCount, Discount);
                Notify(nameof(Total));
            }
        }
        
        private int nightsCount;
        public int NightsCount
        {
            get => nightsCount;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }

                nightsCount = value;
                Notify(nameof(NightsCount));
                total = RecalculateTotal(Price, value, Discount);
                Notify(nameof(Total));
            }
        }

        private double discount;
        public double Discount
        {
            get => discount;
            set
            {
                discount = value;
                Notify(nameof(Discount));
                total = RecalculateTotal(Price, NightsCount, discount);
                Notify(nameof(Total));
            }
        }

        private double total;
        public double Total
        {
            get => total;
            set
            {
                total = value;
                Notify(nameof(Total));
                discount = RecalculateDiscount(value);
                Notify(nameof(Discount));
                ValidateTotal(value);
            }
        }

        private double RecalculateTotal(double nextPrice, int nextNightsCount, double nextDiscount)
        {
            var nextTotal = nextPrice * nextNightsCount * (1 - nextDiscount / 100);
            ValidateTotal(nextTotal);

            return nextTotal;
        }

        private void ValidateTotal(double nextTotal)
        {
            if (nextTotal < 0 || Math.Abs(nextTotal - Price * NightsCount * (1 - Discount / 100)) > 10e-5)
            {
                throw new ArgumentException();
            }
        }
        
        private double RecalculateDiscount(double nextTotal)
        {
            return 100 * (1 - nextTotal / (Price * NightsCount));
        }
    }
}
