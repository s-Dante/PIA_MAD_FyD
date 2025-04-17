//ESte documento explica la estructrura que tiene este proyecto y como fuimos resolviendo cada parte del mismo

/PIA_MAD_FyD

	/Properties						# Archivos generados por la plantilla 
	/References						# Librerías externas

	/Forms							# Formularios (separados por tipo de usuario)
		/Admin						# Formularios para el Admin
			

		/User						# Formularios para el Usuario
			

		/Shared						# Formularios compartidos por ambos roles
			

	/UserControls					# Controles de usuario reutilizables
		/Admin						# Controles específicos para Admin
			

		/User						# Controles específicos para Usuario
			

		/Shared						# Controles reutilizables por ambos
			 

	/ToolTips&PopUps				# Tooltips, Pop-ups y ventanas emergentes
		PasswordCheck.cs			# Ventana de validación de contraseña
		InfoPopup.cs				# Ventana emergente informativa
		ErrorPopup.cs				# Ventana emergente de error

	/Data							# Conexión a la base de datos
		DbConnection.cs				# Clase para la conexión a la BD
		Queries.cs					# Métodos reutilizables de queries
		DataAccess.cs				# Lógica de acceso a la base de datos
		/Models						# Clases que representan las tablas
			User.cs
			Product.cs
			Order.cs

	/Services						# Lógica de negocio (abstracción de la BD)
		AuthService.cs				# Servicios de autenticación
		UserService.cs				# Lógica relacionada con los usuarios
		AdminService.cs				# Lógica relacionada con el admin
		ReportService.cs			# Generación de reportes

	/Helpers						# Funciones auxiliares reutilizables
		Utils.cs					# Métodos estáticos genéricos
		ValidationHelper.cs			# Métodos para validaciones
		FormatHelper.cs				# Métodos para formato de texto, fechas, etc.

	/Assets							# Recursos (imágenes, íconos, etc.)
		Images                      # Imágenes para UI
		Icons						# Íconos para botones, menús, etc.
		Fonts						# Fuentes personalizadas

	App.config						# Configuración principal del proyecto
	Program.cs						# Punto de entrada de la aplicación
	MainForm.cs						# Formulario principal
