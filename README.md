🏦 BankTechTest – Backend

Este proyecto corresponde al backend del sistema Inventario desarrollado en .NET 8 + SQL Server.
Incluye autenticación con JWT, manejo de usuarios, productos, proveedores, compras y wishlist.

⚙️ Requisitos previos

Antes de ejecutar el proyecto asegúrate de tener instalado:

.NET 8 SDK

SQL Server (local o remoto)

SQL Server Management Studio (SSMS) o cualquier cliente SQL para ejecutar scripts

Git (opcional, para clonar el repo)

🗄️ Base de datos

Crear una base de datos en SQL Server llamada:

CREATE DATABASE InventarioDB;


Ejecutar el script proporcionado por el creador del proyecto (/Database/InventarioDB.sql o el archivo que se te entregue).
Esto creará las tablas, procedimientos almacenados (SPs) y datos iniciales necesarios.

Verifica que los procedimientos como usp_Usuarios_Create, usp_ProductoProveedor_GetAll, etc., estén creados en la base.

🔑 Configuración

Abre el archivo Api/appsettings.json y ajusta la cadena de conexión si tu SQL Server no usa la configuración por defecto:

"ConnectionStrings": {
"Default": "Server=localhost,1433;Database=InventarioDB;User Id=sa;Password=TuPassword;TrustServerCertificate=True;"
}

▶️ Ejecución del backend

En la raíz del proyecto, compila y corre la API:

dotnet build
dotnet run --project Api


Por defecto, la API se levantará en:

http://localhost:5267

📖 Endpoints principales

Una vez levantada la API puedes probar los endpoints desde Swagger en:

http://localhost:5267/swagger


Ejemplos de endpoints disponibles:

POST /api/Auth/login → Login de usuario

POST /api/Auth/register → Registro de usuario

GET /api/ProductoProveedor → Listar productos con proveedor

POST /api/Compras → Registrar compra

GET /api/Usuarios → Listar usuarios

👤 Roles de usuario

Administrador → Puede crear, editar y eliminar usuarios, productos y proveedores.

Comprador → Puede listar productos y registrar compras.

Proveedor → Puede gestionar sus propios productos.

🧑‍💻 Desarrollo

Para compilar en modo desarrollo:

dotnet watch run --project Api


Esto levantará el servidor con hot reload.

👉 Con este README cualquier persona sabrá:

Qué necesita instalar.

Cómo crear la base de datos.

Cómo ejecutar tu backend.

Dónde encontrar y probar los endpoints.