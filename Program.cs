using System.Runtime.CompilerServices;
using System.Text.Json;


static void crearArchivo(string datoAlmacenar, string rutaCarpeta, string nombre ){
    // Se crearan los archivos del contenido
        string datosIngresar = "";
        int contador = 1;
        for(int i = 0; i < datoAlmacenar.Length; i++){
            datosIngresar += datoAlmacenar[i];
            if (datosIngresar.Length == 20){
                string direccionArch = $"{rutaCarpeta}\\{nombre}-{contador}.json";
                // Serializar el objeto a JSON
                string rutaSiguiente = "";
                if (contador == datoAlmacenar.Length / 20){
                    rutaSiguiente = "null";
                }
                else{
                    rutaSiguiente = $"{rutaCarpeta}\\{nombre}-{contador + 1}.json";
                }

                Archivo archivoNuevo = new Archivo($"{nombre}-{contador + 1}", datosIngresar, rutaSiguiente);
                string jsonString = JsonSerializer.Serialize(archivoNuevo);
                File.WriteAllText(direccionArch, jsonString);
                contador += 1;
                datosIngresar = "";
            }

            else if(datosIngresar.Length < 20  && datoAlmacenar.Length % 20 != 0 && datoAlmacenar.Length / 20 == contador - 1){
                string direccionArch = $"{rutaCarpeta}\\{nombre}-{contador}.json";
                // Serializar el objeto a JSON
                Archivo archivoNuevo = new Archivo($"{nombre}-{contador}", datosIngresar, "null");
                string jsonString = JsonSerializer.Serialize(archivoNuevo);
                File.WriteAllText(direccionArch, jsonString);
            }

        }
}


static void crear(string nombre, string datoAlmacenar){
    string  rutaGeneral  = Directory.GetCurrentDirectory();
    string rutaEvaluar = $"{rutaGeneral}\\archivosTrabajados";
    if (!rutaEvaluar.Contains(nombre)){
        // Crea la carpeta ya que no existe
        string rutaCarpeta = Path.Combine(rutaEvaluar, nombre);
        Directory.CreateDirectory(rutaCarpeta);
        // SE CREARA LA TABLA FAT DEL ARCHIVO
        string direccionTablaFAT = $"{rutaCarpeta}\\TablaFat.json";
        TablaFat tablaFat = new TablaFat(nombre,$"{rutaCarpeta}\\{nombre}-{1}.json", datoAlmacenar.Length);
        // Serializar el objeto a JSON
        string jsonStringFAT = JsonSerializer.Serialize(tablaFat);
        File.WriteAllText(direccionTablaFAT, jsonStringFAT);
        crearArchivo(datoAlmacenar, rutaCarpeta, nombre);   
    }
    
    else{
        Console.WriteLine(@"El nombre del archivo que deseas crear ya existe, 
si quieres modificarlo o eliminarlo entra a las opciones 4 o 5. 
No se creara ningun archivo");
        
    }
}


static void listarArchivo(List<Dictionary<string, Archivo>> archivos, List<TablaFat> tablaFats){
    string  rutaGeneral  = Directory.GetCurrentDirectory();
    string rutaEvaluar = $"{rutaGeneral}/archivosTrabajados";
    string[] subcarpetas = Directory.GetDirectories(rutaEvaluar);
    int contador = 0; 
    
    foreach (string subcarpeta in subcarpetas)
        {
            string[] archivosCarpeta = Directory.GetFiles(subcarpeta);
            int contador2 = 0;
            Dictionary<string, Archivo> diccionario = new Dictionary<string, Archivo>();
            foreach (string archivo in archivosCarpeta)

            {
                if(archivo == $"{subcarpeta}\\TablaFat.json"){
                    contador += 1;
                    string jsonFromFile = File.ReadAllText(archivo);
                    TablaFat var = JsonSerializer.Deserialize<TablaFat>(jsonFromFile)!;
                    tablaFats.Add(var);
                    if(!var.papelera){
                        Console.WriteLine($"----Archivo Número {contador}---- \n-Nombre: {var.nombre} \n-Caracteres: {var.caracteres} \n-Fecha creación: {var.fechaCreacion} \n-Fecha modificación: {var.fechaModificacion} \n");

                    }
                }
                else{
                    contador2 += 1;
                    
                    string jsonFromFile = File.ReadAllText(archivo);
                    Archivo var = JsonSerializer.Deserialize<Archivo>(jsonFromFile)!;
                    diccionario.Add($"Archivo{contador2}", var);

                }
                

            }

            archivos.Add(diccionario);
            }

}


static void abrirArchivo(List<Dictionary<string, Archivo>> archivos, List<TablaFat> tablaFats, int opcion){
    try{
        Dictionary<string, Archivo> archivoAcceder = archivos[opcion - 1];
        string contenido = "";

        foreach(Archivo arch in archivoAcceder.Values){
            contenido += arch.datos;
        }

        TablaFat tablaAcceder = tablaFats[opcion - 1];

        Console.WriteLine($"----Archivo {tablaAcceder.nombre}---- \n-Caracteres: {tablaAcceder.caracteres} \n-Fecha creación: {tablaAcceder.fechaCreacion} \n-Fecha modificación: {tablaAcceder.fechaModificacion} \n Contenido:");
        Console.WriteLine(contenido);
    }
    catch{
        Console.WriteLine("El número enviado no existe");
    }
    

}


static void modificar(string texto, List<Dictionary<string, Archivo>> archivos, List<TablaFat> tablaFats, int opcion){
    try{

        string  rutaGeneral  = Directory.GetCurrentDirectory();
        Dictionary<string, Archivo> archivoAcceder = archivos[opcion - 1];
        TablaFat tablaAcceder = tablaFats[opcion - 1];



        foreach(Archivo arch in archivoAcceder.Values){
            string filePathEliminar = $"{rutaGeneral}\\archivosTrabajados\\{tablaAcceder.nombre}\\{arch.nombre}.json";
            File.Delete(filePathEliminar);
        }

        crearArchivo(texto, $"{rutaGeneral}\\archivosTrabajados\\{tablaAcceder.nombre}", tablaAcceder.nombre);

        tablaAcceder.fechaModificacion = DateTime.Now;
        tablaAcceder.caracteres = texto.Length;


        // Serializar el objeto a JSON
        string filePathTabla = $"{rutaGeneral}\\archivosTrabajados\\{tablaAcceder.nombre}\\TablaFat.json";
        Console.WriteLine($"Aqui estoyy: {filePathTabla}");
        
        
        string jsonString = JsonSerializer.Serialize(tablaAcceder);
        File.WriteAllText(filePathTabla, jsonString);



    }
    catch{
        Console.WriteLine("El número enviado no existe");
    }
    }


static void eliminar(List<Dictionary<string, Archivo>> archivos, List<TablaFat> tablaFats, int opcion){
    try{

        string  rutaGeneral  = Directory.GetCurrentDirectory();
        Dictionary<string, Archivo> archivoAcceder = archivos[opcion - 1];
        TablaFat tablaAcceder = tablaFats[opcion - 1];

        foreach(Archivo arch in archivoAcceder.Values){
            arch.papelera = true;
            string filePathArchivo = $"{rutaGeneral}\\archivosTrabajados\\{tablaAcceder.nombre}\\{arch.nombre}.json";
            string jsonStringArch = JsonSerializer.Serialize(arch);
            File.WriteAllText(filePathArchivo, jsonStringArch);
        }



        
        tablaAcceder.papelera = true;
        tablaAcceder.fechaEliminacion = DateTime.Now;

        // Serializar el objeto a JSON
        string filePathTabla = $"{rutaGeneral}\\archivosTrabajados\\{tablaAcceder.nombre}\\TablaFat.json";
        Console.WriteLine($"Aqui estoyy: {filePathTabla}");
        
        
        string jsonString = JsonSerializer.Serialize(tablaAcceder);
        File.WriteAllText(filePathTabla, jsonString);



    }
    catch{
        Console.WriteLine("El número enviado no existe");
    }
    }


static void listarPapelera(List<Dictionary<string, Archivo>> archivos, List<TablaFat> tablaFats){
    string  rutaGeneral  = Directory.GetCurrentDirectory();
    string rutaEvaluar = $"{rutaGeneral}/archivosTrabajados";
    string[] subcarpetas = Directory.GetDirectories(rutaEvaluar);
    int contador = 0; 
    
    foreach (string subcarpeta in subcarpetas)
        {
            string[] archivosCarpeta = Directory.GetFiles(subcarpeta);
            int contador2 = 0;
            Dictionary<string, Archivo> diccionario = new Dictionary<string, Archivo>();
            foreach (string archivo in archivosCarpeta)

            {
                if(archivo == $"{subcarpeta}\\TablaFat.json"){
                    contador += 1;
                    string jsonFromFile = File.ReadAllText(archivo);
                    TablaFat var = JsonSerializer.Deserialize<TablaFat>(jsonFromFile)!;
                    tablaFats.Add(var);
                    if(var.papelera){
                        Console.WriteLine($"----Archivo Número {contador}---- \n-Nombre: {var.nombre} \n-Caracteres: {var.caracteres} \n-Fecha eliminación: {var.fechaEliminacion}\n");

                    }
                }
                else{
                    contador2 += 1;
                    
                    string jsonFromFile = File.ReadAllText(archivo);
                    Archivo var = JsonSerializer.Deserialize<Archivo>(jsonFromFile)!;
                    diccionario.Add($"Archivo{contador2}", var);

                }
                

            }

            archivos.Add(diccionario);
            }

}


static void recuperar(List<Dictionary<string, Archivo>> archivos, List<TablaFat> tablaFats, int opcion){
    try{

        string  rutaGeneral  = Directory.GetCurrentDirectory();
        Dictionary<string, Archivo> archivoAcceder = archivos[opcion - 1];
        TablaFat tablaAcceder = tablaFats[opcion - 1];

        foreach(Archivo arch in archivoAcceder.Values){
            arch.papelera = false;
            string filePathArchivo = $"{rutaGeneral}\\archivosTrabajados\\{tablaAcceder.nombre}\\{arch.nombre}.json";
            string jsonStringArch = JsonSerializer.Serialize(arch);
            File.WriteAllText(filePathArchivo, jsonStringArch);
        }

        
        tablaAcceder.papelera = false;

        // Serializar el objeto a JSON
        string filePathTabla = $"{rutaGeneral}\\archivosTrabajados\\{tablaAcceder.nombre}\\TablaFat.json";
        Console.WriteLine($"Aqui estoyy: {filePathTabla}");
        
        
        string jsonString = JsonSerializer.Serialize(tablaAcceder);
        File.WriteAllText(filePathTabla, jsonString);



    }
    catch{
        Console.WriteLine("El número enviado no existe");
    }
    }


static void main(){
    string nombreCarpeta = "archivosTrabajados";
    if (!Directory.Exists(nombreCarpeta)){
        Directory.CreateDirectory(nombreCarpeta);
    }
    
    // listas
    List<Dictionary<string, Archivo>>archivos=[];
    List<TablaFat> tablas = [];

    while (true){
        // Menú
        Console.WriteLine(@"----Inicio----
1. Crear un archivo y agregar datos
2. Listar Archivos
3. Abrir un archivo
4. Modificar un archivo
5. Eliminar un Archivo
6. Recuperar un archivo
7. Salir");
        string opcion = Console.ReadLine()!;

        // opciones
        if (opcion == "1"){
            Console.Clear();
            Console.WriteLine("--Aquí crearas un nuevo archivo--");
            Console.WriteLine("Ingresa el nombre de tu archivo:");
            string nombre = Console.ReadLine()!;
            Console.WriteLine("Ingresa el texo que desees almacenar");
            string datoAlmacenar = Console.ReadLine()!;
            crear(nombre, datoAlmacenar);
        }
        
        else if (opcion == "2"){
            Console.Clear();
            listarArchivo(archivos, tablas);
            archivos = [];
            tablas = [];
        }
        
        else if (opcion == "3"){
            Console.Clear();
            listarArchivo(archivos, tablas);
            Console.WriteLine("Ingresa el NÚMERO del archivo que desees ver");
            string opcionArchivo = Console.ReadLine()!;
            abrirArchivo(archivos, tablas, int.Parse(opcionArchivo));
            archivos = [];
            tablas = [];
        }

        else if (opcion == "4"){
            Console.Clear();
            listarArchivo(archivos, tablas);
            Console.WriteLine("Ingresa el NÚMERO del archivo que desees modificar");
            string opcionArchivo = Console.ReadLine()!;
            abrirArchivo(archivos, tablas, int.Parse(opcionArchivo));

            Console.WriteLine("Ingrese el texto nuevo (presiona 'Escape' para guardar y salir):");
            string texto = "";

            while (true){
            var keyInfo = Console.ReadKey(intercept: true);

            // Verifica si se presiona Escape
            if (keyInfo.Key == ConsoleKey.Escape){
                break; // Sale del bucle
            }

            // Si no es Escape, agrega el carácter al texto
            texto += keyInfo.KeyChar;
            Console.Write(keyInfo.KeyChar);}

            Console.WriteLine("Deseea modificar el archivo (si/no):");
            string opcionVer = Console.ReadLine()!;

            if  (opcionVer == "si"){
                modificar(texto, archivos, tablas, int.Parse(opcionArchivo));

            } else{
                Console.WriteLine("No se modificara nada");

            }


            archivos = [];
            tablas = [];

        }

        else if(opcion == "5"){
            Console.Clear();
            listarArchivo(archivos, tablas);
            Console.WriteLine("Ingresa el NÚMERO del archivo que desees eliminar");
            string opcionArchivo = Console.ReadLine()!;
            abrirArchivo(archivos, tablas, int.Parse(opcionArchivo));

            Console.WriteLine("Deseea eliminar el archivo (si/no):");
            string opcionVer = Console.ReadLine()!;

            if  (opcionVer == "si"){
                eliminar(archivos, tablas, int.Parse(opcionArchivo));

            } else{
                Console.WriteLine("No se eliminara nada");

            }
            
            archivos = [];
            tablas = [];
        }
        
        else if(opcion == "6"){
            Console.Clear();
            listarPapelera(archivos, tablas);
            Console.WriteLine("Ingresa el NÚMERO del archivo que desees recuperar");
            string opcionArchivo = Console.ReadLine()!;
            abrirArchivo(archivos, tablas, int.Parse(opcionArchivo));
            
            Console.WriteLine("Deseea recuperar el archivo (si/no):");
            string opcionVer = Console.ReadLine()!;

            if  (opcionVer == "si"){
                recuperar(archivos, tablas, int.Parse(opcionArchivo));

            } else{
                Console.WriteLine("No se recuperara nada");

            }
            
            archivos = [];
            tablas = [];
        }
        
        else if(opcion == "7"){
            break;
        }
 
    }
}

    

main();