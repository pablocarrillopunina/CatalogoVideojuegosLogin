# 🕹️ Catálogo de Videojuegos con Sistema de Login y Administración de Usuarios

**Prácticas 3 y 4 – Desarrollo de Interfaces (C# / Windows Forms)**

Este proyecto corresponde a la evolución de las **Prácticas 3 y 4**, donde se amplía el sistema de autenticación utilizando una **base de datos MySQL real**, se mantiene el **catálogo de videojuegos gestionado mediante XML** y se incorpora una **ventana de administración de usuarios con control de roles y operaciones CRUD**.

---

## ✨ Funcionalidades Implementadas

## 🔐 Sistema de Login con Roles (MySQL)
- Autenticación mediante consultas SQL reales.
- Validación de:
  - Usuario existente
  - Contraseña correcta
  - Estado del usuario (activo o baneado)
- Recuperación del **rol del usuario** durante el login.
- Control de acceso:
  - 👑 **Administrador** → acceso al catálogo y a la ventana de administración
  - 👤 **Usuario nominal** → acceso únicamente al catálogo
- Manejo de errores de conexión.
- Mensajes claros de éxito o error.

---

## 📝 Registro de Usuarios (MySQL)
- Inserción de nuevos usuarios en la base de datos (`INSERT INTO usuarios`).
- Validación de campos obligatorios.
- Registro de usuarios con rol por defecto **Usuario**.
- Confirmación visual del registro correcto.

---

## 👥 Ventana de Administración de Usuarios (Práctica 4)
Funcionalidad exclusiva para usuarios con rol **Administrador**.

### 🔧 Operaciones CRUD
- ➕ Crear nuevos usuarios.
- 📋 Listar todos los usuarios en un `DataGridView`.
- ✏️ Editar usuarios:
  - Actualizar contraseña
  - Actualizar email
- 🗑️ Eliminar usuarios de la base de datos.

### 🔍 Búsqueda
- Búsqueda de usuarios por `nombre_usuario`.

### 🚫 Control de Estado (Baneo)
- Posibilidad de **banear usuarios**.
- Cambio del estado del usuario en la base de datos.
- Los usuarios baneados no pueden iniciar sesión.

---

## 🎮 Catálogo de Videojuegos (XML)
Se mantiene la funcionalidad de las prácticas anteriores:

- Carga del catálogo desde archivo XML.
- Visualización en `DataGridView`.
- Gestión completa del catálogo:
  - ➕ Añadir videojuegos
  - ✏️ Modificar videojuegos
  - 🗑️ Eliminar videojuegos

Cada videojuego contiene:
- Título
- Desarrollador
- Plataforma
- Precio 💶

---

## 🔄 Gestión de Sesión
- Cerrar sesión y volver al formulario de login.
- Cierre seguro de la aplicación.

---

## 🛠️ Tecnologías Utilizadas
- **C# .NET 8**
- **Windows Forms**
- **MySQL 8**
- **MySql.Data**
- **XML** para el catálogo de videojuegos
- **GitHub** para control de versiones

---

## 📁 Estructura del Proyecto

<img width="328" height="348" alt="image" src="https://github.com/user-attachments/assets/3118f536-88c4-474f-b77c-c9bc198317b9" />


---

## 🧪 Testing – Prácticas 3 y 4

### 🔐 Autenticación y Roles
- ✔️ Login exitoso
- ✔️ Usuario inexistente
- ✔️ Contraseña incorrecta
- ✔️ Campos vacíos
- ✔️ Usuario baneado
- ✔️ Redirección correcta según rol

### 👥 Administración de Usuarios
- ✔️ Alta de usuarios
- ✔️ Edición de email y contraseña
- ✔️ Eliminación de usuarios
- ✔️ Baneo de usuarios
- ✔️ Búsqueda por nombre

### 🎮 Catálogo XML
- ✔️ Añadir videojuego
- ✔️ Modificar videojuego
- ✔️ Eliminar videojuego
- ✔️ Cargar catálogo desde XML

---

## 🔧 Base de Datos (MySQL)

**Base de datos:** `login_db`  
**Tabla:** `usuarios`

| Campo | Tipo | Descripción |
|------|------|-------------|
| id | INT (AI, PK) | Identificador |
| nombre_usuario | VARCHAR(50) | Usuario |
| password | VARCHAR(50) | Contraseña |
| email | VARCHAR(100) | Email |
| rol | VARCHAR(20) | Admin / Usuario |
| estado | VARCHAR(20) | Activo / Baneado |

---

## 🚀 Ejecución del Proyecto
1. Clonar el repositorio:
```bash
git clone https://github.com/pablocarrillopunina/CatalogoVideojuegosLogin.git

👤 Autor

Pablo Carrillo Punina
2º DAM – IES Rey Fernando VI

