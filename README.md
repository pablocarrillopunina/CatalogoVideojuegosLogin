# 🕹️ Catálogo de Videojuegos con Sistema de Login  
**Práctica 3 – Desarrollo de Interfaces (C# / Windows Forms)**  

Este proyecto forma parte de la práctica donde se migra el sistema de autenticación para utilizar una **base de datos MySQL real**, manteniendo el catálogo de videojuegos gestionado mediante **XML**.

Incluye:

- 🔐 **Sistema de Login conectado a MySQL**
- 📝 **Registro de nuevos usuarios (INSERT en MySQL)**
- 🗄️ **Clase DatabaseHelper con conexión real a MySQL**
- 🎮 **Catálogo de Videojuegos usando XML**
- ✔️ **Validaciones completas (campos vacíos, usuario incorrecto, contraseña errónea)**
- 🔄 **Cerrar sesión / Cerrar aplicación**

---

## 📌 Funcionalidades principales

### 🔐 **Login (con MySQL)**
- Valida usuario y contraseña mediante consulta SQL real.
- Consulta a la tabla **usuarios**.
- Manejo de errores de conexión.
- Mensajes claros de éxito o fallo.
- Permite registrarse desde el mismo formulario.

### 📝 **Registro de Usuario (MySQL)**
- Inserción de nuevos usuarios en la base de datos (`INSERT INTO usuarios`).
- Validación de campos vacíos.
- Mensajes de confirmación.

### 🎮 **Catálogo de Videojuegos (XML)**
Se mantiene toda la funcionalidad de la práctica anterior:

- Añadir juegos  
- Modificar juegos  
- Eliminar juegos  
- Cargar juegos desde un archivo XML  
- Mostrar el catálogo en un `DataGridView`

Cada videojuego contiene:
- Título  
- Desarrollador  
- Plataforma  
- Precio 💶  

---

## 🛠️ **Tecnologías utilizadas**

- **C# .NET 8**
- **Windows Forms**
- **MySQL 8** (autenticación y registro)
- **MySql.Data** (conector .NET)
- **XML para almacenamiento del catálogo**
- **GitHub para control de versiones**

---

## 📁 Estructura del proyecto

```
/PracticaXMLDinamica
│── Data/
│   └── DatabaseHelper.cs   ← Conexión MySQL
│── Resources/
│   └── login.jpg
│── FormLogin.cs
│── FormRegistro.cs
│── Form1.cs        (Catálogo de Videojuegos)
│── catalogo.xml
│── Interfaz.xml
│── PracticaXMLDinamica.csproj
│── Program.cs
```

---

## 🧪 Testing actualizado (Práctica 3)

### 🔐 Autenticación MySQL
- ✔️ Login exitoso con usuario real  
- ✔️ Login fallido  
- ✔️ Usuario no existente  
- ✔️ Campos vacíos  
- ✔️ Manejo de errores de conexión  
- ✔️ Registro de nuevo usuario (INSERT) → comprobado en MySQL  

### 🎮 Catálogo XML
- ✔️ Añadir juego  
- ✔️ Modificar juego  
- ✔️ Eliminar juego  
- ✔️ Cargar catálogo desde XML  

---

## 🔧 Base de Datos utilizada (MySQL)

**Base de datos:** `login_db`  
**Tabla:** `usuarios`

| Campo | Tipo | Descripción |
|-------|------|-------------|
| id | INT (AI, PK) | Identificador |
| nombre_usuario | VARCHAR(50) | Usuario |
| password | VARCHAR(50) | Contraseña |

---

## 🚀 Ejecución

1. Clona el repositorio:
   ```bash
   git clone https://github.com/pablocarrillopunina/CatalogoVideojuegosLogin.git
   ```
2. Abre el proyecto en Visual Studio.  
3. Asegúrate de tener MySQL activo en el puerto **3306**.  
4. Ejecuta con **F5**.

---

## 👤 Autor
**Pablo Carrillo Punina**  
IES Rey Fernando VI – 2º DAM

**Pablo Carrillo Punina**  
IES Rey Fernando VI – 2º DAM
