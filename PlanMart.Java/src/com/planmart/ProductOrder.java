package com.planmart;

/**
 * Contained by an Order and represents a single product type and the quantity of that product that was ordered.
 */
public class ProductOrder {
    private Product product;
    private int quantity;

    public ProductOrder(Product product, int quantity) {
        this.product = product;
        this.quantity = quantity;
    }

    /**
     * Gets the product that was ordered.
     */
    public Product getProduct() {
        return product;
    }

    /**
     * Sets the product that was ordered.
     */
    public void setProduct(Product product) {
        this.product = product;
    }

    /**
     * Gets the number of the product that was ordered.
     */
    public int getQuantity() {
        return quantity;
    }

    /**
     * Sets the number of the product that was ordered.
     */
    public void setQuantity(int quantity) {
        this.quantity = quantity;
    }
}
