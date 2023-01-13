This folder contains 3 parts: 
1) Contact Application - Dot net core MVC Solution
2) ContactWebAPI - Dot net core Web API
3) DB Scripts for creating the db tables and sample data. 

Please ensure to run the Web api first and then ensure that the api is running in the below mentioned endpoint : 

1) https://localhost:44305/api/getcontacts _ this will read the sample data from the API. 


The MVC application will call the API for the DB persistance. 

The MVC application has the below funtionalities :
1) Home page - Displays the list of contacts. Create the sample records in the DB. 
2) Create Contact - Creates a new contact
3) Edit Contact - Edits an existing contact
4) Map View -  Finds the latitude and longitude of the given city name and displays in the map. -> For this purpose I have used google maps api whose API key
is currently hard coded in the application. 
5) Error page -> will navigate to error page when something goes wrong. 