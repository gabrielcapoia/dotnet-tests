Feature: Order - Insert Item to shopping cart
	Like a user
	I want to insert a item to cart
	So I could buy it later

Scenario: Insert item with success in a new order
Given A product is in o display
And It is available in stock
And The user is logged in
When the user insert a item to the cart
Then the user will be redirect to purchase summary
And The order amount is equal to the inserted item amount

Scenario: Add items over limit
Given that a product is on display
And be available in stock
And the user is logged in
When User adds an item above the maximum allowed quantity
Then you will receive an error message mentioning that the limit amount has been exceeded

Scenario: Add existing item to cart
Given that a product is on display
And be available in stock
And the user is logged in
And The same product has already been added to the cart previously
When User adds a unit to cart
Then The user will be redirected to the purchase summary
And The number of items for that product will have been increased by one more unit
And The total value of the order will be the multiplication of the quantity of items by the unit value

Scenario: Add existing item where sum exceeds maximum limit
Given that a product is on display
And be available in stock
And the user is logged in
And The same product has already been added to the cart previously
When The user adds the maximum amount allowed to the cart
Then The user will be redirected to the purchase summary
And You will receive an error message mentioning that the limit quantity has been exceeded
