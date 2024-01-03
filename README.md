# Madspild

1. Laven en App.config hvor du putter denne har kode mellem Configuration:
    <add name="post" connectionString="Data Source=YourComputerLokaleHost;
         Initial Catalog=Databasen;Integrated Security=True" providerName="System.Data.SqlClient"/>
  </connectionStrings>
Lave om på Hvor der står YourComputerLokaleHost som skal være det du finder nå du åbner Microsoft SQL Server Management Studio. Tage det der står i Server name.
Lave op på Database der står i catalog op i koden det skal være navnet på databasen som er FoodWaste.