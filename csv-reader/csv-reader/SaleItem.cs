using System;

namespace csv_reader
{
    public class SaleItem
    {
        public DateTime Date { get; private set; }
        public string Product { get; private set; }
        public long Quantity { get; private set; }

        public SaleItem(DateTime date, string product, long quantity)
        {
            this.Date = date;
            this.Product = product;
            this.Quantity = quantity;
        }
    }
}
