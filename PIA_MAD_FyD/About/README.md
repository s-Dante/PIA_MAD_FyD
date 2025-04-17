//ESte documento explica la estructrura que tiene este proyecto y como fuimos resolviendo cada parte del mismo

/PIA_MAD_FyD

	/Properties						# Archivos generados por la plantilla 
	/References						# Librer�as externas

	/Forms							# Formularios (separados por tipo de usuario)
		/Admin						# Formularios para el Admin
			

		/User						# Formularios para el Usuario
			

		/Shared						# Formularios compartidos por ambos roles
			

	/UserControls					# Controles de usuario reutilizables
		/Admin						# Controles espec�ficos para Admin
			

		/User						# Controles espec�ficos para Usuario
			

		/Shared						# Controles reutilizables por ambos
			 

	/ToolTips&PopUps				# Tooltips, Pop-ups y ventanas emergentes
		PasswordCheck.cs			# Ventana de validaci�n de contrase�a
		InfoPopup.cs				# Ventana emergente informativa
		ErrorPopup.cs				# Ventana emergente de error

	/Data							# Conexi�n a la base de datos
		DbConnection.cs				# Clase para la conexi�n a la BD
		Queries.cs					# M�todos reutilizables de queries
		DataAccess.cs				# L�gica de acceso a la base de datos
		/Models						# Clases que representan las tablas
			User.cs
			Product.cs
			Order.cs

	/Services						# L�gica de negocio (abstracci�n de la BD)
		AuthService.cs				# Servicios de autenticaci�n
		UserService.cs				# L�gica relacionada con los usuarios
		AdminService.cs				# L�gica relacionada con el admin
		ReportService.cs			# Generaci�n de reportes

	/Helpers						# Funciones auxiliares reutilizables
		Utils.cs					# M�todos est�ticos gen�ricos
		ValidationHelper.cs			# M�todos para validaciones
		FormatHelper.cs				# M�todos para formato de texto, fechas, etc.

	/Assets							# Recursos (im�genes, �conos, etc.)
		Images                      # Im�genes para UI
		Icons						# �conos para botones, men�s, etc.
		Fonts						# Fuentes personalizadas

	App.config						# Configuraci�n principal del proyecto
	Program.cs						# Punto de entrada de la aplicaci�n
	MainForm.cs						# Formulario principal
