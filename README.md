# Historias Clinicas 📖

## Objetivos 📋
Desarrollar un sistema de historias clinicas para un consultorio, que permita la administración y uso de esta. 
De cara a los empleados): Pacientes, Medicos, Empleados, HistoriaClinica, Episodio, Evoluciones, Epicrisis con Diagnostico, etc., como así también, permitir a los pacientes, realizar consultas acerca de su Historia clinica.
Utilizar Visual Studio 2019 preferentemente y crear una aplicación utilizando ASP.NET MVC Core (versión a definir por el docente 3.1 o 6.0).

<hr />

## Enunciado 📢
La idea principal de este trabajo práctico, es que Uds. se comporten como un equipo de desarrollo.
Este documento, les acerca, un equivalente al resultado de una primera entrevista entre el paciente y alguien del equipo, el cual relevó e identificó la información aquí contenida. 
A partir de este momento, deberán comprender lo que se está requiriendo y construir dicha aplicación, 

Deben recopilar todas las dudas que tengan y evacuarlas con su nexo (el docente) de cara al paciente. De esta manera, él nos ayudará a conseguir la información ya un poco más procesada. 
Es importante destacar, que este proceso, no debe esperar a ser en clase; es importante, que junten algunas consultas, sea de índole funcional o técnicas, en lugar de cada consulta enviarla de forma independiente.

Las consultas que sean realizadas por correo deben seguir el siguiente formato:

Subject: [NT1-<CURSO LETRA>-GRP-<GRUPO NUMERO>] <Proyecto XXX> | Informativo o Consulta

Body: 

1.<xxxxxxxx>

2.< xxxxxxxx>


# Ejemplo
**Subject:** [NT1-A-GRP-5] Agenda de Turnos | Consulta

**Body:**

1.La relación del paciente con Turno es 1:1 o 1:N?

2.Está bien que encaremos la validación del turno activo, con una propiedad booleana en el Turno?

<hr />

### Proceso de ejecución en alto nivel ☑️
 - Crear un nuevo proyecto en [visual studio](https://visualstudio.microsoft.com/en/vs/).
 - Adicionar todos los modelos dentro de la carpeta Models cada uno en un archivo separado.
 - Especificar todas las restricciones y validaciones solicitadas a cada una de las entidades. [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1).
 - Crear las relaciones entre las entidades
 - Crear una carpeta Data que dentro tendrá al menos la clase que representará el contexto de la base de datos DbContext. 
 - Crear el DbContext utilizando base de datos en memoria (con fines de testing inicial). [DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-3.1), [Database In-Memory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=vs).
 - Agregar los DbSet para cada una de las entidades en el DbContext.
 - Crear el Scaffolding para permitir los CRUD de las entidades al menos solicitadas en el enunciado.
 - Aplicar las adecuaciones y validaciones necesarias en los controladores.  
 - Realizar un sistema de login con al menos los roles equivalentes a <Usuario Cliente> y <Usuario Administrador> (o con permisos elevados).
 - Si el proyecto lo requiere, generar el proceso de registración. 
 - Un administrador podrá realizar todas tareas que impliquen interacción del lado del negocio (ABM "Alta-Baja-Modificación" de las entidades del sistema y configuraciones en caso de ser necesarias).
 - El <Usuario Cliente> sólo podrá tomar acción en el sistema, en base al rol que tiene.
 - Realizar todos los ajustes necesarios en los modelos y/o funcionalidades.
 - Realizar los ajustes requeridos del lado de los permisos.
 - Todo lo referido a la presentación de la aplicaión (cuestiones visuales).
 
<hr />

## Entidades 📄

- Usuario
- Paciente
- Medico
- Empleado
- HistoriaClinica
- Episodio
- Evolucion
- Notas
- Epicrisis
- Diagnostico

`Importante: Todas las entidades deben tener su identificador unico. Id o <ClassNameId>`

`
Las propiedades descriptas a continuación, son las minimas que deben tener las entidades. Uds. pueden agregar las que consideren necesarias.
De la misma manera Uds. deben definir los tipos de datos asociados a cada una de ellas, como así también las restricciones.
`

**Usuario**
```
- Nombre
- Email
- FechaAlta
- Password
```

**Paciente**
```
- Nombre
- Apellido
- DNI
- Telefono
- Direccion
- FechaAlta
- Email 
- ObraSocial
- HistoriaClinica
```

**Medico**
```
- Nombre
- Apellido
- DNI
- Telefono
- Direccion
- FechaAlta
- Email
- Matricula
- Especialidad
```

**Empleado**
```
- Nombre
- Apellido
- DNI
- Telefono
- Direccion
- FechaAlta
- Email
- Legajo
```

**HistoriaClinica**
```
- Paciente
- Episodios
```

**Episodio**
```
- Motivo
- Descripcion
- FechaYHoraInicio
- FechaYHoraAlta
- FechaYHoraCierre
- EstadoAbierto
- Evoluciones
- Epicrisis
- EmpleadoRegistra
```

**Evolucion**
```
- Medico
- FechaYHoraInicio
- FechaYHoraAlta
- FechaYHoraCierre
- DescripcionAtencion
- EstadoAbierto
- Notas 
```

**Nota**
```
- Evolucion
- Empleado
- Mensaje
- FechaYHora
```

**Epicrisis**
```
- Episodio
- Medico
- FechaYHora 
- Diagnostico
```

**Diagnostico**
```
- Epicrisis
- Descripcion
- Recomendacion
```

**Direccion**🟢
```
- Calle
- Altura
- Localidad
```


**NOTA:** aquí un link para refrescar el uso de los [Data annotations](https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/).

<hr />

## Caracteristicas y Funcionalidades ⌨️
`Todas las entidades, deben tener implementado su correspondiente ABM, a menos que sea implicito el no tener que soportar alguna de estas acciones.`

`IMPORTANTE: Ninguna entidad en el circuito de atención medica, puede ser modificado o eliminado una vez que se ha creado. Ej. No se puede Eliminar una Historia Clinica, No se puede modificar una nota de una evolución, etc.`

**Usuario**
- Los Pacientes pueden auto registrarse.
- La autoregistración desde el sitio, es exclusiva para los pacientes. Por lo cual, se le asignará dicho rol.
- Los empleados, deben ser agregados por otro Empleado. Lo mismo, para los Medicos.
	- Al momento, del alta del empleado o medico, se le definirá un username y password.
    - También se le asignará a estas cuentas el rol de empleado y/o medico según corresponda.

**Paciente**
- Un paciente puede consultar su historia clinica, con todos los detalles que la componen, en modo solo visualización.
- Puede acceder a los episodios, y por cada episodio, ver las evoluciones que se tienen, con sus detalles.
- Puede actualizar datos de contacto, como el telefono, dirección,etc.. Pero no puede modificar su DNI, Nombre, Apellido, etc.

**Empleado**
- Un empleado, puede modificar todos los datos de los pacientes. 
-- No puede quitar o asociar una nueva Historia Clinica a los pacientes.
- El Empleado puede listar todos los pacientes, y por cada uno, ver en sus detalles, la HistoriaClinica que tiene asociada y si tiene episodios abiertos. 
- El Empleado, puede crear un paciente, un empleado, y un medico. Cada uno de ellos, con su correspondientes datos requeridos y usuario.
- El Empleado, puede crear un Episodio para el Paciente, en la Historia Clinica del paciente.
-- Pero no puede hacer más nada, que crearlo con su Motivo y Descripción.

**Medico**
- Un Medico, puede crear evoluciones, en Episodios que esten en estado abierto.
-- Para ello, buscará al paciente, accederá a su Historia Clinica -> Episodio -> Crear la Evolución.
- Un medico puede cerrar una evlución, si se han completado todos los campos. El campo de FechaYHoraCierre, se guardará automaticamente. 
-- Un Empleado o Medico, pueden cargar notas en cada evolución según sea necesario.
-- Las notas pueden continuar agregandose, luego del cierre de la evolución.
- Puede cerrar un Episodio, pero para hacer esto, el sistema realizará ciertas validaciones.

**HistoriaClinica**
- La misma se crea automaticamente con la creación de un paciente.
-- No se puede eliminar, ni realizar modificaciones posteriores.
-- El detalle internos de la misma, para los Medicos y empleados, pero dependiendo del rol, es lo que podrán hacer.
-- El paciente propietario de la HC, es el unico paciente que puede ver la HC.

- Por medio de la HC, se podrá acceder a la lista de Episodios, que tenga relacionados.

**Epidodio**
- La creación de un Episodio en una HC, solo puede realizarla un empleado.
-- El empleado, deberia acceder a un Paciente -> HC -> Crear Episodio, e ingresará:
--- Motivo. Ej. Traumatismo en pierna Izquierda.
--- Descripción. Ej. El paciente se encontraba andando en Skate y sufrió un accidente.
- El episodio se:
-- Creará en estadoAbierto automaticamente
-- Con una FechaYHoraInicio también, de forma automática.
-- Con un Empleado, como el que creó el episodio. (persona en recepción, que recibe al paciente).

- Solo un medico puede cerrar un Episodio, para hacer esto, el sistema, validará:
-- 1. Que el Episodio, no tenga ninguna Evlución en estado Abierta o no tenga evoluciones. Si fuese así, deberá mostrar un mensaje.
-- 2. Cargará el Medico manualmente la FechaYHoraAlta (alta del episodio) del paciente.
-- 3. Le pedirá que cargue una Epicrisis, con su diagnostico y recomendaciones.
--- Una vez finalizado el diagnostico, el Episodio, pasará a esatr en estado Cerrado.
-- 4. La FechaYHoraCierre, será cargada automaticamente, si se cumplen los requerimientos previos.

Nota: Si el cierre del episodio, es por la condición sin evoluciones, se generará un "Cierre Administrativo", en el cual, el sistema, cargará una epicrisi, con alguna información que el empleado ingresará para dejar registro de que fue un cierre administrativo. Ej. El paciente realizó el ingreso y antes de ser atendido, se fué. 

**Evolucion**
- Una evolución, solo la puede crear y gestionar un Medico.
-- La unica excepción, es que un empleado, puede cargar notas en Evoluciones por cuestiones administrativas. Ej. Salvo, que el alta del paciente en la evolución, es 10/08/2020
- Automaticamente al crear una evolución se cargará:
-- Medico que la esta creando
-- FechaYHoraInicio
-- EstadoAbierto
-- FechaYHoraCierre (Cuando se registre el cierre)
- Manualmente:
-- La FechaYHoraAlta
-- DescripcionAtencion
-- Notas (Las que sean necesarias)

- Para cerrar una evolución, se deben haber cargado todos los datos manuales requeridos, y solo lo puede hacer un Medico.

**Nota**
- La nota pertenece a una evolución. 
-- Para crearla, uno solo puede hacerla desde una Evolución.
- En las notas, puede cargar un mensaje cualquier empleado o medico.
- Quedará automaticmente la fecha y hora, como asi también, quien es el que la cargó.


**Epicrisis**
- La epicrisis, pertenes a un Episodio.
-- Solo puede haber una epicrisis por episodio.
-- Para poder crearla, todas las evoluciones, deben estar cerradas.
-- El Episodio debe estar abierto, y al finalizar este proceso, de estar todo ok, se debe cerrar automaticamente.
-- La epicrisis, solo debe poder cargarla un Medico.
-- La excepción, es la creación automatica, si cierra un empleado, por proceso administrativo.
-- La FechayHora, se carga automaticamente
-- El Diagnostico, de forma Manual.

**Diagnostico**
- Pertenece a una Epicrisis. 
- Se cargará una descripcion de forma manual
- También se cargará una recomendacion.


**Aplicación General**
- Información institucional.
- Se deben listar el cuerpo medico, junto con sus especialidades.
- Los accesos a las funcionalidades y/o capacidades, debe estar basada en los roles que tenga cada individuo.
