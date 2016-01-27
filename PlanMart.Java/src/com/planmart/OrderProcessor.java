package com.planmart;

/**
 * Interface defining the contract of the order processor, which is responsible for:
 *
 * * Validating that an order is allowed
 * * Adding line items to account for taxes, shipping costs, and reward points
 */
public interface OrderProcessor {
    /**
     * Processes and validates the order.  An order should have zero or more line items added to it, and
     * should return a boolean status indicating whether the order is valid.
     * @param order The order being processed.  The only modifications to the order should be to the line items property.
     * @return true if the order is valid, false if something about the order breaks a rule.
     */
    boolean processOrder(Order order);
}