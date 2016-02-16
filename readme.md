# PlanMart

For this homework we are creating the next great online shopping super store PlanMart! We need you to 
implement the order processing and validation system.  You are provided with a set of classes representing
an `Order` and are expected to create an implementation of the interface `IOrderProcessor` (C#) or 
`OrderProcessor` (Java).  This interface simply defines one method:

**C#**

    bool ProcessOrder(Order order);

**Java**

	boolean processOrder(Order order);
	
## Context
Imagine you are at the standard stage of an online ordering process where you click "checkout" and are taken to a confirmation 
screen that summarizes the products in your cart, shipping costs, taxes, etc.  This page would typically have a button, "Buy Now", 
which would execute the order.  This method represents the work needed to create this confirmation page.   
	
You are expected to perform two tasks in this method:
* Return `true` or `false` depending on whether the order is valid
* Add zero or more line items to the order representing details such as taxes, shipping costs, and reward points.

## What We're Looking For

Ideally when you're done you have something that works, and the code is designed in a fashion you'd be proud to show to others. Also,
when contemplating your design, allow that there may be future implementations of the interface for different companies and that the
requirements between those implementations may have a lot of similarities. 

Set aside about two hours to work on this.  It is not a problem if you don't have enough time to implement every nuance.

## Specification

Here are the rules the implementation must enforce (feel free to implement any subset of these in the allocated time):

* All items are taxed at 8% unless exempt
* The following types of items are exempt from tax:
    * Food items shipped to CA, NY
    * Clothing items shipped to CT
* Orders to nonprofits are exempt from all tax and shipping
* Orders get 1 reward point per $2 spent
* Orders get double rewards points when:
    * Using PlanMart rewards credit card
    * Three of these criteria met:
        * Multiple different products in the same order
        * Orders over $200 shipped to US regions other than AZ
        * Orders over $100 shipped to AZ
        * Orders on:
            * Any of the [3 recurring Black Fridays](https://en.wikipedia.org/wiki/List_of_Black_Fridays#Repetitive_events)
            * Memorial Day
            * Veteran’s Day
* Alcohol may not be shipped to VA, NC, SC, TN, AK, KY, AL
* Alcohol may only be shipped to customers age 21 or over in the US
* Shipping is $10 for orders under 20 pounds in the continental US
* Shipping is $20 for orders 20 pounds or over in the continental US
* Shipping for orders to the non-continental US is $35
* Food may not be shipped to HI
* An `Order` should not be empty (the customer should be ordering *something*!)
* Sales tax should be rounded using the [round half up strategy](https://en.wikipedia.org/wiki/Rounding#Round_half_up)

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

## Example

    Order
		ShippingRegion: AZ
		PaymentMethod: Visa
		Placed: 2015-11-27
		Items: 
			ProductOrder
				Product
					Type: Alcohol
					Price: $20
					Weight: 2 lb
				Quantity: 3
			ProductOrder
				Product
					Type: Food
					Price: $25
					Weight: 3 lb
				Quantity: 3
		Customer
			BirthDate: 1988-03-04
			IsNonProfit: false
		
The processor should return `true`:
* While alcohol is involved, the customer is over the age of 21
* The order is not being shipped to any of VA, NC, SC, TN, AK, KY, or AL

The processor should add the following line items to the order:

    Order
		LineItems
			LineItem
				Type: Tax
				Amount: $10.80
			LineItem
				Type: Shipping
				Amount: $10.00
			LineItem
				Type: RewardsPoints
				Amount: 134
				
* A tax of $10.80 is 8% of the order total of $135
* The total weight of the order is less than 20 lb so shipping is $10
* $135 was spent, which represent 74 $2 chunks, giving us 74 reward points as a base.  But the amount is doubled to 134 because:
  * The order was shipped to Arizona and the order total was greater than $100
  * More than one each of two different products were ordered
  * The order was placed on the day after Thanksgiving (a Black Friday)   

## Unit Tests

In addition to implementing the processor interface, you should also write unit tests that verify that your processor is 
producing the results expected of the specification.  Each unit test should instantiate and populate an `Order` and call
your implementation and validate the results.

