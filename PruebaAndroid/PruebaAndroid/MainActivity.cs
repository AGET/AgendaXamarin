using Android.App;
using Android.Widget;
using Android.OS;
using System;


namespace PruebaAndroid
{
	// Actividad principal de aplicación de lanzamiento 
	[Activity (Label = "Agenda", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
	public class MainActivity : Activity
	{
		String index ="";
		// Clase de base de datos nuevo objeto 
		Database sqldb;
		// Nombre, Apellidos y Edad EditText para la entrada de datos 
		EditText txtName, txtAge, txtLastName;
		// Objeto Message TextView para la visualización de datos
		//TextView shMsg;
		// Añadir, Editar, Eliminar y Buscar ImageButton objetos para los eventos de manipulación 
		ImageButton imgAdd, imgEdit, imgDelete, imgSearch;
		// Objeto ListView para mostrar los datos de la base de datos 
		ListView listItems;
		// Inicia el evento Crear de aplicación 
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Establecer nuestro diseño principal como vista predeterminada 
			SetContentView (Resource.Layout.Main);
			// Inicializa nueva base de datos objeto de clase 
			sqldb = new Database("person_db");
			// Obtiene instancias de objetos ImageButton 
			imgAdd = FindViewById<ImageButton> (Resource.Id.imgAdd);
			imgDelete = FindViewById<ImageButton> (Resource.Id.imgDelete);
			imgEdit = FindViewById<ImageButton> (Resource.Id.imgEdit);
			imgSearch = FindViewById<ImageButton> (Resource.Id.imgSearch);
			// Obtiene instancias de objetos EditText 
			txtAge = FindViewById<EditText> (Resource.Id.txtAge);
			txtLastName = FindViewById<EditText> (Resource.Id.txtLastName);
			txtName = FindViewById<EditText> (Resource.Id.txtName);
			// Obtiene instancias de objeto TextView 
			//shMsg = FindViewById<TextView> (Resource.Id.shMsg);
			// Obtiene instancia de objeto ListView 
			listItems = FindViewById<ListView> (Resource.Id.listItems);
			// Establece la propiedad clase de mensaje de base de datos a la instancia shMsg TextView 
			//shMsg.Text = sqldb.Message;
			// Crea ImageButton clic eventos para imgAdd, ImgEdit, imgDelete y imgSearch 
			imgAdd.Click += delegate {
				// Llama a la función addRecord para añadir un nuevo récord 
				if(txtName.Text != ""){
				sqldb.AddRecord (txtName.Text, txtLastName.Text, int.Parse (txtAge.Text));
			//	shMsg.Text = sqldb.Message;
				Toast.MakeText(this,"Datos agregados",ToastLength.Short).Show();
				txtName.Text = txtAge.Text = txtLastName.Text = "";
				GetCursorView();
				}else{
					//shMsg.Text = "Ingrese datos";
					Toast.MakeText(this,"Ingrese datos",ToastLength.Short).Show();
				}
			};

			imgEdit.Click += delegate {
				if(txtName.Text != ""){
				//int iId = int.Parse(shMsg.Text);
				int iId = int.Parse(index);
				// Llama a la función UpdateRecord para la actualización de un registro existente 
				sqldb.UpdateRecord (iId, txtName.Text, txtLastName.Text, int.Parse (txtAge.Text));
				//shMsg.Text = sqldb.Message;
				Toast.MakeText(this,"Editado",ToastLength.Short).Show();
				txtName.Text = txtAge.Text = txtLastName.Text = "";
				GetCursorView();
				}else{
					//shMsg.Text = "Seleccione un dato";
					Toast.MakeText(this,"Seleccione un dato",ToastLength.Short).Show();
				}
			};

			imgDelete.Click += delegate {
				if(txtName.Text != ""){
				//int iId = int.Parse(shMsg.Text);
				int iId = int.Parse(index);
				// Llama a la función DeleteRecord para borrar el registro asociado al parámetro id 
				sqldb.DeleteRecord (iId);
				//shMsg.Text = sqldb.Message;
				Toast.MakeText(this,"Eliminado",ToastLength.Short).Show();
				txtName.Text = txtAge.Text = txtLastName.Text = "";
				GetCursorView();
			}else{
					//shMsg.Text = "Seleccione un dato";
					Toast.MakeText(this,"Seleccione un dato",ToastLength.Short).Show();
				}
			};

			imgSearch.Click += delegate {
				// Llama a la función GetCursorView para buscar todos los registros o registro único de acuerdo a los criterios de búsqueda 
				string sqldb_column = "";
				if (txtName.Text.Trim () != "") 
				{
					sqldb_column = "Name";
					GetCursorView (sqldb_column, txtName.Text.Trim ());
				} else
					if (txtLastName.Text.Trim () != "") 
					{
						sqldb_column = "LastName";
						GetCursorView (sqldb_column, txtLastName.Text.Trim ());
					} else
						if (txtAge.Text.Trim () != "") 
						{
							sqldb_column = "Age";
							GetCursorView (sqldb_column, txtAge.Text.Trim ());
						} else 
						{
							GetCursorView ();
							sqldb_column = "en Todos";
						}
				//shMsg.Text = "Buscados " + sqldb_column + ".";
				Toast.MakeText(this, "Buscados " + sqldb_column + ".",ToastLength.Short).Show();
			};
			// Agregar controlador de eventos ItemClick a instancia ListView 
			listItems.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs> (item_Clicked);
		}
		// Lanzado cuando un elemento ListView se hace clic 
		void item_Clicked (object sender, AdapterView.ItemClickEventArgs e)
		{
			// Obtiene TextView instancia de objeto de diseño record_view 
			TextView shId = e.View.FindViewById<TextView> (Resource.Id.Id_row);
			TextView shName = e.View.FindViewById<TextView> (Resource.Id.Name_row);
			TextView shLastName = e.View.FindViewById<TextView> (Resource.Id.LastName_row);
			TextView shAge = e.View.FindViewById<TextView> (Resource.Id.Age_row);
			// Lee valores y conjuntos de instancias de objetos EditText 
			txtName.Text = shName.Text;
			txtLastName.Text = shLastName.Text;
			txtAge.Text = shAge.Text;
			// Muestra mensajes para operaciones CRUD 
			//shMsg.Text = shId.Text;
			index = shId.Text;
		}
		// Obtiene la vista del cursor para mostrar todos los registros 
		void GetCursorView()
		{
			Android.Database.ICursor sqldb_cursor = sqldb.GetRecordCursor ();
			if (sqldb_cursor != null) 
			{
				sqldb_cursor.MoveToFirst ();
				string[] from = new string[] {"_id","Name","LastName","Age" };
				int[] to = new int[] {
					Resource.Id.Id_row,
					Resource.Id.Name_row,
					Resource.Id.LastName_row,
					Resource.Id.Age_row
				};
				// Crea un SimplecursorAdapter para el objeto ListView 
				SimpleCursorAdapter sqldb_adapter = new SimpleCursorAdapter (this, Resource.Layout.record_view, sqldb_cursor, from, to);
				listItems.Adapter = sqldb_adapter;
			} 
			else 
			{
				//shMsg.Text = sqldb.Message;
				Toast.MakeText(this,"No hay contactos",ToastLength.Short).Show();
			}
		}
		// Obtiene la vista del cursor para mostrar los registros de acuerdo con los criterios de búsqueda 
		void GetCursorView (string sqldb_column, string sqldb_value)
		{
			Android.Database.ICursor sqldb_cursor = sqldb.GetRecordCursor (sqldb_column, sqldb_value);

			if (sqldb_cursor != null) 
			{
				sqldb_cursor.MoveToFirst ();
				string[] from = new string[] {"_id","Name","LastName","Age" };
				int[] to = new int[] 
				{
					Resource.Id.Id_row,
					Resource.Id.Name_row,
					Resource.Id.LastName_row,
					Resource.Id.Age_row
				};
				SimpleCursorAdapter sqldb_adapter = new SimpleCursorAdapter (this, Resource.Layout.record_view, sqldb_cursor, from, to);
				listItems.Adapter = sqldb_adapter;
			} 
			else 
			{
				Toast.MakeText(this,"No hay contactos",ToastLength.Short).Show();
			}
		}

	}
}


