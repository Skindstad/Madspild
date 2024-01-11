# Madspild

1. Inde på Madspild er der en mappe hedder "DataAndQuerys"
2. Åben Microsoft SQL Server Management Studio.
3. Klik på Connect
4. Åben "OprettesAfDBOgTables" inde på "DataAndQuerys"
5. Marker Create Database "FoodWaste" og Execute
6. Vælge at du på "FoodWaste"
7. Marker alle Tables på siden og Execute
8. Åben "Data" Inde på "DataAndQuerys"
9. Tryk på Execute
10. Åben "Madspild Solution" der er på "Madspild" Eller oppen Visual Studio og find Madspild file
11 Højre klik på "Madspild" i "Solution Explorer"
12. Gå ned på "add" og tryk på "new item".
13. Vælg "Application Configuration File" og tryk "Add"
14. Put koden neden under ind på "app.config" mellem "Configuration".
<connectionStrings>
	<add name="post" connectionString="Data Source=YourComputerLokaleHost;;
         Initial Catalog=Databasen;Integrated Security=True" providerName="System.Data.SqlClient"/>
</connectionStrings>
15. Skift Catalog om til "FoodWaste"
16. Åben "Microsoft SQL Server Management Studio". Inden du connect var der en text with din server name tag det.
17. Åben app.config og skift "YourComputerLokaleHost" i Source til dit server name.

Nu kan du køre programmet