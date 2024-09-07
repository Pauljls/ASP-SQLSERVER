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

create procedure sp_usuario
@id int
as begin 
select * from Usuarios where IDUsuario=@id;
end;

create procedure sp_actualizar
@id int,
@Nombre varchar(10),
@Edad int,
@Correo varchar(20)
as begin
update Usuarios
set Nombre = @Nombre,
Edad = @Edad,
Correo = @Correo
where IDUsuario = @id
end

Create procedure sp_delete
@id int
as begin 
delete from Usuarios where IDUsuario = @id
end

delete from Usuarios where IDUsuario = 7

EXEC sp_helptext 'sp_actualizar';

