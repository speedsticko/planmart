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
* An `Order` should not be empty (the customer should be ordering *something*!)

## Data Model

We provide the following types:
* **Order** - The parameter to the processor and the top level object with which you interact.  Contains various details such
as the products ordered, the shipping address, line items, etc.
* **Customer** - Details about the customer who placed the order, including the date of birth, and whether they are a non-profit.
* **Product** - Describes a single product (such as a book, a TV, etc.) with details that describe the product such as its weight
and price.
* **ProductType** - An enum that specifies what sort of product it is (alcohol, clothing, etc.)
* **PaymentMethod** - An enum that specifies what payment method the customer used (Visa, PlanMart Rewards Card, etc.)
* **LineItem** - Added to an `Order` by the processor to indicate taxes, shipping costs, and awarded reward points.
* **LineItemType** - An enum that specifies whether the line item represents taxes, shipping costs, etc.
* **ProductOrder** - An `Order` contains one of these that describes what product is being ordered and how many..

## Unit Tests

In addition to implementing the processor interface, you should also write unit tests that verify that your processor is 
producing the results expected of the specification.  Each unit test should instantiate and populate an `Order` and call
your implementation and validate the results.