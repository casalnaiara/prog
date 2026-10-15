-- Sprint 7 - Datos de prueba (opcional, solo para probar las pantallas)

USE cantina_padel;

-- Productos de prueba (el producto 2 "Papas fritas" ya existe con stock 150)
INSERT INTO producto (nombre, stock, stock_minimo, precio_compra, precio_venta, codigo, id_categoria, id_marca, activo)
VALUES ('Coca-Cola 500ml', 40, 10, 1000.00, 1800.00, '7790895000011', 1, 1, 1),
       ('Agua mineral 500ml', 25, 10, 500.00, 1000.00, '7790895000028', 1, NULL, 1),
       ('Alfajor (poco stock)', 3, 1, 600.00, 1000.00, '7790895000035', 3, NULL, 1),
       ('Producto sin stock', 0, 1, 300.00, 500.00, '7790895000042', 3, NULL, 1);

-- Un cliente activo extra para probar el fiado (el cliente 1 es Malena Gonzalez, ya activa)
INSERT INTO persona (nombre, apellido, telefono, email, direccion, activo, cuit)
VALUES ('Cliente', 'De Prueba', '3644000000', 'prueba@mail.com', 'Calle 123', 1, '20111111112');
INSERT INTO cliente (id_persona, activo) VALUES (LAST_INSERT_ID(), 1);
