package com.planmart;

import java.math.BigDecimal;

public class Product {
    private BigDecimal price;
    private BigDecimal weight;
    private ProductType type;

    public Product(BigDecimal price, BigDecimal weight, ProductType type) {
        this.price = price;
        this.weight = weight;
        this.type = type;
    }

    /**
     * Gets the unit price of the product.
     */
    public BigDecimal getPrice() {
        return price;
    }

    /**
     * Sets the unit price of the product.
     */
    public void setPrice(BigDecimal price) {
        this.price = price;
    }

    /**
     * Gets the weight in pounds of the product.
     */
    public BigDecimal getWeight() {
        return weight;
    }

    /**
     * Sets the weight in pounds of the product.
     */
    public void setWeight(BigDecimal weight) {
        this.weight = weight;
    }

    /**
     * Gets the class of product (alcohol, clothes, food, etc.).
     */
    public ProductType getType() {
        return type;
    }

    /**
     * Sets the class of product (alcohol, clothes, food, etc.).
     */
    public void setType(ProductType type) {
        this.type = type;
    }
}
