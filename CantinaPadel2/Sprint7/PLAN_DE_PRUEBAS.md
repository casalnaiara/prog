# Sprint 7 – Plan de pruebas (Ventas y Cuenta Corriente)

Antes de empezar: ejecutar `sprint7_migracion.sql` y (opcional) `sprint7_datos_prueba.sql`.

| # | Caso | Pasos | Resultado esperado |
|---|------|-------|--------------------|
| 1 | Venta al contado | Ventas → agregar 2 Coca-Cola → Consumidor final → Efectivo → Confirmar | Mensaje "Venta #N registrada". Stock de Coca-Cola baja 2. Carrito vacío. |
| 2 | Carrito acumula | Agregar el mismo producto dos veces | Una sola línea con la cantidad sumada; el total se actualiza. |
| 3 | Quitar / vaciar | Quitar una línea; luego "Vaciar carrito" | El total baja / queda en $0,00. |
| 4 | Stock insuficiente (carrito) | Agregar 4 "Alfajor (poco stock)" (stock 3) | Aviso de stock insuficiente, no se agrega. |
| 5 | Producto sin stock | Intentar agregar "Producto sin stock" | Aviso de stock insuficiente. |
| 6 | Carrito vacío | Confirmar sin productos | Aviso "El carrito está vacío". |
| 7 | Búsqueda | Escribir parte del nombre o un código | La grilla filtra. Con un código exacto + Enter se agrega solo. |
| 8 | Fiar sin cliente | Marcar fiar con "Consumidor final" | El check de fiar está deshabilitado. |
| 9 | Venta fiada | Elegir cliente → tildar Fiar → Confirmar | Venta estado "Fiada" (sin medio de pago), stock baja, aparece la deuda en Cuenta Corriente. |
| 10 | Deuda visible en ventas | Elegir un cliente con deuda | Se muestra "Deuda actual: $X". |
| 11 | Pago parcial de una línea | Cuenta Corriente → cliente → línea → monto menor al saldo → Pagar línea | Estado "Parcial", saldo baja, total del cliente baja. |
| 12 | Pago total de una línea | Pagar el saldo completo | Estado "Pagada"; desaparece del detalle (visible con "Incluir pagadas"). |
| 13 | Pago mayor al saldo | Monto > saldo de la línea | Error "supera el saldo pendiente", no se registra nada. |
| 14 | Pago a cuenta | Monto menor a la deuda total → "Pagar a cuenta" | Se descuenta de las deudas más viejas primero (la 1ª puede quedar Pagada y la 2ª Parcial). |
| 15 | Pago a cuenta > deuda total | Monto mayor a lo que debe | Error, no se registra nada. |
| 16 | Dos ventanas, mismo stock | Abrir 2 instancias, vender las 3 unidades de Alfajor en una y luego intentar vender en la otra | La segunda rechaza: "Stock insuficiente" (se valida en la transacción). |
| 17 | Cambio de precio | Con el carrito armado, cambiar el precio del producto desde Productos y confirmar | Error "el precio cambió"; vuelve a cargar. |

Verificación en la base después de las pruebas:

```sql
SELECT * FROM venta ORDER BY id_venta DESC;
SELECT * FROM detalle_venta ORDER BY id_detalle_venta DESC;
SELECT id_producto, nombre, stock FROM producto;
SELECT * FROM detalle_deuda;
SELECT * FROM pago_cuenta_corriente;
```
