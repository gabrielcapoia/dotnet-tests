using System;

namespace NerdStore.Sales.Domain
{
    public class Roadmap
    {
        /* DEVELOPMENT OF THE SALES DOMAIN */

        /* ORDER - ORDER ITEM - VOUCHER */

        /*
            An order item represents a product and may contain more than one unit
            Regardless of the action, an item must always be valid:
                Possess: Product ID and Name, quantity between 1 and 15 units, value greater than 0

            An order while not started (payment process) is in draft state
            and must belong to a customer.

            1 - Add Item - ok
                1.1 - When adding an item it is necessary to calculate the total value of the order - ok
                1.2 - If an item is already on the list then you must add the item's quantity to the order - ok
                1.3 - The item must have between 1 and 15 units of the product - ok

            2 - Update Item
                2.1 - The item needs to be on the list to be updated - ok
                2.2 - An item can be upgraded containing more or less units than previously - ok
                2.3 - When updating an item it is necessary to calculate the total value of the order - ok
                2.4 - An item must remain between 1 and 15 units of the product - ok

            3 - Delete Item
                3.1 - The item must be on the list to be removed
                3.2 - When removing an item it is necessary to calculate the total value of the order

            A voucher has a unique code and the discount can be in percentage or fixed amount
            Use a flag indicating that an order had a discount voucher applied and the discount amount generated

            4 - Apply discount voucher
                4.1 - The voucher can only be applied if it is valid, for this:
                    4.1.1 - Must have a code
                    4.1.2 - The expiration date is greater than the current date
                    4.1.3 - The voucher is active
                    4.1.4 - The voucher has available quantity
                    4.1.5 - One of the discount forms must be filled in with a value above 0
                4.2 - Calculate the discount according to the type of voucher
                    4.2.1 - Voucher with percentage discount
                    4.2.2 - Voucher with discount in amounts (reais)
                4.3 - When the discount amount exceeds the order total, the order receives the value: 0
                4.4 - After applying the voucher, the discount must be recalculated after any modification of the list
                      of order items
        */

        /* REQUEST COMMANDS - HANDLER */

        /*
            The order handler will handle a command for each intent against the order.
            In all manipulated commands must be checked:

                If the command is valid
                If the order exists
                If the order item exists

            On order status change:

                Must be done via repository
                Must send an event

            1 - AddItemOrderCommand
                1.1 Check if it is a new order or in progress
                1.2 Check if the item has already been added to the list
        */
    }
}
