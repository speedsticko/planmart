# PlanMart

For this homework we are creating the next great online shopping super store PlanMart! We need you to 
implement the order processing and validation system.  You are provided with a set of classes representing
an `Order` and are expected to create an implementation of the interface `IOrderProcessor` (C#) or 
`OrderProcessor` (Java).  This interface simply defines one method:

**C#**

    bool ProcessOrder(Order order);

**Java**

	boolean processOrder(Order order);
	
You are expected to perform two tasks in this method:
* Return true or false depending on whether the order is valid
* Add zero or more line items to the order representing details such as taxes, shipping costs, and reward points.

## Specification

Here are the rules the implementation must enforce:

* All items are taxed at 8% unless exempt
* The following types of items are exempt from tax:
    * Food items shipped to CA, NY
    * Clothing items shipped to CT
* Orders to nonprofits are exempt from all tax and shipping
* Orders get 1 reward point per  $2 spent
* Orders get double rewards points when:
    * Using PlanMart rewards credit card
    * Three of these criteria met:
        * Multiple different products in the same order
        * Orders over $200 shipped to US territories other than AZ
        * Orders over $100 shipped to AZ
        * Orders on:
            * Any of the [3 recurring Black Fridays](https://en.wikipedia.org/wiki/List_of_Black_Fridays#Repetitive_events)
            * Memorial Day
            * Veteran’s Day
* Alcohol may not be shipped to VA, NC, SC, TN, AK, KY, AL
* Alcohol may only be shipped to customers age 21 or over in the US
* Shipping is $10 for items under 20 pounds in the continental US
* Shipping is $20 for items over 20 pounds in the continental US
* Shipping for items to the non-continental US is $35
* Food may not be shipped to HI

