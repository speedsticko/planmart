using System;

namespace PlanMart
{
    public class Customer
    {
        /// <summary>
        /// A two-letter abbreviation for the region in U.S. where the customer resides
        /// </summary>
        public string Region { get; }

        /// <summary>
        /// The date of birth of the customer 
        /// </summary>
        public DateTime BirthDate { get; }

        /// <summary>
        /// Is the customer representing a non-profit organization
        /// </summary>
        public bool IsNonProfit { get; }

        public Customer(string region, DateTime birthDate, bool isNonProfit)
        {
            Region = region;
            BirthDate = birthDate;
            IsNonProfit = isNonProfit;
        }
    }
}