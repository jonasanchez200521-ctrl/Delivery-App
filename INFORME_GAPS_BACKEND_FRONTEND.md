# Informe: funcionalidades del backend (Nami.API) sin implementar en el frontend (Nami.Web)

Comparación endpoint por endpoint entre los controllers de `Nami.API` y las páginas/servicios de `Nami.Web`, verificando no solo que exista un método "wrapper" en `Services/*ApiService.cs`, sino que ese método se invoque realmente desde algún `.razor`.

## 🔴 Gaps funcionales reales (endpoint existe, capacidad no disponible al usuario)

### 1. Editar producto — no existe en el frontend
- **Backend:** `PUT /api/products/{id}` (`ProductsController.Update`, rol Administrator).
- **Frontend:** `CatalogApiService.UpdateProduct(id, request)` está definido pero **ningún `.razor` lo llama**.
- **Impacto:** en `AdminProducts.razor` solo hay "Crear" y "Eliminar". Para corregir un precio, stock, descripción o cambiar de categoría, el admin debe borrar el producto y crearlo de nuevo (perdiendo su Id e historial de referencias en pedidos ya emitidos, lo cual además puede romper la integridad si el producto está referenciado).
- **Adicional:** el formulario de creación tampoco tiene campo para `ImageUrl`, así que ningún producto creado desde el panel admin tiene imagen.

### 2. Editar restaurante — solo se puede activar/desactivar y borrar
- **Backend:** `PUT /api/restaurants/{id}` (`RestaurantsController.Update`, rol Administrator) permite actualizar `Name`, `Address`, `Category`, `Status`, `Rating`, `ImageUrl`, `Latitude`, `Longitude`.
- **Frontend:** `CatalogApiService.UpdateRestaurant` solo se usa desde `ToggleStatusAsync` en `AdminRestaurants.razor`, que **reenvía los mismos valores existentes y solo cambia `Status`**. No hay ningún formulario para editar nombre, dirección, categoría, rating, imagen o coordenadas.
- **Impacto real:** el formulario de "Nuevo restaurante" tampoco pide `Latitude`/`Longitude` (quedan en 0,0 por defecto) ni `ImageUrl`. Como `OrderDetail.razor` dibuja un mapa usando `RestaurantLatitude`/`RestaurantLongitude`, **todo restaurante creado desde el panel admin actual sale con el pin del mapa en (0,0)** y sin foto, y no hay forma de corregirlo desde la UI — solo directo en base de datos.

### 3. Desbloqueo de cuenta (`unlock-account`) — no implementado en absoluto
- **Backend:** `POST /api/auth/unlock-account` (`AuthController.UnlockAccount`, rol Administrator) — reactiva una cuenta vía email.
- **Frontend:** no existe ni el wrapper en `AuthApiService` ni ningún botón/formulario en `AdminUsers.razor`.
- **Nota:** hoy el "Bloquear/Activar" de `AdminUsers.razor` usa `PATCH /api/users/{id}/status`, que logra un efecto equivalente (reactivar), así que el impacto funcional es bajo, pero el endpoint dedicado a desbloqueo (pensado para el flujo de cuenta bloqueada por intentos fallidos, con búsqueda por email en vez de por Id) sigue sin usarse.

### 4. Logout de servidor no se invoca
- **Backend:** `POST /api/auth/logout` (`AuthController.Logout`, requiere token).
- **Frontend:** el botón "Salir" en `NavMenu.razor` solo hace `AuthState.Clear()` (limpieza local) y nunca llama al endpoint.
- **Impacto:** bajo — el propio backend documenta que es un no-op porque el JWT es *stateless* (`Logout` en `AuthService.cs` solo retorna `Task.CompletedTask`). Aun así, es un endpoint expuesto que el cliente nunca toca; si en el futuro se agrega invalidación de tokens (deny-list, refresh tokens, etc.), el frontend no está preparado para dispararlo.

### 5. Detalle de producto individual — endpoint sin consumidor
- **Backend:** `GET /api/products/{id}` (`ProductsController.GetById`).
- **Frontend:** no hay ningún wrapper en `CatalogApiService` ni llamada a esa ruta. El listado (`GetProducts`) ya trae todos los campos, así que hoy no hay una necesidad funcional evidente, pero el endpoint está huérfano.

## 🟡 Zonas ya cubiertas (verificado, no son gaps)

Se revisó explícitamente y **sí están** correctamente conectados de extremo a extremo:
- Auth: login, registro, recuperar contraseña.
- Carrito: agregar/actualizar/eliminar ítem, vaciar carrito.
- Checkout → creación de pedido con dirección, método de pago y cupón.
- Pedidos: mis pedidos (cliente), mis entregas y disponibles (delivery), aceptar/rechazar, todos los pedidos (admin), detalle, cambiar estado, asignar repartidor.
- Pagos: consulta de pago por pedido (mostrado en detalle de pedido — verificar si se usa visualmente, ver nota abajo).
- Usuarios: listar clientes/repartidores/administradores, crear repartidor/admin, cambiar estado, disponibilidad propia y por admin.
- Categorías: listar, crear, eliminar.
- Restaurantes: listar, detalle, crear (parcial, ver gap #2), eliminar.
- Promociones: listar, crear, eliminar, validación de cupón en checkout.
- Notificaciones: listar propias, marcar como leída.
- Reportes: dashboard con totales, pedidos por estado, usuarios por tipo.

## 🟢 Nota menor (no es un gap de funcionalidad, sino de UI)

- `OrderApiService.GetPayment(orderId)` existe y envuelve `GET /api/payments/order/{orderId}`, pero conviene confirmar visualmente si `OrderDetail.razor` efectivamente lo consume y muestra el resultado — no se encontró una llamada a `GetPayment` en ese archivo durante esta revisión, lo que sugiere que el estado del pago (`PaymentStatus`, método, fecha) nunca se le muestra al usuario en la pantalla de detalle del pedido, a pesar de que el servicio ya está listo para hacerlo.

---

## Resumen priorizado

| # | Funcionalidad | Backend listo | Frontend | Prioridad sugerida |
|---|---|---|---|---|
| 1 | Editar producto (incluye imagen) | ✅ | ❌ | Alta |
| 2 | Editar restaurante (nombre, dirección, categoría, imagen, lat/long, rating) | ✅ | ❌ (solo status) | Alta — rompe el mapa de pedidos |
| 3 | Mostrar estado/detalle de pago en `OrderDetail.razor` | ✅ (servicio ya wrapea) | ⚠️ no confirmado su uso en UI | Media |
| 4 | Desbloquear cuenta vía `unlock-account` | ✅ | ❌ | Baja (hay alternativa funcional) |
| 5 | Logout contra servidor | ✅ (no-op) | ❌ | Baja |
| 6 | Detalle de producto individual (`GET /products/{id}`) | ✅ | ❌ (sin caso de uso claro) | Baja |
