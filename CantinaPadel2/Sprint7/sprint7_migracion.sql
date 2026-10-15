-- Sprint 7 - Ventas y Cuenta Corriente
-- Ejecutar UNA vez sobre la base cantina_padel (MySQL Workbench) antes de probar las pantallas.

USE cantina_padel;

-- 1) venta.estado: el dump tenía los valores '\nFiada' y '\nAnulada' (con salto de línea adentro),
--    por eso "Fiada" no se podía guardar. Se corrige el ENUM.
ALTER TABLE venta
    MODIFY estado ENUM('Pagada','Fiada','Anulada') NOT NULL DEFAULT 'Pagada';

-- 2) Todavía no existe el módulo de Caja ni el login, así que id_caja pasa a ser opcional en venta.
--    (id_usuario ya era opcional). Cuando exista caja, se vuelve a poner NOT NULL.
ALTER TABLE venta
    MODIFY id_caja INT NULL;

-- 3) pago_cuenta_corriente.medio_pago tenía los valores de ESTADO ('Pendiente','Parcial','Pagada')
--    en lugar de los medios de pago. Se corrige, y caja/usuario también quedan opcionales por ahora.
ALTER TABLE pago_cuenta_corriente
    MODIFY medio_pago ENUM('Efectivo','Tarjeta','Transferencia') NOT NULL DEFAULT 'Efectivo',
    MODIFY id_caja INT NULL,
    MODIFY id_usuario INT NULL;
