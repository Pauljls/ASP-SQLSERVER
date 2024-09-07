create database REGISTRO_MVC
USE REGISTRO_MVC

CREATE TABLE USUARIOS(
	IDUsuario int identity(1,1),
	Nombre varchar(50),
	Edad int,
	Correo varchar(50)
)

select * from Usuarios;

create procedure sp_registrar
@Nombre varchar(50),
@Edad int,
@Correo varchar(50)
as begin 
insert into Usuarios values(@Nombre, @Edad, @Correo)
end

create procedure sp_usuarios
as begin 
select * from Usuarios;
end;

EXEC sp_helptext 'sp_usuarios';

