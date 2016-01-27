package com.planmart;

import java.util.ArrayList;
import java.util.Date;

public class Order {
    private Customer customer;
    private String shippingRegion;
    private PaymentMethod paymentMethod;
    private Date placed;
    private ArrayList<ProductOrder> items = new ArrayList<>();
    private ArrayList<LineItem> lineItems = new ArrayList<>();

    public Order(Customer customer, String shippingRegion, PaymentMethod paymentMethod, Date placed) {
        this.customer = customer;
        this.shippingRegion = shippingRegion;
        this.paymentMethod = paymentMethod;
        this.placed = placed;
    }

    /**
     * Gets the customer who placed the order.
     */
    public Customer getCustomer() {
        return customer;
    }

    /**
     * Sets the customer who placed the order.
     */
    public void setCustomer(Customer customer) {
        this.customer = customer;
    }

    /**
     * Gets two-letter region where the order should be shipped to.
     */
    public String getShippingRegion() {
        return shippingRegion;
    }

    /**
     * Sets two-letter region where the order should be shipped to.
     */
    public void setShippingRegion(String shippingRegion) {
        this.shippingRegion = shippingRegion;
    }

    /**
     * Gets an enum describing the method of payment for the order.
     */
    public PaymentMethod getPaymentMethod() {
        return paymentMethod;
    }

    /**
     * Sets an enum describing the method of payment for the order.
     */
    public void setPaymentMethod(PaymentMethod paymentMethod) {
        this.paymentMethod = paymentMethod;
    }

    /**
     * Gets the date and time in UTC when the order was placed.
     */
    public Date getPlaced() {
        return placed;
    }

    /**
     * Sets the date and time in UTC when the order was placed.
     */
    public void setPlaced(Date placed) {
        this.placed = placed;
    }

    /**
     * Gets a list of items representing one or more products and the quantity of each.
     */
    public ArrayList<ProductOrder> getItems() {
        return items;
    }

    /**
     * Gets a list of line items that represent adjustments to the order by the processor (tax, shipping, etc.)
     */
    public ArrayList<LineItem> getLineItems() {
        return lineItems;
    }
}
