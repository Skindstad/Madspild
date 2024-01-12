# Madspild

1. Inde på Madspild er der en mappe hedder "DataAndQuerys"
2. Åben Microsoft SQL Server Management Studio.
3. Klik på Connect
4. Åben "OprettesAfDB" inde på "DataAndQuerys"
5. Execute
6. Åben "Tables" inde på "DataAndQuerys"
7. Execute
8. Vælge at du på "FoodWaste"
9. Marker alle Tables på siden og Execute
10. Åben "Data" Inde på "DataAndQuerys"
11. Tryk på Execute
12. Åben "Madspild Solution" der er på "Madspild" Eller oppen Visual Studio og find Madspild file
13 Højre klik på "Madspild" i "Solution Explorer"
14. Gå ned på "add" og tryk på "new item".
15. Vælg "Application Configuration File" og tryk "Add"
16. Put koden neden under ind på "app.config" mellem "Configuration".
<connectionStrings>
	<add name="post" connectionString="Data Source=YourComputerLokaleHost;;
         Initial Catalog=Databasen;Integrated Security=True" providerName="System.Data.SqlClient"/>
</connectionStrings>
18. Skift Catalog om til "FoodWaste"
19. Åben "Microsoft SQL Server Management Studio". Inden du connect var der en text with din server name tag det.
20. Åben app.config og skift "YourComputerLokaleHost" i Source til dit server name.

Nu kan du køre programmet