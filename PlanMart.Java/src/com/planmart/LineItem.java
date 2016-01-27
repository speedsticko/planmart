package com.planmart;

import java.math.BigDecimal;

/**
 * Represents adjustments made by the processor to indicate taxes, shipping costs, and awarded rewards points.
 */
public class LineItem {
    private LineItemType type;
    private BigDecimal amount;

    public LineItem(LineItemType type, BigDecimal amount) {
        this.type = type;
        this.amount = amount;
    }

    /**
     * Gets the type of line item (tax, shipping, rewards points)
     */
    public LineItemType getType() {
        return type;
    }

    /**
     * Sets the type of line item (tax, shipping, rewards points)
     */
    public void setType(LineItemType type) {
        this.type = type;
    }

    /**
     * Gets the amount of taxes, shipping costs, or rewards points added to the order.
     */
    public BigDecimal getAmount() {
        return amount;
    }

    /**
     * Sets the amount of taxes, shipping costs, or rewards points added to the order.
     */
    public void setAmount(BigDecimal amount) {
        this.amount = amount;
    }
}
