resource "azurerm_servicebus_namespace" "orders_servicebus_namespace" {
  name                = "orders-ns"
  location            = var.location
  resource_group_name = azurerm_resource_group.orders_rg.name
  sku                 = "Standard"
}
